using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using WebApi.Models;
using WebApi.Models.Autorizacion;
using WebApi.Utils;
using wsBoreal;

namespace WebApi.Repository.OS
{
    public class OSBoreal : ObrasSociales
    {
        readonly WsBorealSoapPortClient _objBoreal = new WsBorealSoapPortClient();
        private readonly string _clave;

        public OSBoreal()
        {
            _clave = Clave_Boreal();
        }

        public Afiliado Eligibilidad(string crecencial)
        {
            try
            {
                crecencial = "28133584/1";
                var posicion = crecencial.IndexOf("/");
                if (posicion < 0)
                {
                    return new Afiliado { Name = string.Empty, Plan = string.Empty, HasError = true, Error = new Error { Mensaje = "Credencial con formato incorrecto" } };
                }

                //armado del xml de solicitud
                var output = new StringBuilder();
                using (var writer = XmlWriter.Create(output))
                {
                    writer.WriteStartElement("BOREAL");
                    writer.WriteStartElement("Mensaje");
                    writer.WriteElementString("Canal", "ID");
                    writer.WriteElementString("SitioEmisor", "CMSws");
                    writer.WriteElementString("Empresa", "BOREAL");
                    writer.WriteStartElement("Receptor");
                    writer.WriteElementString("Nombre", "BOREAL");
                    writer.WriteElementString("ID", "222023");
                    writer.WriteElementString("Tipo", "IIN");
                    writer.WriteEndElement();
                    writer.WriteStartElement("MsgTipo");
                    writer.WriteElementString("Tipo", "ZQI");
                    writer.WriteElementString("Evento", "Z01");
                    writer.WriteElementString("Estructura", "ZQI_Z01");
                    writer.WriteEndElement();
                    writer.WriteElementString("MsgEntorno", "P");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Seguridad");
                    writer.WriteElementString("Usuario", "cmsws");
                    writer.WriteElementString("Clave", _clave);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Prestador");
                    writer.WriteElementString("PrestadorId", "30543364610");
                    writer.WriteElementString("PrestadorNombre", "");
                    writer.WriteElementString("PrestadorTipoIdent", "CU");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Afiliado");
                    writer.WriteElementString("AfiliadoNroCredencial", crecencial.Substring(0, posicion));
                    writer.WriteElementString("AfiliadoGf", crecencial.Substring(posicion + 1));
                    writer.WriteElementString("TipoIdentificador", "HC");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                var mensajexml = output.ToString();
                mensajexml = mensajexml.Replace(" />", "/>");
                mensajexml = mensajexml.Substring(39);
                mensajexml = HttpUtility.HtmlDecode(mensajexml);

                var resultado = _objBoreal.ExecuteAsync(HttpUtility.HtmlDecode(mensajexml)).Result;

                logResult(mensajexml, resultado.Egresoxml, "E");

                var plan = "";
                var afiliado = "";

                using (var reader = XmlReader.Create(new StringReader(resultado.Egresoxml)))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case "AfiliadoPlanDes":
                                plan = reader.ReadElementContentAsString();
                                break;
                            case "AfiliadoNombre":
                                afiliado = reader.ReadElementContentAsString();
                                break;
                        }
                    }
                }
                return new Afiliado { Name = afiliado, Plan = plan };
            }
            catch (Exception ex)
            {
                var afi = new Afiliado { HasError = true };
                afi.SetError(GetType().Name, 0, ex.Message, ex.InnerException?.ToString() ?? string.Empty, crecencial, string.Empty);
                return afi;
            }
        }

        public AutorizacionOs Autorizar(Authorize model)
        {

            var posicion = model.Credencial.IndexOf("/");
            if (posicion < 0)
            {
                return new AutorizacionOs { HasError = true, Error = new Error { Mensaje = "Credencial con formato incorrecto" } };
            }

            var linea = 1;

            //armado del xml de solicitud
            var output = new StringBuilder();
            using (var writer = XmlWriter.Create(output))
            {
                writer.WriteStartElement("BOREAL");
                writer.WriteStartElement("Mensaje");
                writer.WriteElementString("Canal", "ID");
                writer.WriteElementString("SitioEmisor", "CMSws");
                writer.WriteElementString("Empresa", "BOREAL");
                writer.WriteStartElement("Receptor");
                writer.WriteElementString("Nombre", "BOREAL");
                writer.WriteElementString("ID", "222023");
                writer.WriteElementString("Tipo", "IIN");
                writer.WriteEndElement();
                writer.WriteStartElement("MsgTipo");
                writer.WriteElementString("Tipo", "ZQA");
                writer.WriteElementString("Evento", "Z02");
                writer.WriteElementString("Estructura", "ZQA_Z02");
                writer.WriteEndElement();
                writer.WriteElementString("MsgEntorno", "P");
                writer.WriteEndElement();
                writer.WriteStartElement("Seguridad");
                writer.WriteElementString("Usuario", "cmsws");
                writer.WriteElementString("Clave", _clave);
                writer.WriteEndElement();
                writer.WriteStartElement("Prestador");
                writer.WriteElementString("PrestadorId", "30543364610");
                writer.WriteElementString("PrestadorNombre", "");
                writer.WriteElementString("PrestadorTipoIdent", "CU");
                writer.WriteEndElement();
                writer.WriteStartElement("Afiliado");
                writer.WriteElementString("AfiliadoNroCredencial", model.Credencial.Substring(0, posicion));
                writer.WriteElementString("AfiliadoGf", model.Credencial.Substring(posicion + 1));
                writer.WriteElementString("TipoIdentificador", "HC");
                writer.WriteEndElement();

                writer.WriteStartElement("Practicas");

                foreach (var prestacion in model.Prestaciones)
                {
                    writer.WriteStartElement("Practica");
                    writer.WriteElementString("LineaNro", linea.ToString());
                    writer.WriteElementString("SeccionId", "1");
                    writer.WriteElementString("PracticaId", prestacion.CodPres);
                    writer.WriteElementString("PracticaItem", "");
                    writer.WriteElementString("PracticaCantSol", prestacion.Cant.ToString());
                    writer.WriteElementString("PracticaCantAprob", "");
                    writer.WriteElementString("PracticaDes", ""); //objPrestacion.Descripcion);
                    writer.WriteElementString("PracticaCoseguro", "");
                    writer.WriteElementString("PracticaIdEstado", "");
                    writer.WriteElementString("PracticaObs", "");
                    writer.WriteElementString("PracticaPreAutorizacion", "");
                    writer.WriteEndElement();
                    linea++;
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            var mensajexml = output.ToString();

            mensajexml = mensajexml.Replace(" />", "/>");
            mensajexml = mensajexml.Substring(39);

            //try
            //{
            //    var resultado = _objBoreal.ExecuteAsync(HttpUtility.HtmlDecode(mensajexml)).Result;
            //    logResult(mensajexml, resultado.Egresoxml, "A");

            //    return SetAutorizacionOs(resultado.Egresoxml, model);

            //    //Todo: Si vuelve Vacio error, ver como tratar
            //}
            //catch (Exception ex)
            //{
            //    var autorizacionOs = new AutorizacionOs();
            //    autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            //    return autorizacionOs;
            //}
            return null;
        }

        private AutorizacionOs SetAutorizacionOs(string data, Authorize model)
        {
            var autorizacionOs = new AutorizacionOs();
            try
            {
                var det = new AutorizacionOsDet();
                var osrepository = new OSRepository();
                autorizacionOs = new AutorizacionOs
                {
                    NnroAfiliado = model.Credencial,
                    Idpre = Convert.ToInt32(model.PrestadorId),
                    //  Ncodosoc = osrepository.GetOSbyId(model.OSId),
                    Nestado = 0,
                    NidUsuario = Convert.ToInt32(model.UserId),
                    DfecEstado = DateTime.Today.ToString(),
                    CcodAnulacion = "",
                    Idfacturador = Convert.ToInt32(model.FacturadorId),
                };

                var fechaint = "";

                using (var reader = XmlReader.Create(new StringReader(data)))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case "AutCod":
                                var sIdTransaccion = reader.ReadElementContentAsString();
                                autorizacionOs.CcodinternoAutorizacion = sIdTransaccion;
                                autorizacionOs.CnroAutorizacion = sIdTransaccion;
                                det.PracticaAuthNr = sIdTransaccion;
                                break;
                            case "AutEstadoId":
                                var sEstado = reader.ReadElementContentAsString();
                                //sCodEstado = sEstado;
                                switch (sEstado)
                                {
                                    case "B000": sEstado = "Autorizada"; break;
                                    case "B001": sEstado = "Autorizada Parcial"; break;
                                    default: sEstado = "Rechazada"; break;
                                }
                                autorizacionOs.ResultadoAutorizacion = sEstado;
                                autorizacionOs.EstadoAutorizacion = sEstado;
                                break;
                            case "AutObs":
                                autorizacionOs.Error.Mensaje = reader.ReadElementContentAsString();
                                break;
                            case "AfiliadoPlanDes":
                                autorizacionOs.CdescripcionPlan = reader.ReadElementContentAsString();
                                break;
                            case "AfiliadoNombre":
                                autorizacionOs.CnomAfiliado = reader.ReadElementContentAsString();
                                break;
                            case "AfiliadoIVADes":
                                autorizacionOs.CdescripcionIva = reader.ReadElementContentAsString();
                                break;
                            case "Year":
                                fechaint = "20" + reader.ReadElementContentAsString() + "-";
                                break;
                            case "Mes":
                                fechaint += reader.ReadElementContentAsString() + "-";
                                break;
                            case "Dia":
                                fechaint += reader.ReadElementContentAsString() + " ";
                                break;
                            case "Hora":
                                fechaint += reader.ReadElementContentAsString() + ":";
                                break;
                            case "Minutos":
                                fechaint += reader.ReadElementContentAsString() + ":";
                                break;
                            case "Seg":
                                fechaint += reader.ReadElementContentAsString();
                                autorizacionOs.DfecAutorizacion = fechaint;
                                break;
                            case "Practicas":

                                while (reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        switch (reader.Name)
                                        {
                                            case "PracticaId":
                                                det.PracticaId = reader.ReadElementContentAsString();
                                                break;
                                            case "PracticaDes":
                                                det.PracticaDes = reader.ReadElementContentAsString();
                                                break;
                                            case "PracticaIdEstado":
                                                var sEstadox = reader.ReadElementContentAsString().Trim();
                                                det.PracticaIdEstado = sEstadox == "B000" || sEstadox == "B001"
                                                    ? "Autorizada"
                                                    : "Rechazada";
                                                break;
                                            case "PracticaCantAprob":
                                                det.PracticaCantAprob = reader.ReadElementContentAsString();
                                                break;
                                            case "PracticaObs":
                                                det.PracticaDetAuth = reader.ReadElementContentAsString();
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (reader.NodeType != XmlNodeType.EndElement || reader.Name != "Practica")
                                            continue;
                                        det.PracticaDetAuth = det.PracticaDetAuth == string.Empty ? "OK" : det.PracticaDetAuth;
                                        autorizacionOs.AutorizacionOsDet.Add(det);
                                        det = new AutorizacionOsDet { PracticaAuthNr = autorizacionOs.CnroAutorizacion };
                                    }
                                }
                                break;
                        }
                    }
                }
                return autorizacionOs;
            }
            catch (Exception ex)
            {
                autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return autorizacionOs;
        }

        public AnulacionOS Anular(Anular model)
        {
            try
            {
                var authRepo = new AutorizacionRepository();
                var sIdInternoAut = authRepo.GetIdAuth(model.AuthId);

                //armado del xml de solicitud
                var output = new StringBuilder();
                using (var writer = XmlWriter.Create(output))
                {
                    writer.WriteStartElement("BOREAL");
                    writer.WriteStartElement("Mensaje");
                    writer.WriteElementString("Canal", "ID");
                    writer.WriteElementString("SitioEmisor", "CMSws");
                    writer.WriteElementString("Empresa", "BOREAL");
                    writer.WriteStartElement("Receptor");
                    writer.WriteElementString("Nombre", "BOREAL");
                    writer.WriteElementString("ID", "222023");
                    writer.WriteElementString("Tipo", "IIN");
                    writer.WriteEndElement();
                    writer.WriteStartElement("MsgTipo");
                    writer.WriteElementString("Tipo", "ZQA");
                    writer.WriteElementString("Evento", "Z04");
                    writer.WriteElementString("Estructura", "ZQA_Z02");
                    writer.WriteEndElement();
                    writer.WriteElementString("MsgEntorno", "P");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Seguridad");
                    writer.WriteElementString("Usuario", "cmsws");
                    writer.WriteElementString("Clave", _clave);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Prestador");
                    writer.WriteElementString("PrestadorId", "30543364610");
                    writer.WriteElementString("PrestadorNombre", "");
                    writer.WriteElementString("PrestadorTipoIdent", "CU");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Autorizacion");
                    writer.WriteElementString("AutCod", "");
                    writer.WriteElementString("AutEstadoId", "");
                    writer.WriteElementString("AutObs", "");
                    writer.WriteElementString("AutCodAnulacion", sIdInternoAut);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                }

                var mensajexml = output.ToString();
                mensajexml = mensajexml.Replace(" />", "/>");

                try
                {
                    var resultado = _objBoreal.ExecuteAsync(HttpUtility.HtmlDecode(mensajexml)).Result;
                    logResult(mensajexml, resultado.Egresoxml, "A");

                    return SetAnulacionOs(resultado.ToString(), model);

                    //Todo: Si vuelve Vacio error, ver como tratar
                }
                catch (Exception ex)
                {
                    var anulacionOs = new AnulacionOS();
                    anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), Mensajes.Get("AnulFail"), ex.InnerException?.ToString() ?? "", model, string.Empty);
                    return anulacionOs;
                }
            }
            catch (Exception ex)
            {
                var anulacionOs = new AnulacionOS();
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
                return anulacionOs;
            }
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

                var sEstado = " ";
                var fechaint = "";

                using (var reader = XmlReader.Create(new StringReader(data)))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case "AutCod":
                                anulacionOs.CodAnulacion = reader.ReadElementContentAsString();
                                break;
                            case "AutEstadoId":
                                sEstado = reader.ReadElementContentAsString();
                                sEstado = sEstado == "B000" ? "OK" : "NO";
                                break;
                            case "Year":
                                fechaint = "20" + reader.ReadElementContentAsString() + "-";
                                break;
                            case "Mes":
                                fechaint += reader.ReadElementContentAsString() + "-";
                                break;
                            case "Dia":
                                fechaint += reader.ReadElementContentAsString() + " ";
                                break;
                            case "Hora":
                                fechaint += reader.ReadElementContentAsString() + ":";
                                break;
                            case "Minutos":
                                fechaint += reader.ReadElementContentAsString() + ":";
                                break;
                            case "Seg":
                                fechaint += reader.ReadElementContentAsString();
                                anulacionOs.Fecha = fechaint;
                                break;
                        }
                    }
                }

                if (sEstado != "OK")
                {
                    anulacionOs.HasError = true;
                    anulacionOs.Error.Mensaje = sEstado;
                }
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return anulacionOs;
        }

        private string Clave_Boreal()
        {
            var momento = DateTime.Now;
            var respuesta = "26677736";
            // Año
            var valor = momento.Year;
            var sumatemp = 2557 + valor;
            respuesta += sumatemp.ToString();
            // Mes
            valor = momento.Month;
            sumatemp = 0 + valor;
            var tempo = sumatemp.ToString();
            respuesta += tempo.PadLeft(2, '0');
            // Dia
            valor = momento.Day;
            sumatemp = 10 + valor;
            respuesta += sumatemp.ToString();
            return respuesta;
        }
    }
}
