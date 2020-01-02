using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Repository.OS
{
    public class OSAcaSalud : ObrasSociales
    {
        string urlBase;

        public OSAcaSalud()
        {
            urlBase = "http://caws.acasalud.com.ar:8000/cawsProd/Servicios?invoke=transaccionStr&pSolicitud=";
        }

        public Afiliado Elegibilidad(string credencial, int matricula)
        {
           // credencial = "128990/27";
            var afiliado = new Afiliado();
            try
            {
                string resultado;
                var xAfiplan = "";
                var xAfiliado = "";
                
                //armado del xml de solicitud
                var output = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(output))
                {
                    writer.WriteStartElement("SOLICITUD");

                    writer.WriteStartElement("EMISOR");
                    writer.WriteElementString("ID", "00001-22222");
                    writer.WriteElementString("PROT", "CA_V20");
                    writer.WriteElementString("MSGID", DateTime.Now.ToString("yyyyMMdd") + matricula);//completar
                    writer.WriteElementString("TER", "");
                    writer.WriteElementString("APP", "HMS_CAWEB");
                    writer.WriteElementString("TIME", DateTime.Now.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("SEGURIDAD");
                    writer.WriteElementString("TIPOAUT", "U");
                    writer.WriteElementString("USRID", "7040521");
                    writer.WriteElementString("USRPASS", "DAT_MGR");
                    writer.WriteEndElement();

                    writer.WriteStartElement("OPER");
                    writer.WriteElementString("TIPO", "ELG");
                    writer.WriteElementString("FECHA", DateTime.Now.ToString("yyyy-MM-dd"));
                    writer.WriteElementString("IDASEG", "ACA_SALUD");
                    writer.WriteElementString("IDPRESTADOR", "7040521");
                    writer.WriteEndElement();

                    writer.WriteStartElement("PID");
                    writer.WriteElementString("ID", credencial);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
              
                }
                using (var client = new HttpClient())
                {
                    var url = urlBase + HttpUtility.UrlEncode(output.ToString());
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    
                    Task<HttpResponseMessage> response = client.GetAsync(url);
                    resultado = response.Result.Content.ReadAsStringAsync().Result;
                    resultado = HttpUtility.HtmlDecode(resultado);
                }

                logResult(output.ToString(), resultado, "E");

                if (resultado == "")
                {
                    afiliado.Name = Mensajes.Get("AfiIne"); 
                }
                else
                {
                    using (var reader = XmlReader.Create(new StringReader(resultado)))
                    {
                        reader.MoveToContent();
                        while (reader.Read())
                        {
                            if (reader.NodeType != XmlNodeType.Element) continue;
                            switch (reader.Name)
                            {
                                case "AFIPLAN":
                                    xAfiplan = reader.ReadElementContentAsString();
                                    break;
                                case "AFIAPE":
                                    xAfiliado = reader.ReadElementContentAsString();
                                    break;
                                case "AFINOM":
                                    xAfiliado += ", " + reader.ReadElementContentAsString();
                                    break;
                            }
                        }
                    }
                afiliado.Name = xAfiliado != "" ? xAfiliado : Mensajes.Get("AfiIne");  // "Afiliado inexistente o inhabilitado";
                    afiliado.Plan = xAfiplan;
                }

            }
            catch (Exception ex)
            {
                afiliado.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", credencial + ";" + matricula, string.Empty);
            }
            return afiliado;
        }

        public async Task<AutorizacionOs> Autorizar(Authorize model)
        {
            var autorizacionOs = new AutorizacionOs();
            string resultado;
            try
            {
                var output = new StringBuilder();
                using (var writer = XmlWriter.Create(output))
                {
                    writer.WriteStartElement("SOLICITUD");

                    writer.WriteStartElement("EMISOR");
                    writer.WriteElementString("ID", "00001-22222");
                    writer.WriteElementString("PROT", "CA_V20");
                    writer.WriteElementString("MSGID", DateTime.Now.ToString("yyyyMMdd") + model.Efector.Matricula);//completar
                    writer.WriteElementString("TER", "");
                    writer.WriteElementString("APP", "HMS_CAWEB");
                    writer.WriteElementString("TIME", DateTime.Now.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("SEGURIDAD");
                    writer.WriteElementString("TIPOAUT", "U");
                    writer.WriteElementString("USRID", "7040521");
                    writer.WriteElementString("USRPASS", "DAT_MGR");
                    writer.WriteEndElement();

                    writer.WriteStartElement("OPER");
                    writer.WriteElementString("TIPO", "AP");
                    writer.WriteElementString("FECHA", DateTime.Now.ToString("yyyy-MM-dd"));
                    writer.WriteElementString("IDASEG", "ACA_SALUD");
                    writer.WriteElementString("IDPRESTADOR", "7040521");
                    writer.WriteEndElement();

                    writer.WriteStartElement("PID");
                    if (model.Tipo == "B") { writer.WriteElementString("TIPOID", "CODIGO"); }
                    writer.WriteElementString("ID", model.Credencial);
                    if (model.Tipo == "B") { writer.WriteElementString("VERIFID", "AUTO"); }
                    writer.WriteEndElement();

                    writer.WriteStartElement("CONTEXTO");
                    writer.WriteElementString("TIPO", "A");
                    writer.WriteEndElement();

                    writer.WriteStartElement("PRESCRIP");
                    writer.WriteElementString("ORG", "MP A");
                    writer.WriteElementString("MAT", model.Efector.Matricula.ToString());
                    writer.WriteElementString("FECHA", DateTime.Now.ToString("yyyy-MM-dd"));
                    writer.WriteEndElement();

                    foreach (var objPrestacion in model.Prestaciones)
                    {
                        writer.WriteStartElement("PR");
                        writer.WriteElementString("TIPO", "P");
                        writer.WriteElementString("ID", objPrestacion.CodPres);
                        writer.WriteElementString("CANT", objPrestacion.Cant.ToString());

                        writer.WriteStartElement("EFECTOR");
                        writer.WriteElementString("ORG", "MP A");
                        writer.WriteElementString("MAT", model.Efector.Matricula.ToString());
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                //using (var client = new HttpClient())
                //{
                //    var url = urlBase + HttpUtility.UrlEncode(output.ToString());
                //    client.BaseAddress = new Uri(url);
                //    client.DefaultRequestHeaders.Accept.Clear();

                //    Task<HttpResponseMessage> response = client.GetAsync(url);
                //    resultado = await response.Result.Content.ReadAsStringAsync();
                //    resultado = HttpUtility.HtmlDecode(resultado);
                //}

                //logResult(output.ToString(), resultado, "A");

                //return SetAutorizacionOs(resultado, model);
                return null;
            }
            catch (Exception ex)
            {
                autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
                return autorizacionOs;
            }
        }


        private AutorizacionOs SetAutorizacionOs(string data, Authorize authorize)
        {
            var autorizacionOs = new AutorizacionOs();
            try
            {
                /******************************************************************************************************************/
                /************************************************** Default Data **************************************************/
                /******************************************************************************************************************/
                var osrepository = new OSRepository();
                var det = new AutorizacionOsDet();
                autorizacionOs = new AutorizacionOs();
                autorizacionOs.NnroAfiliado = authorize.Credencial;
                autorizacionOs.Idpre = Convert.ToInt32(authorize.PrestadorId);
                autorizacionOs.Ncodosoc = osrepository.GetOSbyId(authorize.OSId);
                autorizacionOs.Nestado = 0;
                autorizacionOs.NidUsuario = Convert.ToInt32(authorize.UserId);
                autorizacionOs.DfecEstado = DateTime.Today.ToString();
                autorizacionOs.CcodAnulacion = "";
                autorizacionOs.Idfacturador = Convert.ToInt32(authorize.FacturadorId);
                autorizacionOs.Tipo = authorize.Tipo;

                /******************************************************************************************************************/
                /********************************************** AcaSalud Return Data **********************************************/
                /******************************************************************************************************************/
                using (XmlReader reader = XmlReader.Create(new StringReader(data)))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case "IDTRAN":
                                autorizacionOs.CnroAutorizacion = reader.ReadElementContentAsString();

                                //autorizacionOs.CcodinternoAutorizacion = reader.ReadElementContentAsString();
                                break;
                            case "STATUS":
                                var sEstado = reader.ReadElementContentAsString();
                                var std = sEstado == "OK" ? "Autorizada" : "Rechazada";
                                autorizacionOs.EstadoAutorizacion = std;
                                autorizacionOs.ResultadoAutorizacion = std;
                                break;
                            case "RSPCODG":
                                //autorizacionOs.Error.Estado = reader.ReadElementContentAsString();
                                break;
                            case "RSPMSGG":
                                autorizacionOs.Error.Mensaje += reader.ReadElementContentAsString();
                                break;
                            case "RSPMSGGADIC":
                                autorizacionOs.Error.Mensaje += reader.ReadElementContentAsString();
                                break;
                            case "AFIPLAN":
                                break;
                            case "AFIPLANADIC":
                                autorizacionOs.CdescripcionPlan = reader.ReadElementContentAsString();
                                break;
                            case "AFIAPE": //Apellido del Afiliado
                                autorizacionOs.CnomAfiliado = reader.ReadElementContentAsString();
                                break;
                            case "AFINOM": //Nombre del Afiliado
                                autorizacionOs.CnomAfiliado =
                                    autorizacionOs.CnomAfiliado + ", " + reader.ReadElementContentAsString();
                                break;
                            case "AFISEXO":
                                break;
                            case "AFINAC":
                                break;
                            case "AFIAFIL":
                                autorizacionOs.CdescripcionIva = reader.ReadElementContentAsString();
                                break;
                            case "IDAUT":
                                var cod = reader.ReadElementContentAsString();
                                autorizacionOs.CcodinternoAutorizacion = cod;
                                //autorizacionOs.CnroAutorizacion = cod;
                                break;
                            case "FECHAOPER":
                                autorizacionOs.DfecAutorizacion = reader.ReadElementContentAsString();
                                break;
                            case "PR":
                                /******************************************************************************************************************/
                                /********************************************* Detalle de Autorizacion ********************************************/
                                /******************************************************************************************************************/
                                var bElementoIniciado = false;
                                while (reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        switch (reader.Name)
                                        {
                                            case "TIPO":
                                                bElementoIniciado = true;
                                                reader.ReadElementContentAsString();
                                                break;
                                            case "ID":
                                                det.PracticaId = reader.ReadElementContentAsString();
                                                break;
                                            case "DESCRIPCION":
                                                det.PracticaDes = reader.ReadElementContentAsString();
                                                break;
                                            case "IDTORD":
                                                var sIdTord = reader.ReadElementContentAsString();
                                                break;
                                            case "DESCTORD":
                                                var sDescOrd = reader.ReadElementContentAsString();
                                                break;
                                            case "STATUS":
                                                det.PracticaIdEstado =
                                                    reader.ReadElementContentAsString().Trim() == "OK"
                                                        ? "Autorizada"
                                                        : "Rechazada";
                                                break;
                                            case "RSPCODP":
                                                var sRspcodp = reader.ReadElementContentAsString();
                                                break;
                                            case "RSPMSGP":
                                                det.PracticaDetAuth = reader.ReadElementContentAsString();
                                                break;
                                            case "REALIZNOM":
                                                var sRealizNom = reader.ReadElementContentAsString();
                                                break;
                                            case "CANT":
                                                det.PracticaCantAprob = reader.ReadElementContentAsString();
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (reader.NodeType != XmlNodeType.EndElement || !bElementoIniciado) continue;
                                        if (det.PracticaId == null) continue;
                                        det.PracticaAuthNr = autorizacionOs.CcodinternoAutorizacion;
                                        autorizacionOs.AutorizacionOsDet.Add(det);
                                        det = new AutorizacionOsDet();
                                    }
                                }
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", authorize, string.Empty);
            }
            return autorizacionOs;
        }

        public async Task<AnulacionOS> AnularAsync(Anular model)
        {
            var anulacionOs = new AnulacionOS();
            string resultado;
            try
            {
                var authRepo = new AutorizacionRepository();
                var sIdInternoAut = authRepo.GetIdAuth(model.AuthId);

                var output = new StringBuilder();
                using (var writer = XmlWriter.Create(output))
                {
                    writer.WriteStartElement("SOLICITUD");

                    writer.WriteStartElement("EMISOR");
                    writer.WriteElementString("ID", "00001-22222");
                    writer.WriteElementString("PROT", "CA_V20");
                    writer.WriteElementString("MSGID", DateTime.Now.ToString("yyyyMMdd") + model.Matricula);//completar
                    writer.WriteElementString("TER", "");
                    writer.WriteElementString("APP", "HMS_CAWEB");
                    writer.WriteElementString("TIME", DateTime.Now.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("SEGURIDAD");
                    writer.WriteElementString("TIPOAUT", "U");
                    writer.WriteElementString("USRID", "7040521");
                    writer.WriteElementString("USRPASS", "DAT_MGR");
                    writer.WriteEndElement();

                    writer.WriteStartElement("OPER");
                    writer.WriteElementString("TIPO", "ATR");
                    writer.WriteElementString("FECHA", DateTime.Now.ToString("yyyy-MM-dd"));
                    writer.WriteElementString("IDASEG", "ACA_SALUD");
                    writer.WriteElementString("IDPRESTADOR", "7040521");
                    writer.WriteElementString("TIPOIDANUL", "IDTRAN");
                    writer.WriteElementString("IDANUL", sIdInternoAut);
                    writer.WriteEndElement();

                    writer.WriteStartElement("PID");
                    writer.WriteElementString("TIPOID", "CODIGO_AFI");
                    writer.WriteElementString("ID", model.Credencial);
                    writer.WriteEndElement();
                }

                using (var client = new HttpClient())
                {
                    var url = urlBase + HttpUtility.UrlEncode(output.ToString());
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();

                    Task<HttpResponseMessage> response = client.GetAsync(url);
                    resultado = await response.Result.Content.ReadAsStringAsync();
                    resultado = HttpUtility.HtmlDecode(resultado);
                }

                logResult(output.ToString(), resultado, "D");

                if (resultado != "") return SetAnulacionOs(resultado, model);

                anulacionOs.SetError(GetType().Name, 0, Mensajes.Get("AnulFail"), string.Empty, model, string.Empty);
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, model, string.Empty);
            }

            return anulacionOs;
        }

        private AnulacionOS SetAnulacionOs(string data, Anular model)
        {
            var anulacionOs = new AnulacionOS();
            try
            {
                anulacionOs = new AnulacionOS
                {
                    IdAuth = model.AuthId.ToString(),
                    Nestado = "1",
                    Fecha = DateTime.Now.ToString(),
                };

                var sEstado = "";

                using (var reader = XmlReader.Create(new StringReader(data)))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case "IDTRAN":
                                anulacionOs.CodAnulacion = reader.ReadElementContentAsString();
                                break;
                            case "STATUS":
                                sEstado = reader.ReadElementContentAsString();
                                break;
                            case "AFIPLAN":
                                break;
                            case "AFIAPE":
                                // sAfiliadoNom = reader.ReadElementContentAsString();
                                break;
                            case "AFINOM":
                                //sAfiliadoNom = sAfiliadoNom + ", " + reader.ReadElementContentAsString();
                                break;
                            case "IDAUT":
                                //sNroAutorizacion = reader.ReadElementContentAsString();
                                break;
                        }
                    }
                }

                anulacionOs.Estado = sEstado; //OK - Anular; Cualquier otra cosa; No hacer Nada
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return anulacionOs;
        }
    }
}
