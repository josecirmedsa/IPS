using System;
using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Repository.OS
{
    public class OSSwiss: ObrasSociales
    {
        readonly OsTraditum _traditum;
        public OSSwiss()
        {
            _traditum = new OsTraditum();
        }
        public Afiliado Eligibilidad(string credencial)
        {
            string cadena;
            string plan;
            var afiliado = new Afiliado();
            try
            {
                var output = "MSH|^~\\&|TRIA0100M|TRIA00007160|SWISSHL7|SWISS^800006^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQI^Z01^ZQI_Z01|08050522304540783782|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "PRD|PS^Prestador Solicitante||^^^A||||30543364610^CU|";

                output += Environment.NewLine;
                output += "PID|||" + credencial + "^^^SWISS^HC||UNKNOWN";
                output += Environment.NewLine;
                
                //Call WebService
                var resultado = _traditum.Send(output);
                logResult(output.ToString(), resultado.ToString(), "E");

                var OSerror = false;
                if (resultado == "") return new Afiliado { Name = Mensajes.Get("AfiIne") };

                if (resultado.Contains("Error ejecutando") || resultado.Contains("no se pueden procesar") || resultado.Contains("Unable to read data") || resultado.Contains("El cliente encontró el tipo de contenido de respuesta"))
                {
                    OSerror = OsStatus.checkSwiss(true); 
                    afiliado.Name = Mensajes.Get("ServidorNoResponde");
                    afiliado.SetError(GetType().Name, 37, Mensajes.Get("ServidorNoResponde"), string.Empty, credencial, string.Empty, OSerror);
                    return afiliado;
                }
                // convertimos respuesta en vector
                var msHL7 = HL7.DecifraHL7(resultado);
                var index = 0;
                if (msHL7.Length > 1)
                {
                    index = msHL7[1].IndexOf("En estos momentos, no se pueden procesar transacciones");
                }
                else
                {
                    index = 2;
                }
                if (index > 0)
                {
                    OSerror = OsStatus.checkSwiss(true);
                    afiliado.Name = Mensajes.Get("ServidorNoResponde");
                    afiliado.SetError(GetType().Name, 37, Mensajes.Get("ServidorNoResponde"), string.Empty, credencial, string.Empty, OSerror);
                }
                else
                {
                    OSerror = OsStatus.checkSwiss(false);
                    if (HL7.CampoHL7(msHL7[2], 3, 1) == "B000")
                    {
                        cadena = HL7.CampoHL7(msHL7[4], 5, 1) + ", " + HL7.CampoHL7(msHL7[4], 5, 2);
                        plan = HL7.CampoHL7(msHL7[5], 2, 0);
                    }
                    else
                    {
                        cadena = Mensajes.Get("AfiIne");
                        plan = "";
                    }
                    afiliado.Name = cadena;
                    afiliado.Plan = plan;
                }
            }
            catch (Exception ex)
            {
                var afi = new Afiliado { HasError = true };
                afi.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, credencial, string.Empty);
                return afi;
            }
            return afiliado;
        }
        public AutorizacionOs Autorizar(Authorize model)
        {
            var autorizacionOs = new AutorizacionOs();
            try
            {
                var outputpres = "";
                var cuenta = 1;

                var output = "MSH|^~\\&|TRIA0100M|TRIA00007160|SWISSHL7|SWISS^800006^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQA^Z02^ZQA_Z02|08050522304540783782|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "PRD|PS^CIRCULO MEDICO DE SALTA||^^^A||||30543364610^CU|";
                output += Environment.NewLine;
                output += "PRD|EF^" + model.Efector.Name + "||^^^C||||" + model.Efector.Cuit + "^CU&M&C|";
                output += Environment.NewLine;
                output += "PID|||800006" + model.Credencial + "^^^SWISS^HC||UNKNOWN";
                output += Environment.NewLine;
                foreach (var prestacion in model.Prestaciones)
                {
                    outputpres += "PR1|" + cuenta + "||" + prestacion.CodPres + Environment.NewLine;
                    outputpres += "AUT||||||||" + prestacion.Cant + "|0" + Environment.NewLine;
                    cuenta++;
                }
                output += outputpres;
                output += "PV1||O||P|||||||||||||||||||||||||||||||||||||||||||||||V";
                output += Environment.NewLine;

               // var resultado = _traditum.Send(output);
                //logResult(output.ToString(), resultado.ToString(), "A");

                //var OSerror = false;
                //if (resultado.Contains("Error ejecutando") || resultado.Contains("no se pueden procesar") || resultado.Contains("Unable to read data"))
                //{
                //    OSerror = OsStatus.checkSwiss(true);
                //    autorizacionOs.ShowMessage = Mensajes.Get("ServidorNoResponde");
                //    autorizacionOs.HasError = true;
                //    autorizacionOs.SetError(GetType().Name, 152, resultado, string.Empty, model, string.Empty, OSerror);
                //    return autorizacionOs;
                //}

                //if (resultado.Substring(0, 4) == "MSH|")
                //{
                //    OSerror = OsStatus.checkSwiss(false);
                //    return SetAutorizacionOs(resultado, model);
                //}

                //OSerror = OsStatus.checkSwiss(true);
                //autorizacionOs.SetError(GetType().Name, 0, resultado, string.Empty, model, string.Empty, OSerror);
            }
            catch (Exception ex)
            {
                autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return autorizacionOs;

        }


        private AutorizacionOs SetAutorizacionOs(string resultado, Authorize model)
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
                    Ncodosoc = osrepository.GetOSbyId(model.OSId),
                    Nestado = 0,
                    NidUsuario = Convert.ToInt32(model.UserId),
                    DfecEstado = DateTime.Today.ToString(),
                    CcodAnulacion = "",
                    Idfacturador = Convert.ToInt32(model.FacturadorId),
                };

                // convertimos respuesta en vector
                var msHL7 = HL7.DecifraHL7(resultado);
                if (msHL7[1].IndexOf("Error") > 0)
                {
                    autorizacionOs.SetError(GetType().Name, 0, msHL7[1].Substring(msHL7[1].IndexOf("Error")), "Error desde Swissmedical", model, resultado);
                    return autorizacionOs;
                }
                // tomamos la fecha de la primera línea del vector - MSH
                var fechaint = HL7.CampoHL7(msHL7[0], 6, 0);

                var sFechaAut = fechaint.Substring(0, 4) + "-" + fechaint.Substring(4, 2) + "-" + fechaint.Substring(6, 2);
                sFechaAut += " " + fechaint.Substring(8, 2) + ":" + fechaint.Substring(10, 2) + ":" + fechaint.Substring(12, 2);

                autorizacionOs.DfecAutorizacion = sFechaAut;

                // número de la transacción cuarta línea - ZAU
                var sIdTransaccion = HL7.CampoHL7(msHL7[3], 2, 0);
                autorizacionOs.CcodinternoAutorizacion = sIdTransaccion;
                autorizacionOs.CnroAutorizacion = sIdTransaccion;

                // resultado autorización tercera línea - ZAU
                var sEstado = HL7.CampoHL7(msHL7[3], 3, 1);
                switch (sEstado)
                {
                    case "B000":
                        sEstado = "Autorizada";
                        break;
                    case "B001":
                        sEstado = "Autorizada Parcial";
                        break;
                    default:
                        sEstado = "Rechazada";
                        break;
                }
                autorizacionOs.EstadoAutorizacion = sEstado;
                autorizacionOs.ResultadoAutorizacion = sEstado;

                // mensaje corto autorizacion cuarta línea - ZAU
                var sMsgCorto = HL7.CampoHL7(msHL7[3], 3, 2);

                // nombre del afiliado sexta linea - PID
                autorizacionOs.CnomAfiliado = HL7.CampoHL7(msHL7[6], 5, 1) + " " + HL7.CampoHL7(msHL7[6], 5, 2);

                // plan del afiliado septima linea - IN1
                autorizacionOs.CdescripcionPlan = HL7.CampoHL7(msHL7[7], 2, 0);

                // condicion IVA octavalinea - ZIN
                autorizacionOs.CdescripcionIva = HL7.CampoHL7(msHL7[8], 2, 2);

                var indice = 8;
                while (msHL7[indice] != "")
                {
                    switch (msHL7[indice].Substring(0, 4))
                    {
                        case "PR1|":
                            det.PracticaId = HL7.CampoHL7(msHL7[indice], 3, 1);
                            HL7.CampoHL7(msHL7[indice], 3, 2);
                            break;
                        case "AUT|":
                            det.PracticaCantAprob = HL7.CampoHL7(msHL7[indice], 9, 0);
                            break;
                        case "ZAU|":
                            var sEstadox = HL7.CampoHL7(msHL7[indice], 3, 1);
                            det.PracticaIdEstado = sEstadox == "B000" || sEstadox == "B001" ? "Autorizada" : "Rechazada";
                            det.PracticaDetAuth = HL7.CampoHL7(msHL7[indice], 3, 2);

                            det.PracticaDes = osrepository.GetPrescDesc(det.PracticaId, model.Prestaciones, model);
                            det.PracticaAuthNr = sIdTransaccion;

                            autorizacionOs.AutorizacionOsDet.Add(det);
                            det = new AutorizacionOsDet();
                            break;
                    }
                    indice++;
                }

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

                var output = "MSH|^~\\&|TRIA0100M|TRIA00007160|SWISSHL7|SWISS^800006^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQA^Z04^ZQA_Z02|08050522304540783782|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "ZAU||" + sIdInternoAut;
                output += Environment.NewLine;
                output += "PRD|PS^CIRCULO MEDICO DE SALTA||^^^A||||30543364610^CU|";
                output += Environment.NewLine;
                output += "PID|||800006" + model.Credencial + "^^^SWISS^HC||UNKNOWN";
                output += Environment.NewLine;

                var resultado = _traditum.Send(output);
                logResult(output.ToString(), resultado.ToString(), "D");

                if (resultado.Substring(0, 4) == "MSH|") return SetAnulacionOs(resultado, model);
                anulacionOs.SetError(GetType().Name, 0, resultado, string.Empty, model, string.Empty);
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), Mensajes.Get("AnulFail"), ex.InnerException?.ToString() ?? "", model, string.Empty);
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

                // convertimos respuesta en vector
                var msHL7 = HL7.DecifraHL7(data);

                // número de la transacción tercera línea - ZAU
                anulacionOs.CodAnulacion = HL7.CampoHL7(msHL7[2], 2, 0);

                // resultado autorización tercera línea - ZAU
                var sEstado = HL7.CampoHL7(msHL7[2], 3, 1);

                anulacionOs.Estado = sEstado == "B000" ? "OK" : "NO";
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, model, string.Empty);
            }
            return anulacionOs;
        }


    }
}
