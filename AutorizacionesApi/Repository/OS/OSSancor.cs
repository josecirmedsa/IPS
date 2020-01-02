using System;
using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Repository.OS
{
    public class OSSancor : ObrasSociales { 
    
    Sancor.HL7v24Client _sancor;
  
    public OSSancor()
    {
        _sancor = new Sancor.HL7v24Client();
    }
    public Afiliado Elegibilidad(string credencial)
    {
        credencial = "0841233/00";

        string cadena;
        string plan;
        var afiliado = new Afiliado();
        try
        {
            var cred = credencial.Split('/');
            if (cred.Length > 1)
            {
                var output = @"MSH|^~\{|TRIT0100M|TRIT00999999|SANCOR_SALUD|SANCOR_SALUD^604940^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQ^Z01^ZQI_Z01|05091908480623465897|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "PRD|PS^CIRCULO MEDICO DE SALTA||^^^C||||22^CU|";
                output += Environment.NewLine;
                output += "PID|||" + cred[0] + "^" + cred[1] + "^^SANCOR_SALUD^HC||UNKNOWN";
                output += Environment.NewLine;

                Sancor.MessageResponse resultado = _sancor.MessageAsync(8, output).Result;

                logResult(output.ToString(), resultado.ToString(), "E");

                if (resultado.resultado.ToString() == "") return new Afiliado { Name = Mensajes.Get("AfiIne") };
                if (resultado.resultado.ToString().Contains("Error ejecutando") || resultado.resultado.ToString().Contains("no se pueden procesar") || resultado.resultado.ToString().Contains("Unable to read data"))
                {
                    return new Afiliado { Name = Mensajes.Get("ServidorNoResponde"), HasError = true };
                }
                // convertimos respuesta en vector
                var msHL7 = HL7.DecifraHL7Sancor(resultado.resultado.ToString());
                var index = msHL7[1].IndexOf("En estos momentos, no se pueden procesar transacciones");
                if (index > 0)
                {
                    afiliado.Name = Mensajes.Get("ServidorNoResponde");
                    afiliado.SetError(GetType().Name, 37, Mensajes.Get("ServidorNoResponde"), string.Empty, credencial, string.Empty);
                }
                else
                {
                    if (msHL7[4] != null && msHL7[5] != null)
                    {
                        if (HL7.CampoHL7(msHL7[4], 5, 1) != "UNKNOWN")
                        {
                            cadena = HL7.CampoHL7(msHL7[4], 5, 1) + ", " + HL7.CampoHL7(msHL7[4], 5, 2);
                            plan = HL7.CampoHL7(msHL7[5], 3, 0);
                        }
                        else
                        {
                            cadena = Mensajes.Get("AfiIne");
                            plan = "";
                        }
                    }
                    else
                    {
                        cadena = Mensajes.Get("AfiIne"); ;
                        plan = "";
                    }
                    afiliado.Name = cadena;
                    afiliado.Plan = plan;
                }
            }
            else
            {
                afiliado.Name = "Carnet Mal ingresado";
                afiliado.Plan = "";
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
                var diagnostico = "Z111";
                var cuenta = 1;
                var cred = model.Credencial.Split('/');

                var output = "MSH|^~\\&|TRIA0100M|TRIA00000003|SANCOR_SALUD|SANCOR_SALUD^604940^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQA^Z02^ZQA_Z02|11020208170596892946|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "​||||20110201|20110202|||0|0";
                output += Environment.NewLine;
                output += "PRD|PS^CIRCULO MEDICO DE SALTA||||||96065^PR|";
                output += Environment.NewLine;
                output += "PRD|PL^Salta||||||22^PR|";
                output += Environment.NewLine;
                output += "PRD|PE^" + model.Efector.Name + "^^||||||" + model.Efector.Cuit + "^CU|";
                output += Environment.NewLine;
                output += "PRD|PR^" + model.Efector.Name + "||^^^X||||123456^MP&&X|";
                output += Environment.NewLine;
                output += "PID|||" + cred[0] + "^" + cred[1] + "^^SANCOR_SALUD^HC||UNKNOWN";
                output += Environment.NewLine;
                foreach (var prestacion in model.Prestaciones)
                {
                    if (prestacion.CodPres.Substring(0, 2) != "42") diagnostico = "Z112";
                    outputpres += "PR1|1||" + prestacion.CodPres + "^^NM" + Environment.NewLine;
                    outputpres += "AUT||||||||" + prestacion.Cant + Environment.NewLine;
                    // outputpres += "ZAU||||||0&$" + Environment.NewLine;
                    cuenta++;
                }
                output += "DG1|1||" + diagnostico + "^^I10|||W";
                output += Environment.NewLine;
                output += outputpres;

                //Todo descomentar esta linea
                //Sancor.MessageResponse resultado = _sancor.MessageAsync(8, output).Result;
                //logResult(output.ToString(), resultado.ToString(), "A");

                //if (resultado.resultado.Substring(0, 4) == "MSH|") return SetAutorizacionOs(resultado.resultado.ToString(), model);

                //autorizacionOs.SetError(GetType().Name, 0, resultado.resultado.ToString(), string.Empty, model, string.Empty);
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
                var msHL7 = HL7.DecifraHL7Sancor(data);
                if (msHL7[1].IndexOf("Error") > 0)
                {
                    autorizacionOs.SetError(GetType().Name, 0, msHL7[1].Substring(msHL7[1].IndexOf("Error")), "Error desde Swissmedical", model, data);
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
                autorizacionOs.CnomAfiliado = HL7.CampoHL7(msHL7[8], 5, 1) + " " + HL7.CampoHL7(msHL7[8], 5, 2);

                // plan del afiliado septima linea - IN1
                autorizacionOs.CdescripcionPlan = HL7.CampoHL7(msHL7[9], 2, 0).Split('^')[0];

                // condicion IVA octavalinea - ZIN
                autorizacionOs.CdescripcionIva = HL7.CampoHL7(msHL7[10], 2, 2);

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

                var output = "MSH|^~\\&|TRIA0100M|TRIA00000003|SANCOR_SALUD|SANCOR_SALUD^604940^IIN|";
                output += DateTime.Now.ToString("yyyyMMddHHmmss") + "||ZQA^Z04^ZQA_Z04|18071613545151234567890|P|2.4|||NE|AL|ARG";
                output += Environment.NewLine;
                output += "ZAU||" + sIdInternoAut;
                output += Environment.NewLine;
                output += "PRD|PS^CIRCULO MEDICO DE SALTA||^^^C||||96065^PR|";


                output += Environment.NewLine;
                output += "PID|||" + model.Credencial + "^^^SANCOR_SALUD^HC^SANCOR_SALUD||UNKNOWN";

                Sancor.MessageResponse resultado = _sancor.MessageAsync(8, output).Result;
                logResult(output.ToString(), resultado.ToString(), "D");

                if (resultado.resultado.Substring(0, 4) == "MSH|") return SetAnulacionOs(resultado.ToString(), model);
                anulacionOs.SetError(GetType().Name, 0, resultado.resultado.ToString(), string.Empty, model, string.Empty);
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
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
                var msHL7 = HL7.DecifraHL7Sancor(data);

                // número de la transacción tercera línea - ZAU
                anulacionOs.CodAnulacion = HL7.CampoHL7(msHL7[2], 2, 0);

                // resultado autorización tercera línea - ZAU
                var sEstado = HL7.CampoHL7(msHL7[2], 3, 1);

                anulacionOs.Estado = sEstado == "B000" ? "OK" : "NO";
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), Mensajes.Get("AnulFail"), ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return anulacionOs;
        }
    }

    public class SancorResponse
    {
        public string Estado { get; set; }
        public string Descipcion { get; set; }
        public string Afiliado { get; set; }
        public string Plan { get; set; }
    }
}
