using System;
using System.IO;
using System.Text;
using System.Xml;
using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Repository.OS
{
    public class OSPatrones: ObrasSociales
    {
        Activia.WSActiviaCSoapClient  service;       //Servidor de Prueba
        //activiaweb.WSActiviaC service;              //Servidor de Produccion

        public OSPatrones()
        {
            service = new Activia.WSActiviaCSoapClient(Activia.WSActiviaCSoapClient.EndpointConfiguration.WSActiviaCSoap);     //Servidor de Prueba
            //service = new activiaweb.WSActiviaC();        //Servidor de Produccion
        }

        public Afiliado Eligibilidad(string credencial, int matricula)
        {
            var afiliado = new Afiliado();
            try
            {
                string resultado;

                var output = new StringBuilder();
                using (var writer = XmlWriter.Create(output))
                {
                    writer.WriteStartElement("Mensaje");

                    writer.WriteStartElement("EncabezadoMensaje");
                    writer.WriteElementString("VersionMsj", "ACT20");
                    writer.WriteElementString("TipoMsj", "OL");
                    writer.WriteElementString("TipoTransaccion", "01A");
                    writer.WriteStartElement("InicioTrx");
                    writer.WriteElementString("FechaTrx", DateTime.Now.ToString("yyyyMMdd"));
                    writer.WriteEndElement();
                    writer.WriteStartElement("Terminal");
                    writer.WriteElementString("TipoTerminal", "PC");
                    writer.WriteElementString("NumeroTerminal", "60000001");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Financiador");
                    writer.WriteElementString("CodigoFinanciador", "PATCAB");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Prestador");
                    writer.WriteElementString("CuitPrestador", "30543364610");
                    writer.WriteElementString("RazonSocial", "Circulo Medico de Salta");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteStartElement("EncabezadoAtencion");
                    writer.WriteStartElement("Credencial");
                    writer.WriteElementString("NumeroCredencial", credencial); //"0100002201"
                    writer.WriteElementString("ModoIngreso", "M");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                try
                {
                    resultado = service.ExecuteFileTransactionSLAsync("0000", output.ToString()).Result;
                    logResult(output.ToString(), resultado.ToString(), "E");
                }
                catch (Exception ex)
                {
                    afiliado.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", credencial + ";" + matricula, string.Empty);
                    afiliado.Error.Mensaje = Mensajes.Get("ServidorNoResponde");
                    afiliado.Name = Mensajes.Get("ServidorNoResponde");

                    resultado = "";
                }

                if (resultado == "")
                {
                    afiliado.Name = Mensajes.Get("AfiIne");
                }
                else
                {
                    var nombre = "";
                    var plan = "";
                    using (var reader = XmlReader.Create(new StringReader(resultado)))
                    {
                        reader.MoveToContent();
                        while (reader.Read())
                        {
                            if (reader.NodeType != XmlNodeType.Element) continue;
                            switch (reader.Name)
                            {
                                case "NombreBeneficiario":
                                    nombre = reader.ReadElementContentAsString();
                                    break;
                                case "PlanCredencial":
                                    plan = reader.ReadElementContentAsString();
                                    break;
                            }
                        }
                    }
                    afiliado.Name = nombre.Trim() != "" ? nombre.Trim() : Mensajes.Get("AfiIne");
                    afiliado.Plan = plan.Trim();
                }
            }
            catch (Exception ex)
            {
                afiliado.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", credencial + ";" + matricula, string.Empty);
            }
            return afiliado;
        }

        public AutorizacionOs Autorizar(Authorize model)
        {
            var output = new StringBuilder();
            using (var writer = XmlWriter.Create(output))
            {
                writer.WriteStartElement("Mensaje");

                writer.WriteStartElement("EncabezadoMensaje");
                writer.WriteElementString("VersionMsj", "ACT20");
                writer.WriteElementString("TipoMsj", "OL");
                writer.WriteElementString("TipoTransaccion", "02A");

                writer.WriteStartElement("InicioTrx");
                writer.WriteElementString("FechaTrx", DateTime.Now.ToString("yyyyMMdd"));
                writer.WriteElementString("HoraTrx", DateTime.Now.ToString("hhmmss"));
                writer.WriteEndElement();
                writer.WriteStartElement("Terminal");
                writer.WriteElementString("TipoTerminal", "PC");
                writer.WriteElementString("NumeroTerminal", "60000001");
                writer.WriteEndElement();
                writer.WriteStartElement("Financiador");
                writer.WriteElementString("CodigoFinanciador", "PATCAB");
                writer.WriteEndElement();
                writer.WriteStartElement("Prestador");
                writer.WriteElementString("CuitPrestador", "30543364610");
                writer.WriteElementString("RazonSocial", "Circulo Medico de Salta");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("EncabezadoAtencion");
                writer.WriteStartElement("Credencial");
                writer.WriteElementString("NumeroCredencial", model.Credencial);  //model.Credencial); //"0100002201"
                writer.WriteElementString("VersionCredencial", "M");
                writer.WriteElementString("ModoIngreso", "00");
                writer.WriteEndElement();
                writer.WriteEndElement();

                foreach (var prestacion in model.Prestaciones)
                {
                    writer.WriteStartElement("DetalleProcedimientos");
                    writer.WriteElementString("CodPrestacion", prestacion.CodPres);
                    writer.WriteElementString("TipoPrestacion", "1");
                    writer.WriteElementString("CantidadSolicitada", prestacion.Cant.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            try
            {
                var resultado = service.ExecuteFileTransactionSLAsync("0000", output.ToString()).Result;
                logResult(output.ToString(), resultado.ToString(), "A");

                return SetAutorizacionOs(resultado, model);
                
            }
            catch (Exception ex)
            {
                var autorizacionOs = new AutorizacionOs();
                autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
                return autorizacionOs;
            }
        }

        private AutorizacionOs SetAutorizacionOs(string data, Authorize model)
        {
            //CodRtaGeneral  01-Rechazo Total, 00-Autorizado Parcial/Total
            //MensajeRta Esta el error por practica
            // Si la cantidad aprobada coinside con la solicitada, esta aprobada, sino no

            //NroReferencia: Numero de autorizacion
            //Plan PlanCredencial
            //NombreBeneficiario Nombre
            //NumeroCredencial Credencia
            //CondicionIVA 


            var autorizacionOs = new AutorizacionOs();
            try
            {
                var det = new AutorizacionOsDet();
                var osrepository = new OSRepository();
                autorizacionOs = new AutorizacionOs
                {
                    NnroAfiliado = model.Credencial,
                    Idpre = Convert.ToInt32(model.PrestadorId),
                    Ncodosoc = osrepository.GetOSbyId(model.OSId),
                    Nestado = 0,
                    NidUsuario = Convert.ToInt32(model.UserId),
                    DfecEstado = DateTime.Today.ToString(),
                    CcodAnulacion = "",
                    Idfacturador = Convert.ToInt32(model.FacturadorId),
                };

                var fechaint = "";

                using (var reader = XmlReader.Create(new StringReader(data)))
                {
                    var sIdTransaccion = "";
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case "NroReferencia":
                                sIdTransaccion = reader.ReadElementContentAsString();
                                autorizacionOs.CcodinternoAutorizacion = sIdTransaccion;
                                autorizacionOs.CnroAutorizacion = sIdTransaccion;
                                det.PracticaAuthNr = sIdTransaccion;
                                break;
                            case "CodRtaGeneral":
                                var sEstado = reader.ReadElementContentAsString();
                                //sCodEstado = sEstado;
                                switch (sEstado)
                                {
                                    case "01": sEstado = "Rechazada"; break;
                                    case "00": sEstado = "Autorizada"; break;//Puede ser Parcial dependiendo de las practicas
                                    default: sEstado = "Autorizada"; break; ///Todo: Estudiar los casos
                                }
                                autorizacionOs.ResultadoAutorizacion = sEstado;
                                autorizacionOs.EstadoAutorizacion = sEstado;
                                break;
                            case "DescripcionRtaGeneral":
                                autorizacionOs.Error.Mensaje = reader.ReadElementContentAsString();
                                break;
                            case "PlanCredencial":
                                autorizacionOs.CdescripcionPlan = reader.ReadElementContentAsString();
                                break;
                            case "NombreBeneficiario":
                                autorizacionOs.CnomAfiliado = reader.ReadElementContentAsString();
                                break;
                            case "CondicionIVA":
                                autorizacionOs.CdescripcionIva = reader.ReadElementContentAsString();//Cuales son las posibles respuestas de IVA
                                break;
                            case "FechaTrx":
                                fechaint = reader.ReadElementContentAsString(); //formatear fecha yyyy-MM-dd
                                break;
                            case "HoraTrx":
                                fechaint += reader.ReadElementContentAsString() + ":"; //formatear hora
                                var date = fechaint.Substring(0, 4) + "-" + fechaint.Substring(4, 2) + "-" + fechaint.Substring(6, 2) + " " + fechaint.Substring(8, 2) + ":" + fechaint.Substring(10, 2) + ":" + fechaint.Substring(12, 2);
                                autorizacionOs.DfecAutorizacion = date;
                                break;
                            case "DetalleProcedimientos":
                                var cantSoli = 0;
                                var cantAp = 0;
                                while (reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        switch (reader.Name)
                                        {
                                            case "CodPrestacion":
                                                det.PracticaId = reader.ReadElementContentAsString();
                                                break;
                                            case "MensajeRta":
                                                det.PracticaDetAuth = reader.ReadElementContentAsString();
                                                break;
                                            case "CantidadAprobada":
                                                cantAp = Convert.ToInt32(reader.ReadElementContentAsString().Trim());
                                                if (cantAp == 0)
                                                {
                                                    if (autorizacionOs.ResultadoAutorizacion != "Rechazada")
                                                    {
                                                        autorizacionOs.ResultadoAutorizacion = "Parcialmente Autorizada";
                                                        autorizacionOs.EstadoAutorizacion = "Parcialmente Autorizada";
                                                    }
                                                    det.PracticaIdEstado = "Rechazada";
                                                }
                                                break;
                                            case "CantidadSolicitada":
                                                cantSoli = Convert.ToInt32(reader.ReadElementContentAsString());
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (reader.NodeType != XmlNodeType.EndElement || reader.Name != "DetalleProcedimientos")
                                            continue;
                                        //det.PracticaDetAuth = "OK";
                                        det.PracticaIdEstado = (cantSoli == cantAp) ? "Autorizada" : (cantAp == 0) ? "Rechazada" : "Parcialmente Autorizada";
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
            var anulacionOs = new AnulacionOS();

            try
            {
                var authRepo = new AutorizacionRepository();
                var sIdInternoAut = authRepo.GetIdAuth(model.AuthId);

                var output = new StringBuilder();
                using (var writer = XmlWriter.Create(output))
                {
                    writer.WriteStartElement("Mensaje");
                    writer.WriteStartElement("EncabezadoMensaje");
                    writer.WriteElementString("VersionMsj", "ACT20");
                    writer.WriteElementString("NroReferenciaCancel", sIdInternoAut);
                    writer.WriteElementString("TipoMsj", "OL");
                    writer.WriteElementString("TipoTransaccion", "04A");
                    writer.WriteStartElement("InicioTrx");
                    writer.WriteElementString("FechaTrx", DateTime.Now.ToString("yyyyMMdd"));
                    writer.WriteEndElement();
                    writer.WriteStartElement("Terminal");
                    writer.WriteElementString("TipoTerminal", "PC");
                    writer.WriteElementString("NumeroTerminal", "60000001");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Financiador");
                    writer.WriteElementString("CodigoFinanciador", "PATCAB");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Prestador");
                    writer.WriteElementString("CuitPrestador", "30543364610");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                var resultado = service.ExecuteFileTransactionSLAsync("0000", output.ToString()).Result;
                logResult(output.ToString(), resultado.ToString(), "A");

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

                using (var reader = XmlReader.Create(new StringReader(data)))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) continue;
                        switch (reader.Name)
                        {
                            case "SystemTrace":
                                anulacionOs.CodAnulacion = reader.ReadElementContentAsString();
                                anulacionOs.Estado = "Anulada";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return anulacionOs;
        }
    }
}
