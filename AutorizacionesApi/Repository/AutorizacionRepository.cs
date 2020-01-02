using System;
using System.Data;
using WebApi.Models;
using WebApi.Models.Autorizacion;
using WebApi.Models.Prestacion;
using WebApi.Utils;
using WebApi.Repository.OS;

namespace WebApi.Repository
{
    public class AutorizacionRepository
    {
        public AutorizacionVer Autorizacion(Authorize model)
        {
            var autorizacionOs = new AutorizacionOs();
            var autorizacionVer = new AutorizacionVer();
            try
            {
                model.Efector = DatosPrestador(Convert.ToInt32(model.PrestadorId));
                switch (model.OSId)
                {
                    case 0:
                        break;
                    case 1://Swiss Medical
                        var swiss = new OSSwiss();
                        autorizacionOs = swiss.Autorizar(model);
                        break;
                    case 2:
                        var acaSalud = new OSAcaSalud();
                        autorizacionOs = acaSalud.Autorizar(model).Result;
                        break;
                    case 5:
                    case 8:
                        var boreal = new OSBoreal();
                        autorizacionOs = boreal.Autorizar(model);
                        break;
                    case 6:
                        var medife = new OSMedife();
                        autorizacionOs = medife.Autorizar(model);
                        break;
                    case 7:
                        var redSeguros = new OSRedSeguros();
                        autorizacionOs = redSeguros.Autorizar(model);
                        break;
                    case 9:
                        var sancor = new OSSancor();
                        autorizacionOs = sancor.Autorizar(model);
                        break;
                    case 10:
                        //Luz y Fuerza
                        break;
                    case 11:
                        var os = new OSPatrones();
                        autorizacionOs = os.Autorizar(model);
                        break;
                }
                if (!autorizacionOs.HasError)
                {
                    var authNr = Autorizar(autorizacionOs);
                    //Todo Guardar en Base de Datos
                    // return cod Autorizacion;  
                    //Al menos retornar codigo de Autorizacion  
                    autorizacionVer.AuthNr = autorizacionOs.CnroAutorizacion;// .CcodinternoAutorizacion;
                    autorizacionVer.Fecha = autorizacionOs.DfecAutorizacion;
                    autorizacionVer.Afiliado = autorizacionOs.CnomAfiliado;
                    autorizacionVer.Plan = autorizacionOs.CdescripcionPlan;
                    autorizacionVer.Iva = autorizacionOs.CdescripcionIva;
                    autorizacionVer.IdentificacionNro = autorizacionOs.NnroAfiliado;
                    autorizacionVer.Aseguradora = model.OsNombre;
                    autorizacionVer.Matricula = model.Efector.Matricula.ToString();
                    autorizacionVer.Profesional = model.Efector.Name;
                    autorizacionVer.Estado = autorizacionOs.EstadoAutorizacion;
                    autorizacionVer.Id = authNr;
                    foreach (var det in autorizacionOs.AutorizacionOsDet)
                    {
                        var authVerDet = new AutorizacionVerDet
                        {
                            Prestacion = "(" + det.PracticaId + ") - " + det.PracticaDes,
                            Cantidad = (string.IsNullOrEmpty(det.PracticaCantAprob) ? "1" : det.PracticaCantAprob),
                            Estado = det.PracticaIdEstado,
                            Observacion = det.PracticaDetAuth
                        };
                        autorizacionVer.Detalle.Add(authVerDet);
                    }
                    ///Todo:Enviar Mail con info de Autoizacion
                    //SendMail.SendMailAutirizacion(autorizacionVer);
                }
                else
                {
                    autorizacionVer.HasError = autorizacionOs.HasError;
                    autorizacionVer.Error = autorizacionOs.Error;
                }
            }
            catch (Exception ex)
            {
                autorizacionVer.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, model, string.Empty);
            }
            return autorizacionVer;
        }

