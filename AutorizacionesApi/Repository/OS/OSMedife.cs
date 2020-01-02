using System;
using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Repository.OS
{
    public class OSMedife : ObrasSociales
    {
        readonly OsTraditum _traditum;
        public OSMedife()
        {
            _traditum = new OsTraditum();
        }

        public Afiliado Eligibilidad(string credencial)
        {
            var afiliado = new Afiliado();
            try
            {
                var output = "MSH|^~\\&|TRIA0100M|TRIA00007160|MEDIFE|MEDIFE^222222^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQI^Z01^ZQI_Z01|05091908480623465897|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "PRD|PS^CIRCULO MEDICO DE SALTA||^^^C||||30543364610^CU|";

                output += Environment.NewLine;
                output += "PID|||" + credencial + "^^^MEDIFE^HC^MEDIFE||UNKNOWN";
                output += Environment.NewLine;

                var resultado = _traditum.Send(output);
                logResult(output.ToString(), resultado.ToString(), "E");

                if (resultado == "") return new Afiliado { Name = Mensajes.Get("AfiIne") };

                if (resultado.Contains("Error ejecutando") || resultado.Contains("no se pueden procesar") || resultado.Contains("Unable to read data"))
                {
                    return new Afiliado { Name = Mensajes.Get("ServidorNoResponde"), HasError = true };
                }

                // convertimos respuesta en vector
                var msHL7 = HL7.DecifraHL7(resultado);
                var index = msHL7[1].IndexOf("En estos momentos, no se pueden procesar transacciones");
                if (index > 0)
                {
                    afiliado.Name = Mensajes.Get("ServidorNoResponde");
                    afiliado.SetError(GetType().Name, 37, Mensajes.Get("ServidorNoResponde"), string.Empty, credencial, string.Empty);
                }
                else
                {
                    if (HL7.CampoHL7(msHL7[2], 3, 1) == "B000")
                    {
                        afiliado.Name = HL7.CampoHL7(msHL7[4], 5, 1) + ", " + HL7.CampoHL7(msHL7[4], 5, 2);
                        afiliado.Plan = HL7.CampoHL7(msHL7[5], 2, 0);
                    }
                    else
                    {
                        afiliado.Name = Mensajes.Get("AfiIne");
                    }
                }


            }
            catch (Exception ex)
            {
                afiliado.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", credencial, string.Empty);
            }
            return afiliado;
        }

        public AutorizacionOs Autorizar(Authorize model)
        {
            var autorizacionOs = new AutorizacionOs();
            try
            {
                var outputpres = "";
                var diagnostico = "Z111";
                var cuenta = 1;

                var output = "MSH|^~\\&|TRIA0100M|TRIA00007160|MEDIFE|MEDIFE^222222^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQA^Z02^ZQA_Z02|11041510051761231631|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "PRD|PS^CIRCULO MEDICO DE SALTA||^^^C||||30543364610^CU|";
                output += Environment.NewLine;
                output += "PRD|EF^" + model.Efector.Name + "||^^^C||||" + model.Efector.Cuit + "^CU&M&C|";
                output += Environment.NewLine;
                output += "PID|||" + model.Credencial + "^^^MEDIFE^HC^MEDIFE||UNKNOWN";
                output += Environment.NewLine;
                foreach (var prestacion in model.Prestaciones)
                {
                    if (prestacion.CodPres.Substring(0, 2) != "42") diagnostico = "Z112";
                    outputpres += "PR1|" + cuenta + "||" + prestacion.CodPres + "^^1" + Environment.NewLine; ;
                    outputpres += "AUT||||||||" + prestacion.Cant + Environment.NewLine; ;
                    outputpres += "ZAU||||||0&$" + Environment.NewLine;
                    cuenta++;
                }
                output += "DG1|1||" + diagnostico + "^^I10|||W";
                output += Environment.NewLine;
                output += outputpres;

                //Todo descomentar esta linea
                //var resultado = _traditum.Send(output);
                //logResult(output.ToString(), resultado.ToString(), "A");

                //if (resultado.Substring(0, 4) == "MSH|") return SetAutorizacionOs(resultado, model);

                //autorizacionOs.SetError(GetType().Name, 0, resultado, string.Empty, model, string.Empty);
            }
            catch (Exception ex)
            {
                autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return autorizacionOs;
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
                    Ncodosoc = osrepository.GetOSbyId(model.OSId),
                    Nestado = 0,
                    NidUsuario = Convert.ToInt32(model.UserId),
                    DfecEstado = DateTime.Today.ToString(),
                    CcodAnulacion = "",
                    Idfacturador = Convert.ToInt32(model.FacturadorId),
                };

                // convertimos respuesta en vector
                var msHL7 = HL7.DecifraHL7(data);

                // tomamos la fecha de la primera línea del vector - MSH
                var fechaint = HL7.CampoHL7(msHL7[0], 6, 0);

                var sFechaAut = fechaint.Substring(0, 4) + "-" + fechaint.Substring(4, 2) + "-" + fechaint.Substring(6, 2);
                sFechaAut += " " + fechaint.Substring(8, 2) + ":" + fechaint.Substring(10, 2) + ":" + fechaint.Substring(12, 2);

                autorizacionOs.DfecAutorizacion = sFechaAut;

                // número de la transacción tercera línea - ZAU **Codigo Interno de la transaccion
                var sIdTransaccion = HL7.CampoHL7(msHL7[2], 2, 0);
                autorizacionOs.CcodinternoAutorizacion = sIdTransaccion;
                autorizacionOs.CnroAutorizacion = sIdTransaccion;

                // resultado autorización tercera línea - ZAU
                var sEstado = HL7.CampoHL7(msHL7[2], 3, 1);
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

                // mensaje corto autorizacion tercera línea - ZAU
                HL7.CampoHL7(msHL7[2], 3, 2);

                // nombre del afiliado quinta linea - PID
                autorizacionOs.CnomAfiliado = (HL7.CampoHL7(msHL7[5], 5, 1) + " " + HL7.CampoHL7(msHL7[5], 5, 2));

                // plan del afiliado sexta linea - IN1
                autorizacionOs.CdescripcionPlan = HL7.CampoHL7(msHL7[6], 2, 0);

                // condicion IVA septima linea - ZIN
                autorizacionOs.CdescripcionIva = HL7.CampoHL7(msHL7[7], 2, 2);

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
                            det.PracticaCantAprob = HL7.CampoHL7(msHL7[indice], 8, 0);
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

                var output = "MSH|^~\\&|TRIA0100M|TRIA00007160|MEDIFE|MEDIFE^222222^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQA^Z04^ZQA_Z02|10121509341187324160|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "ZAU||" + sIdInternoAut;
                output += Environment.NewLine;
                output += "PRD|PS^CIRCULO MEDICO DE SALTA||^^^C||||30543364610^CU|";
                output += Environment.NewLine;
                output += "PID|||" + model.Credencial + "^^^MEDIFE^HC^MEDIFE||UNKNOWN";

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
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return anulacionOs;
        }
    }
}
