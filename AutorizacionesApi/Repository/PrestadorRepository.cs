using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApi.Models;
using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Repository
{
    public class PrestadorRepository
    {
        public int Login(string userName, string passWord)
        {
            var iNidUsuario = -1;
            var iNivel = 0;
            var query = "";
            try
            {
                query = "select NID_USUARIO, NID_NIVEL from USUARIOS where CLOGIN_USUARIO='" + userName + "' and CCLAVE_USUARIO='" + passWord + "'";
                var c = new Connection();
                var dt = c.Query(query);

                foreach (DataRow dr in dt.Rows)
                {
                    iNidUsuario = Convert.ToInt32(dr.ItemArray[0]);
                    iNivel = Convert.ToInt32(dr.ItemArray[1]);
                }
                
                OsStatus.TimeOut = null;
            }
            catch (Exception ex)
            {
                OsStatus.TimeOut = DateTime.Now;
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", userName + ", " + passWord, query);
            }
            return iNidUsuario;
        }

        public List<Prestador> Prestadores(string id)
        {
            var query = "";
            try
            {
                if (id != string.Empty)
                {
                    query = "SELECT UP.IDPRE, CAPE_NOM, UP.CLASIF_USUARIO FROM USUARIOS_PRESTADORES UP, PRESTADOR P WHERE P.NESTADO=0 AND UP.IDPRE=P.IDPRE AND UP.NID_USUARIO=" + id;
                    var c = new Connection();
                    var dt = c.Query(query);

                    var list = (from DataRow dr in dt.Rows
                                select new Prestador
                                {
                                    Id = Convert.ToInt32(dr.ItemArray[0]),
                                    Name = dr.ItemArray[1].ToString().Trim(),
                                    Type = Convert.ToInt32(dr.ItemArray[2])
                                }).ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, query);
            }
            return null;
        }

        public int GetMatriculaFromIdPre(string idPre)
        {
            var query = "";
            try
            {
                query = "SELECT NNRO_PRE FROM Prestador WHERE idpre = " + idPre;
                var c = new Connection();
                var obj = c.QueryObject(query);
                return Convert.IsDBNull(obj) ? 0 : Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", idPre, query);
                return 0;
            }
        }

        public PrestadorData GetInfoFromIdPre(string idPre)
        {
            var prestadorData = new PrestadorData();
            var query = "";
            try
            {
                query = "SELECT IDPRE, NNRO_PRE, CAPE_NOM FROM Prestador WHERE idpre = " + idPre;
                var c = new Connection();
                var dt = c.Query(query);

                List<PrestadorData> list = (from DataRow dr in dt.Rows
                                            select new PrestadorData
                                            {
                                                Id = Convert.ToInt32(dr.ItemArray[0]),
                                                Matricula = dr.ItemArray[1].ToString().Trim(),
                                                Name = dr.ItemArray[2].ToString().Trim()
                                            }).ToList();
                prestadorData = list[0];
                return prestadorData;
            }
            catch (Exception ex)
            {
                var errors = new Errores();
                errors.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", idPre, query);
                return prestadorData;
            }
        }
    }
}
