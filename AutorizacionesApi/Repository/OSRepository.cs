using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApi.Models;
using WebApi.Models.Autorizacion;
using WebApi.Models.OS;
using WebApi.Models.Prestacion;
using WebApi.Repository.OS;
using WebApi.Utils;

namespace WebApi.Repository
{
    public class OSRepository
    {
        public List<Os> OSList { get; set; }

        public List<Os> List(int userId)
        {
            var query = "";
            try
            {
                var c = new Connection();

                query += " SELECT ows.nid_osocwebservices, ows.cLeyenda, p.idpre";
                query += " FROM sociospad s";
                query += " INNER JOIN prestador p ON s.nnro_pre = p.nnro_pre AND s.ctip_pre = p.ctip_pre";
                query += " INNER JOIN usuarios_prestadores u ON u.idpre = p.idpre";
                query += " INNER JOIN nompadsoc n ON s.cpadmedi = n.cpadmedi";
                query += " INNER JOIN osocial o ON o.ncodosoc = n.ncodosoc";
                query += " INNER JOIN obrasocial_webservices ows ON ows.ncodosoc = n.ncodosoc";
                query += " WHERE s.dfec_ini < SYSDATE AND (s.dfec_fin > SYSDATE OR s.dfec_fin IS NULL) AND ows.nestado >= 0";
                query += " AND u.nid_usuario = " + userId + " AND n.nestado = 0";  // AND p.idpre =  " + idPre;

                var dt = c.Query(query);

                List<Os> list = (from DataRow dr in dt.Rows
                                    select new Os
                                    {
                                        Id = (int)dr.ItemArray[0],
                                        Name = dr.ItemArray[1].ToString().Trim(),
                                        Idpre = (int)dr.ItemArray[2]
                                    }).ToList();

                return list;
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", userId, query);
            }

            return null;
        }

        public List<Nomenclador> Nomenclador(Search search)
        {
            var query = "";
            try
            {
                var c = new Connection();
                query += " SELECT DISTINCT N.ccodprest, N.CREFPREST";
                query += " FROM sistema.nomencla N";
                query += " INNER JOIN sistema.osocplan OSP on N.ccodnomen = OSP.ccodnomen OR N.ccodnomen = OSP.csecnomen";
                query += " INNER JOIN obrasocial_webservices WS on WS.ncodosoc = OSP.ncodosoc";
                query += " WHERE N.ccodprest = " + search.Cod + " and WS.nid_osocwebservices = " + search.OsId;

                var dt = c.Query(query);

                return (from DataRow dr in dt.Rows
                        select new Nomenclador
                        {
                            CodPres = dr.ItemArray[0].ToString().Trim(),
                            Descripcion = dr.ItemArray[1].ToString().Trim()
                        }).ToList();
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", search, query);
            }

            return null;
        }