        public AutorizacionVer Anular(int authId)
        {
            var anulacionOs = new AnulacionOS();
            var autorizacionVer = new AutorizacionVer();
            try
            {
                var auth = new AutorizacionRepository();
                Anular model = auth.GetAuthDataById(authId);

                switch (model.OSId)
                {
                    case 114://Swiss Medical
                        var swiss = new OSSwiss();
                        anulacionOs = swiss.Anular(model);
                        break;
                    case 132:
                        var acaSalud = new OSAcaSalud();
                        anulacionOs = acaSalud.AnularAsync(model).Result;
                        break;
                    case 137:
                    case 192:
                        var boreal = new OSBoreal();
                        anulacionOs = boreal.Anular(model);
                        break;
                    case 140:
                        var medife = new OSMedife();
                        anulacionOs = medife.Anular(model);
                        break;
                    case 181:
                        var redSeguros = new OSRedSeguros();
                        anulacionOs = redSeguros.Anular(model);
                        break;
                    case 148:
                        var sancor = new OSSancor();
                        anulacionOs = sancor.Anular(model);
                        break;
                    case 141: //Hay que cambiar por la correspondiente
                        var os = new OSPatrones();
                        anulacionOs = os.Anular(model);
                        break;
                }
                if (!anulacionOs.HasError)
                {
                    if (Anular(anulacionOs))
                    {
                        return GetAuthBy(authId);
                    }
                    //todo mapear objeto
                    //return Al menos retornar codigo de anulacion  
                }
                else
                {
                    autorizacionVer.HasError = anulacionOs.HasError;
                    autorizacionVer.Error = anulacionOs.Error;
                }
            }
            catch (Exception ex)
            {
                autorizacionVer.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", authId, string.Empty);
            }
            return autorizacionVer;
        }



