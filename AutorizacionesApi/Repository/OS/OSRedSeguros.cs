using System;
using System.IO;
using System.Net;
using WebApi.Models;
using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Repository.OS
{
    public class OSRedSeguros : ObrasSociales
    {
        public Afiliado Eligibilidad(string credencial)
        {
            const string url = "http://ws1.rsmprestadores.com/datos_afil.php?cuit=30-54336461-0&clave=4610&afil=";
            try
            {
                var request = WebRequest.Create(url + credencial);            // Create a request for the URL.
                var response = request.GetResponse();                         // Get the response.
                var dataStream = response.GetResponseStream();                // Get the stream containing content returned by the server.
                var reader = new StreamReader(dataStream);                    // Open the stream using a StreamReader for easy access.
                var resultado = reader.ReadToEnd();                           // Read the content.
                reader.Close();                                               // Display the content.
                response.Close();

                var resul = resultado.Split(':');
                var datos = resul[1].Split(',');

                logResult(request.ToString(), resultado.ToString(), "E");

                if (resul[0].Trim() == "OK")
                {
                    return new Afiliado
                    {
                        Name = datos[1],
                        Plan = datos[4]
                    };
                }
                return new Afiliado { Error = new Error { Mensaje = "Credencial con formato incorrecto" }, HasError = true, Name = "Credencial con formato incorrecto" };
            }
            catch (Exception ex)
            {
                var afi = new Afiliado();
                afi.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? string.Empty, credencial, string.Empty);
                return afi;
            }
        }

        public AutorizacionOs Autorizar(Authorize model)
        {
            const string url = "http://ws1.rsmprestadores.com/validar.php?cuit=30-54336461-0&clave=4610&afil=";
            try
            {
                var cadena = "";

                foreach (var prestacion in model.Prestaciones)
                {
                    var codPrest = prestacion.CodPres;
                    var cantidad = prestacion.Cant.ToString();
                    if (cadena.Length > 1)
                    {
                        cadena += ",";
                    }
                    cadena += codPrest + "-" + cantidad;
                }

                var request = WebRequest.Create(url + model.Credencial + "&nomen=2&codigos=" + cadena);
                var response = request.GetResponse();
                var dataStream = response.GetResponseStream();
                var reader = new StreamReader(dataStream);
                var resultado = reader.ReadToEnd();

                reader.Close();
                response.Close();

                logResult(request.ToString(), resultado.ToString(), "A");

                return Autorizar(model, resultado, cadena);
            }
            catch (Exception ex)
            {
                var autorizacionOs = new AutorizacionOs { HasError = true };
                autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
                return autorizacionOs;
            }
        }

        public AutorizacionOs Autorizar(Authorize model, string resultado, string cadena)
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

                var resul = resultado.Split(':');
                var solicitud = cadena.Split(',');
                var datosAut = resul[0].Split(',');
                var datosRec = resul[1].Split(',');

                string sEstado;
                var sNroAutorizacion = "0";
                string sMsgCorto;

                autorizacionOs.DfecAutorizacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // resultado autorización 
                if (datosRec[0].Trim() != "RECHAZ")
                {
                    sEstado = "Autorizada";
                    sNroAutorizacion = datosAut[0];
                    sMsgCorto = "Autorizada";
                }
                else
                {
                    if (datosAut[0].Trim() == "")
                    {
                        sEstado = "Rechazada";
                        sMsgCorto = "Rechazada";
                    }
                    else
                    {
                        sEstado = "Autorizada Parcial";
                        sNroAutorizacion = datosAut[0];
                        sMsgCorto = "Aut. parcial";
                    }
                }

                autorizacionOs.ResultadoAutorizacion = sEstado;
                autorizacionOs.EstadoAutorizacion = sEstado;
                autorizacionOs.CnroAutorizacion = sNroAutorizacion;
                autorizacionOs.CcodinternoAutorizacion = sNroAutorizacion;
                autorizacionOs.Mensaje = sMsgCorto;
                autorizacionOs.CnomAfiliado = model.AfiliadoNombre;
                autorizacionOs.CdescripcionPlan = model.AfiliadoPlan;


                if (datosAut.Length > 1)
                {
                    for (var i = 1; i < datosAut.Length; i++)
                    {
                        det.PracticaId = datosAut[i].Replace(".", "").Trim();

                        for (var j = 0; j <= solicitud.Length; j++)
                        {
                            var prestasol = solicitud[j].Split('-');
                            if (det.PracticaId != prestasol[0].Trim()) continue;
                            det.PracticaCantAprob = prestasol[1];
                            break;
                        }

                        det.PracticaDes = osrepository.GetPrescDesc(det.PracticaId, model.Prestaciones, model);
                        det.PracticaIdEstado = "Autorizada";
                        autorizacionOs.AutorizacionOsDet.Add(det);
                        det = new AutorizacionOsDet();
                    }
                }

                // se graban las rechazadas
                if (datosRec.Length > 1)
                {
                    for (var i = 1; i < datosRec.Length; i++)
                    {
                        var presta = datosRec[i].Split('-');
                        det.PracticaId = datosRec[i].Replace(".", "").Trim();

                        switch (presta[1])
                        {
                            case "1":
                                sMsgCorto = "El codigo es de alta complejidad, se debe emitir con autorizacion";
                                break;
                            case "2":
                                sMsgCorto = "La cantidad solicitada supera el tope establecido ";
                                break;
                            case "3":
                                sMsgCorto = "Codigo inexistente en el nomenclador";
                                break;
                        }

                        det.PracticaDetAuth = sMsgCorto;
                        det.PracticaDes = osrepository.GetPrescDesc(det.PracticaId, model.Prestaciones, model);

                        det.PracticaIdEstado = "Rechazada";

                        autorizacionOs.AutorizacionOsDet.Add(det);
                        det = new AutorizacionOsDet();

                    }
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
            const string url = "http://ws1.rsmprestadores.com/anular.php?cuit=30-54336461-0&clave=4610&numorden=";
            var anulacionOs = new AnulacionOS();
            try
            {
                var authRepo = new AutorizacionRepository();
                var sIdInternoAut = authRepo.GetIdAuth(model.AuthId);
                var request = WebRequest.Create(url + sIdInternoAut);
                var response = request.GetResponse();
                var dataStream = response.GetResponseStream();
                var reader = new StreamReader(dataStream);
                var resultado = reader.ReadToEnd();
                reader.Close();
                response.Close();

                logResult(request.ToString(), resultado.ToString(), "D");
                
                if (resultado.Substring(0, 3) == "ERR")
                {
                    anulacionOs.SetError(GetType().Name, 0, resultado, string.Empty, model, string.Empty);
                }
                else
                {
                    anulacionOs = new AnulacionOS
                    {
                        IdAuth = model.AuthId.ToString(),
                        Nestado = "1",
                        Fecha = DateTime.Now.ToString(),
                        CodAnulacion = model.AuthId.ToString(),
                        Estado = "Ok"
                    };
                }
            }
            catch (Exception ex)
            {
                anulacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), Mensajes.Get("AnulFail"), ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return anulacionOs;
        }

    }
}