        public Afiliado Elegibilidad(Elegibilidad model)
        {
            try
            {
                var prestador = new PrestadorRepository();
                var matricula = 0;
                switch (model.OsId)
                {
                    case 0:
                        break;
                    case 1://Swiss Medical
                        var swiss = new OSSwiss();
                        return swiss.Eligibilidad(model.Credencial);
                    case 2:
                        var acaSalud = new OSAcaSalud();

                        matricula = prestador.GetMatriculaFromIdPre(model.IdPre);
                        return acaSalud.Elegibilidad(model.Credencial, matricula);
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                    case 8:
                        var boreal = new OSBoreal();
                        return boreal.Eligibilidad(model.Credencial);
                    case 6:
                        var medife = new OSMedife();
                        return medife.Eligibilidad(model.Credencial);
                    case 7:
                        var redSeguros = new OSRedSeguros();
                        return redSeguros.Eligibilidad(model.Credencial);
                    case 9:
                        var sancor = new OSSancor();
                        matricula = prestador.GetMatriculaFromIdPre(model.IdPre);
                        return sancor.Elegibilidad(model.Credencial);
                    case 10:
                        var lyf = new OSLuzFuerza();
                        var datos = prestador.GetInfoFromIdPre(model.IdPre);

                        Afiliado afiliado = lyf.Eligibilidad(model.Credencial, Convert.ToInt32(datos.Matricula)).Result;
                        if (!afiliado.HasError)
                        {
                            Authorize autoriza = new Authorize(model.OsId, model.IdPre, model.IdPre, model.Credencial,string.Empty, new List<Prestacion>(), model.UserId);

                            autoriza.AfiliadoNombre = afiliado.Name;
                            autoriza.AfiliadoPlan = "";
                            
                            autoriza.Efector = new Efector();
                            autoriza.Efector.Matricula = matricula;
                            autoriza.Efector.Name = datos.Name;

                            autoriza.Prestaciones = new List<Prestacion>();
                            var presta = new Prestacion();
                            presta.Cant = 1;
                            presta.CodPres = "420101";
                            presta.Descripcion = "CONSULTA MEDICA";

                            autoriza.Prestaciones.Add(presta);

                            var autorizacionOs = lyf.Autorizar(autoriza, afiliado.Plan);

                            if (!autorizacionOs.HasError)
                            {
                                var autorizacionRepository = new AutorizacionRepository();
                                var authNr = autorizacionRepository.Autorizar(autorizacionOs);
                                afiliado.Nr = authNr.ToString();
                            }

                        }
                        else
                        {
                            if (afiliado.Name == "afiliado inexistente")
                            {
                                afiliado.HasError = false;
                            }
                            else
                            {
                                if (afiliado.Name == "afiliado con 2 consultas realizadas en el mes")
                                {
                                    afiliado.Name = "El afiliado supero el límite de consumo mensual. Por favor digerirse a las oficinas de la Obra Social para la autorización de la práctica. ";
                                }
                                else
                                {
                                    if (afiliado.Name != "Error el formato del carnet es incorrecto! Ej. xx-xxxxx-x/xx")
                                    {
                                        afiliado.SetError(GetType().Name, 0, "Luz y Fuerza: " + afiliado.Name, string.Empty, model.Credencial + ";" + datos.Matricula, string.Empty);
                                        afiliado.Name = "Se ha producido un error desconocido. Por favor comunicarse con el Área de Sistemas del Circulo Medico de Salta";
                                    }

                                }
                            }
                        }

                        afiliado.Plan = "";
                        return afiliado;
                        //case 11:
                        //    var os = new OSOspatrones();
                        //    matricula = prestador.GetMatriculaFromIdPre(model.IdPre);
                        //    return os.Eligibilidad(model.Credencial, matricula);
                }

            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, model, string.Empty);
            }
            return new Afiliado();
        }

        public List<CarnetSearch> GetCarnetList(int prestadorId, string osId, string cod)
        {
            var query = "";
            try
            {
                var c = new Connection();

                query += "select distinct A.NNRO_AFILIADO from autorizaciones A inner join OBRASOCIAL_WEBSERVICES O on O.NCODOSOC = A.NCODOSOC where O.NID_OSOCWEBSERVICES = " + osId + " and A.IDPRE = " + prestadorId + " and NNRO_AFILIADO like '" + cod.Trim() +"%' order by A.NNRO_AFILIADO desc";
                
                var dt = c.Query(query);

                return (from DataRow dr in dt.Rows
                        select new CarnetSearch
                        {
                            Descripcion = dr.ItemArray[0].ToString().Trim()
                        }).ToList();
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", prestadorId + ", " + osId, query);
            }
            return null;
        }