        public Anular GetAuthDataById(int authId)
        {
            var anular = new Anular();
            var query = "";
            try
            {
                query = "SELECT  A.NCODOSOC, A.NID_AUTORIZACION, P.NNro_Pre, A.NNRO_AFILIADO, P.CNRO_DGI";
                query += " FROM AUTORIZACIONES A";
                query += " INNER JOIN Prestador P ON P.Idpre = A.Idpre";
                query += " WHERE NID_AUTORIZACION =" + authId;

                var c = new Connection();
                var dt = c.Query(query);
                
                foreach (DataRow dr in dt.Rows)
                {
                    anular = new Anular
                    {
                        OSId = Convert.ToInt32(dr.ItemArray[0]),
                        AuthId = Convert.ToInt32(dr.ItemArray[1].ToString().Trim()),
                        Matricula = dr.ItemArray[2].ToString(),
                        Credencial = dr.ItemArray[3].ToString(),
                        Cuit = dr.ItemArray[4].ToString()
                    };
                }

                return anular;
            }
            catch (Exception ex)
            {
                anular.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, authId, query);
            }
            return anular;
        }

        internal PdfAuth AuthPdf(int id)
        {

            /////
            ///// https://selectpdf.com/html-to-pdf/demo/convert-html-code-to-pdf.aspx
            /////

            //PdfAuth model = new PdfAuth();
            //try
            //{
            //    var auth = GetAuthBy(id);

            //    var estado = "";
            //    switch (auth.Estado)
            //    {
            //        case "Autorizada":
            //            estado = "green";
            //            break;
            //        case "Rechazada":
            //            estado = "red";
            //            break;
            //        case "Autorizada Parcial":
            //            estado = "orange";
            //            break;
            //        default:
            //            estado = "black";
            //            break;
            //    }
            //    var logo = HttpRuntime.AppDomainAppPath + @"/images/logo.png";
            //    var html = "<div>";
            //    html += "   <table> ";
            //    html += "       <tr> ";
            //    html += "           <td style='width:200px'>";
            //    html += "               <img src='" + logo + "' height='75px' width='75px'>";
            //    html += "           </td>";
            //    html += "           <td style='width:800px:text-align: center;'> ";
            //    html += "               <h3 style ='font-size: 34px; line-height: 1.11111111; letter-spacing: -1px; font-family: Raleway,sans-serif;font-weight: 900;margin: 0;text-transform: uppercase;color: #039;text-align: center;'> Circulo Medico Salta</h3>";
            //    html += "               <h6 style='font-size: 20px;line-height: 1.25;color: #21c2f8;font-weight: 700;text-transform: none;margin: 0;text-align: center;'> Asoc.civil sin fines de Lucro</h6>";
            //    html += "           </td>";
            //    html += "       </tr>";
            //    html += "   </table>";
            //    html += "   <div>";
            //    html += "       <div style = 'padding-bottom: 10px;text-align: center;height:35px; font-size: 20px'>";
            //    html += "           <h7 style = 'padding-bottom: 10px;text-align: center;height:35px' > Comprobante de Autorización ON - LINE </h7>";
            //    html += "       </div>";
            //    html += "   </div>";
            //    html += "   <div>";
            //    html += "       <table>";
            //    html += "           <tr>";
            //    html += "               <td style='width:110px;font-weight: bold;'>Autorización Nº: </td>";
            //    html += "               <td style='width:150px;'>" + auth.AuthNr + "</td>";
            //    html += "               <td style='width:35px;font-weight: bold;'>Fecha: </td>";
            //    html += "               <td style='width:125px'>" + auth.Fecha + "</td>";
            //    html += "               <td style='width:40px;font-weight: bold;'>Estado: </td>";
            //    html += "               <td style='width:110px;font-size:1.5em;color:" + estado + "'>" + auth.Estado + "</td>";
            //    html += "           </tr>";
            //    html += "       </table>";
            //    html += "   </div>";
            //    html += "   <div>";
            //    html += "       <table>";
            //    html += "           <tr>";
            //    html += "               <td style='width:47px;font-weight: bold;'>Afiliado: </td>";
            //    html += "               <td style='width:300px;'>" + auth.Afiliado + "</td>";
            //    html += "               <td style='width:40px;font-weight: bold;'>Plan: </td>";
            //    html += "               <td style='width:150px;'>" + auth.Plan + "</td>";
            //    html += "               <td style='width:60px;font-weight: bold;'>Cond.IVA: </td>";
            //    html += "               <td style='width:150px;'>" + auth.Iva + "</td>";
            //    html += "           </tr>";
            //    html += "       </table>";
            //    html += "   </div>";
            //    html += "   <div>";
            //    html += "       <table>";
            //    html += "           <tr>";
            //    html += "               <td style='width:120px;font-weight: bold;'>Nº Identificacion: </td>";
            //    html += "               <td style='width:150px;'>" + auth.IdentificacionNro.Trim() + "</td>";
            //    html += "               <td style='width:90px;font-weight: bold;'>Aseguradora: </td>";
            //    html += "               <td style='width:200px;'>" + auth.Aseguradora + "</td>";
            //    html += "           </tr>";
            //    html += "       </table>";
            //    html += "   </div>";
            //    html += "   <div>";
            //    html += "       <table>";
            //    html += "           <tr>";
            //    html += "               <td style='width:50px;font-weight: bold;'>Profesional: </td>";
            //    html += "               <td style='width:450px;'>" + auth.Profesional + "</td>";
            //    html += "               <td style='width:53px;font-weight: bold;'>Matricula: </td>";
            //    html += "               <td style='width:150px;'> " + auth.Matricula + "</td>";
            //    html += "           </tr>";
            //    html += "       </table>";
            //    html += "   </div>";
            //    html += "   <div>";
            //    html += "       <div style = 'padding-top: 15px; padding-bottom: 10px; text-align:center; height:35px; font-size: 18px'>";
            //    html += "           <h7 style = 'padding-bottom: 10px;text-align: center;height:35px' > Presentaciones </h7>";
            //    html += "       </div>";
            //    html += "   </div>";
            //    html += "   <div>";
            //    html += "       <table>";
            //    html += "           <tr>";
            //    html += "               <td style='width:200px;font-weight: bold;'>Prestación</td>";
            //    html += "               <td style='width:25px;font-weight: bold;'>Cant.</td>";
            //    html += "               <td style='width:100px;font-weight: bold;'>Estado</td>";
            //    html += "               <td style='width:230px;font-weight: bold;'>Observación</td>";
            //    html += "           </tr>";
            //    foreach (var item in auth.Detalle)
            //    {
            //        html += "           <tr>";
            //        html += "               <td>" + item.Prestacion + "</td>";
            //        html += "               <td style='text-align:center;'>" + item.Cantidad + "</td>";
            //        html += "               <td>" + item.Estado + "</td>";
            //        html += "               <td>" + item.Observacion + "</td>";
            //        html += "           </tr>";
            //    }
            //    html += "       </table>";
            //    html += "   </div>";
            //    html += "</div>";

            //    string ServerPath = System.Web.HttpRuntime.AppDomainAppPath + @"/PDF/";
            //    string fileName = auth.AuthNr + ".pdf";
            //    var OutputPath = ServerPath + fileName;

            //    // read parameters from the webpage
            //    string htmlString = html;
            //    string baseUrl = "";

            //    string pdf_page_size = "A4";
            //    PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

            //    string pdf_orientation = "Portrait"; //"Landscape"; //"Portrait";
            //    PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

            //    int webPageWidth = 800;

            //    int webPageHeight = 0;

            //    HtmlToPdf converter = new HtmlToPdf();

            //    // set converter options
            //    converter.Options.PdfPageSize = pageSize;
            //    converter.Options.PdfPageOrientation = pdfOrientation;
            //    converter.Options.WebPageWidth = webPageWidth;
            //    converter.Options.WebPageHeight = webPageHeight;

            //    // create a new pdf document converting an url
            //    PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);

            //    // save pdf document
            //    doc.Save(OutputPath);

            //    // close pdf document
            //    doc.Close();

            //    model.url = fileName;
            //    return model;

            //}
            //catch (Exception ex)
            //{
            //    model.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, "");
            //    return model;
            //}
            return null;
        }

        public int ObtenerCodigoAsignado(int iUsuarioId, string nroAfiliado)
        {
            var query = "";
            try
            {
                var c = new Connection();
                query = "SELECT MAX(NID_AUTORIZACION) FROM autorizaciones WHERE nid_usuario=" + iUsuarioId + "  AND NNRO_AFILIADO='" + nroAfiliado + "'";
                var result = c.QueryObject(query);
                
                return Convert.IsDBNull(result) ? 0 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                var errores = new Errores();
                errores.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, nroAfiliado, query);
            }
            return 0;
        }

        public Efector DatosPrestador(int idPre)
        {
            var efector = new Efector();
            var query = "";
            try
            {
                var c = new Connection();
                query = "SELECT rtrim(CAPE_NOM), TRIM(CNRO_DGI), NNRO_PRE, EMAIL FROM PRESTADOR WHERE IDPRE = " + idPre;
                var dt = c.Query(query);
                
                foreach (DataRow dr in dt.Rows)
                {
                    efector.Name = dr.ItemArray[0].ToString().Trim();
                    efector.Cuit = dr.ItemArray[1].ToString().Trim();
                    efector.Matricula = Convert.ToInt32(dr.ItemArray[2]);
                    efector.Email = dr.ItemArray[3].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                efector.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", idPre, query);
            }
            return efector;
        }

        public Efector DatosPrestadorByNnroPre(int nnroPre)
        {
            var efector = new Efector();
            var query = "";
            try
            {
                var c = new Connection();
                query = "SELECT rtrim(CAPE_NOM), TRIM(CNRO_DGI), NNRO_PRE, EMAIL FROM PRESTADOR WHERE NNRO_PRE  = " + nnroPre;
                var dt = c.Query(query);
                
                foreach (DataRow dr in dt.Rows)
                {
                    efector.Name = dr.ItemArray[0].ToString().Trim();
                    efector.Cuit = dr.ItemArray[1].ToString().Trim();
                    efector.Matricula = Convert.ToInt32(dr.ItemArray[2]);
                    efector.Email = dr.ItemArray[3].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                efector.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", nnroPre, query);
            }
            return efector;
        }

        public string GetIdAuth(int idAut)
        {
            var query = "";
            try
            {
                var c = new Connection();

                query = "SELECT CNRO_AUTORIZACION FROM autorizaciones WHERE NID_AUTORIZACION=" + idAut;
                var result = c.QueryObject(query);
                
                return result.ToString() == "" ? "" : (Convert.IsDBNull(result) ? "" : (result.ToString()));
            }
            catch (Exception ex)
            {
                var errores = new Errores();
                errores.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, idAut, query);
            }
            return string.Empty;
        }

        private bool Anular(AnulacionOS anulacionOs)
        {
            var query = "";
            try
            {
                query = "UPDATE Autorizaciones SET Estado_Autorizacion='Anulada', NESTADO=1, DFEC_ESTADO=SYSDATE, CCOD_ANULACION='" + anulacionOs.CodAnulacion + "', dfec_anulacion=SYSDATE";
                query += " WHERE NID_AUTORIZACION=" + anulacionOs.IdAuth;

                var c = new Connection();
                c.QueryNoResult(query);
                
                query = "UPDATE Autorizaciones_Detalle SET Estado_Autorizacion='Anulada' WHERE Estado_Autorizacion='Autorizada' AND NID_AUTORIZACION=" + anulacionOs.IdAuth;
                c.QueryNoResult(query);

                return true;
            }
            catch (Exception ex)
            {
                var errores = new Errores { HasError = true };
                errores.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, anulacionOs, query);
                return false;
            }
        }

        public int Autorizar(AutorizacionOs autorizacionOs)
        {
            var query = "";
            var queryDet = "";
            try
            {
                query = "INSERT INTO AUTORIZACIONES(NID_AUTORIZACION, CNRO_AUTORIZACION, IDPRE, NCODOSOC, NNRO_AFILIADO, CNOM_AFILIADO, " +
                                  "DFEC_AUTORIZACION, CDESCRIPCION_PLAN, CDESCRIPCION_IVA, ESTADO_AUTORIZACION, NID_USUARIO, NESTADO, DFEC_ESTADO, " +
                                  "RESULTADO_AUTORIZACION, CCODINTERNO_AUTORIZACION, CCOD_ANULACION, IDFACTURADOR, MODOtIPO ) VALUES( null, '" + autorizacionOs.CnroAutorizacion + "'," + autorizacionOs.Idpre + "," + autorizacionOs.Ncodosoc + ",'" +
                                  autorizacionOs.NnroAfiliado + "','" + autorizacionOs.CnomAfiliado + "', TO_DATE('" + autorizacionOs.DfecAutorizacion + "', 'YYYY-MM-DD'),'" + autorizacionOs.CdescripcionPlan + "','" + autorizacionOs.CdescripcionIva + "','" + autorizacionOs.EstadoAutorizacion + "'," +
                                  autorizacionOs.NidUsuario + "," + autorizacionOs.Nestado + ",SYSDATE,'" + autorizacionOs.ResultadoAutorizacion + "','" + autorizacionOs.CcodinternoAutorizacion + "',' ', " + autorizacionOs.Idfacturador + ",'" + autorizacionOs.Tipo + "')";

                var c = new Connection();
                c.QueryNoResult(query);
                
                var iAutId = ObtenerCodigoAsignado(Convert.ToInt32(autorizacionOs.NidUsuario), autorizacionOs.NnroAfiliado);

                foreach (var autorizacion in autorizacionOs.AutorizacionOsDet)
                {
                    //Todo genera elementos vacios, hay que chequear
                    //Todo gravar CNRO_AutorizacionesPres
                    queryDet = "INSERT INTO AUTORIZACIONES_DETALLE(NID_AUTORIZACION, CCODPREST, CNOMPREST, NCANTIDAD, ESTADO_AUTORIZACION, DETALLE_AUTORIZACION,CNRO_AUTORIZACIONPRES) " +
                                   "VALUES(" + iAutId + ",'" + autorizacion.PracticaId + "','" + autorizacion.PracticaDes + "'," + (string.IsNullOrEmpty(autorizacion.PracticaCantAprob) ? "1" : autorizacion.PracticaCantAprob) + ",'" + autorizacion.PracticaIdEstado + "','" + autorizacion.PracticaDetAuth + "','" + autorizacion.PracticaAuthNr + "')";
                    c.QueryNoResult(queryDet);
                }
            
                return iAutId;
            }
            catch (Exception ex)
            {
                var errores = new Errores();
                errores.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", autorizacionOs, "Query: " + query + " ,QueryDet: " + queryDet);
                return 0;
            }
        }

        public AuthListOutputList GetAuthList(AuthList model)
        {
            var query = "";
            var list = new AuthListOutputList();
            try
            {
                var os = new OSRepository();
                var osId = os.GetOSbyId(model.OsId);

                
                query += "SELECT Distinct";
                query += "                a.nid_autorizacion, ";
                query += "                CNRO_AUTORIZACION, ";
                query += "                DFEC_AUTORIZACION, ";
                query += "                NNRO_PRE, ";
                query += "                CNOM_AFILIADO, ";
                query += "                NNRO_AFILIADO, ";
                query += "               (select LISTAGG(ccodprest, ',') WITHIN GROUP(ORDER BY ccodprest ) from autorizaciones_detalle AD1 where AD1.nid_autorizacion = ad.nid_autorizacion) AS Prestacion,";
                query += "               (select count(*) from autorizaciones_detalle AD1 where AD1.nid_autorizacion = ad.nid_autorizacion) as NCantidad, ";
                query += "               A.ESTADO_AUTORIZACION, ";
                query += "               NVL(A.CCOD_ANULACION, '') as CCOD_ANULACION, ";
                query += "               NVL(PRESENTACIONID, '') as PRESENTACIONID, ";
                query += "               NVL(A.MODOTIPO,'M') as MODOTIPO ";
                query += " FROM AUTORIZACIONES A ";
                query += " inner join autorizaciones_detalle AD on a.nid_autorizacion = ad.nid_autorizacion ";
                query += " inner join prestador P on A.IDPRE = P.IDPRE ";
                query += " WHERE A.IDPRE = " + model.PrestadorId + " AND A.NCODOSOC = " + osId + " AND DFEC_AUTORIZACION >= TO_DATE('" + model.Desde + "', 'DD/MM/YYYY') AND DFEC_AUTORIZACION <= TO_DATE('" + model.Hasta + "', 'DD/MM/YYYY HH24:MI') ";
                query += " Order by DFEC_AUTORIZACION DESC, a.nid_autorizacion Desc ";

                var c = new Connection();
                var dt = c.Query(query);
                
                foreach (DataRow dr in dt.Rows)
                {
                    var strFech = dr.ItemArray[2].ToString();
                    var index = strFech.IndexOf(' ');
                    var banda = dr.ItemArray[11].ToString().Trim() == "B" ? " (B)" : "";
                    var auth = new AuthListOutput
                    {
                        Id = Convert.ToInt32(dr.ItemArray[0]),
                        AuthNr = (dr.ItemArray[9].ToString().Trim() != "" ? dr.ItemArray[9].ToString() : dr.ItemArray[1].ToString()) + banda, // dr.ItemArray[9]. = Cod Anulacion;  dr.ItemArray[1]= Cod Autorizacion
                        Fecha = (index > 0 ? strFech.Substring(0, index).Trim() : strFech.Trim()),
                        Matricula = dr.ItemArray[3].ToString(),
                        Afiliado = dr.ItemArray[4].ToString(),
                        AfiNr = dr.ItemArray[5].ToString(),
                        Prestacion = dr.ItemArray[6].ToString(),
                        Cant = dr.ItemArray[7].ToString(),
                        Estado = dr.ItemArray[8].ToString(),
                        Presentado = dr.ItemArray[8].ToString().Trim() != ""
                    };
                    list.List.Add(auth);
                }
            }
            catch (Exception ex)
            {
                list.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, query);
            }
            return list;
        }

        public AutorizacionVer GetAuthBy(int id)
        {
            //var autorizacionVer = new AutorizacionVer();
            //var query = "";
            //try
            //{
            //    var c = new Connection();
            //    var conn = c.Connect();

            //    query = "SELECT NVL(ESTADO_AUTORIZACION,' ') as ESTADO_AUTORIZACION, NVL(RESULTADO_AUTORIZACION,' ') as RESULTADO_AUTORIZACION, NVL(CNRO_AUTORIZACION,' ') as CNRO_AUTORIZACION, DFEC_AUTORIZACION, NVL(CNOM_AFILIADO,' ') as CNOM_AFILIADO, " + "NVL(CDESCRIPCION_PLAN,' ') as CDESCRIPCION_PLAN, NVL(CDESCRIPCION_IVA,' ') as CDESCRIPCION_IVA, NVL(NNRO_AFILIADO,'') as NNRO_AFILIADO, NVL(CLEYENDA,'') as CLEYENDA, NVL(NNRO_PRE,'')as NNRO_PRE, NVL(CAPE_NOM,'') as CAPE_NOM, NVL(A.CCOD_ANULACION,'') as CCOD_ANULACION  FROM AUTORIZACIONES A, OBRASOCIAL_WEBSERVICES OSWS, " + "PRESTADOR P WHERE A.NCODOSOC=OSWS.NCODOSOC AND OSWS.NESTADO=0 AND A.IDPRE=P.IDPRE AND NID_AUTORIZACION=" + id + " OR ccod_anulacion = '" + id + "'";
            //    var cmd = new OracleCommand(query, conn) { CommandType = CommandType.Text };
            //    conn.Open();


            //    var lResultado = cmd.ExecuteReader();
            //    if (lResultado.Read())//hay informacion con ese codigo, es lo esperable
            //    {
            //        autorizacionVer.AuthNr = lResultado.GetString(11).Trim() != "" ? lResultado.GetString(11).Trim() : lResultado.GetString(2).Trim();
            //        autorizacionVer.Fecha = lResultado.GetDateTime(3).ToShortDateString();
            //        autorizacionVer.Afiliado = lResultado.GetString(4).Trim();
            //        autorizacionVer.Plan = lResultado.GetString(5).Trim();
            //        autorizacionVer.Iva = lResultado.GetString(6).Trim();
            //        autorizacionVer.IdentificacionNro = lResultado.GetString(7).Trim();
            //        autorizacionVer.Aseguradora = lResultado.GetString(8).Trim();
            //        autorizacionVer.Matricula = lResultado.GetInt32(9).ToString().Trim();
            //        autorizacionVer.Profesional = lResultado.GetString(10).Trim();
            //        autorizacionVer.Estado = lResultado.GetString(0).Trim();
            //    }

            //    query = "SELECT CCODPREST || ' - ' || CNOMPREST AS prestacion, NCANTIDAD AS cantidad, ESTADO_AUTORIZACION AS estado, DETALLE_AUTORIZACION AS observaciones FROM AUTORIZACIONES_DETALLE " + "WHERE NID_AUTORIZACION=" + id;
            //    var cmdDet = new OracleCommand(query, conn) { CommandType = CommandType.Text };
            //    var dt = new DataTable();
            //    var da = new OracleDataAdapter(cmdDet);
            //    da.Fill(dt);

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        var det = new AutorizacionVerDet()
            //        {
            //            Prestacion = dr.ItemArray[0].ToString(),
            //            Cantidad = dr.ItemArray[1].ToString(),
            //            Estado = dr.ItemArray[2].ToString(),
            //            Observacion = dr.ItemArray[3].ToString()
            //        };

            //        autorizacionVer.Detalle.Add(det);
            //    }

            //    conn.Dispose();
            //    conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    autorizacionVer.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", id, query);
            //}
            //return autorizacionVer;
            return null;
        }

        public int GetAuthCount(PresentacionNew model)
        {
            var query = "";
            try
            {
                query = "SELECT COUNT(*) FROM Autorizaciones WHERE estado_autorizacion = 'Autorizada'";
                query += " AND ncodosoc = " + model.CmsOsId;
                query += " AND IDPre = " + model.Matricula;
                query += " AND dfec_autorizacion > TO_DATE('" + model.Desde + "', 'DD/MM/YYYY')";
                query += " AND dfec_autorizacion <= TO_DATE('" + model.Hasta + " 23:58', 'DD/MM/YYYY HH24:MI')";
                query += " AND presentacionId IS NULL";

                var c = new Connection();
                var obj = c.QueryObject(query);
                
                return Convert.IsDBNull(obj) ? 0 : Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                var errores = new Errores();
                errores.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, query);
                return 0;
            }
        }

        public PresentacionSaved SetPresentacion(PresentacionNew model, PresentacionSaved modelSaved)
        {
            var query = "";
            try
            {
                var c = new Connection();

                query = "UPDATE Autorizaciones ";
                query += " SET PresentacionId = " + modelSaved.Id;
                query += " WHERE estado_autorizacion = 'Autorizada'";
                query += " AND ncodosoc = " + model.CmsOsId;
                query += " AND IDPre = " + model.Matricula;
                query += " AND dfec_autorizacion > TO_DATE('" + model.Desde + "', 'DD / MM / YYYY')";
                query += " AND dfec_autorizacion <= TO_DATE('" + model.Hasta + "', 'DD / MM / YYYY HH24: MI')";
                query += " AND presentacionId IS NULL";

                c.QueryNoResult(query);
            }
            catch (Exception ex)
            {
                modelSaved.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, query);
            }
            return modelSaved;
        }

        internal AutorizacionVer EnviarMail(int id)
        {
            try
            {
                var auth = GetAuthBy(id);
                var efector = DatosPrestadorByNnroPre(Convert.ToInt32(auth.Matricula));
                SendMail.SendMailAutirizacion(auth, efector.Email);
                return auth;

            }
            catch (Exception ex)
            {
                var errores = new Errores { HasError = true };
                errores.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, id, "");
                return new AutorizacionVer();
            }
        }
    }
}