        public PrestacionSearch NombrePractica(string codigo, string OsId)
        {
            var query = "";
            try
            {
                codigo = codigo.Trim().Replace(".", string.Empty).Replace(",", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);

                var c = new Connection();
                
                query += " SELECT DISTINCT N.ccodprest, N.CREFPREST";
                query += " FROM sistema.nomencla N";
                query += " INNER JOIN sistema.osocplan OSP on N.ccodnomen = OSP.ccodnomen OR N.ccodnomen = OSP.csecnomen";
                query += " INNER JOIN obrasocial_webservices WS on WS.ncodosoc = OSP.ncodosoc";
                query += " WHERE N.ccodprest = '" + codigo + "' AND WS.nid_osocwebservices = " + OsId + " and N.nestado=0";

                var dt = c.Query(query);

                var descripcion = "Código Inexistente";
                foreach (DataRow dr in dt.Rows)
                {
                    descripcion = dr.ItemArray[1].ToString().Trim();
                }

                var p = new PrestacionSearch
                {
                    Descripcion = descripcion,
                    Cod = codigo
                };
                return p;
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", codigo + ", " + OsId, query);
            }
            return null;
        }

        public List<PrestacionSearch> GetPracticaByDesc(string desc, string osId)
        {
            var query = "";
            try
            {
                var c = new Connection();
                query += " SELECT DISTINCT N.ccodprest, N.CREFPREST";
                query += " FROM sistema.nomencla N";
                query += " INNER JOIN sistema.osocplan OSP ON (N.ccodnomen = OSP.ccodnomen OR N.ccodnomen = OSP.csecnomen)";
                query += " INNER JOIN obrasocial_webservices OSW ON OSP.ncodosoc = OSW.ncodosoc";
                query += " WHERE UPPER(N.CREFPREST) LIKE  '%" + desc.ToUpper() + "%' AND OSW.nid_osocwebservices = " + osId + " AND N.nestado=0";
                query += " ORDER BY N.CREFPREST ASC";
                var dt = c.Query(query);

                return (from DataRow dr in dt.Rows
                        select new PrestacionSearch
                        {
                            Cod = dr.ItemArray[0].ToString().Trim(),
                            Descripcion ="(" + dr.ItemArray[0].ToString().Trim() + ") - " + dr.ItemArray[1].ToString().Trim()
                        }).ToList();
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", desc + ", " + osId, query);
            }
            return null;
        }

        public int GetOSbyId(int osId)
        {
            var query = "";
            try
            {
                query = "SELECT NCODOSOC FROM OBRASOCIAL_WEBSERVICES WHERE NID_OSOCWEBSERVICES=" + osId;
                var c = new Connection();
                var obj = c.QueryObject(query);
                return Convert.IsDBNull(obj) ? 0 : Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", osId, query);
            }
            return 0;
        }

        public string GetPrescDesc(string codPres, List<Prestacion> prestaciones, Authorize model)
        {
            try
            {
                foreach (var prestacion in prestaciones)
                {
                    if (codPres.ToUpper().Trim() != prestacion.CodPres.ToUpper().Trim()) continue;
                    if (prestacion.Descripcion == null)
                    {
                        return NombrePractica(codPres, model.OSId.ToString()).Descripcion;
                    }

                    var index = prestacion.Descripcion.IndexOf(" - ", StringComparison.Ordinal);
                    if (index > 0)
                    {
                        return prestacion.Descripcion.Substring(index + 2, prestacion.Descripcion.Length - index - 2)
                            .Trim();
                    }
                    return prestacion.Descripcion.Trim();
                }

            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", prestaciones, string.Empty);
            }
            return string.Empty;
        }

        public List<string> fetchAfiliados(CredencialesXOs model)
        {
            var query = "";
            try
            {
                query = "select distinct  REPLACE(nnro_afiliado,'800006','') from desaosoc.AUTORIZACIONES A inner join desaosoc.OBRASOCIAL_WEBSERVICES Os on Os.NCODOSOC = A.NCODOSOC where OS.NID_OSOCWebservices = " + model.OSId + " and idpre =" + model.PrestadorId;
                var c = new Connection();
                var dt = c.Query(query);

                var list = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr.ItemArray[0].ToString().Trim());
                }
                return list;
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, query);
            }
            return new List<string>();
        }
    }
}
