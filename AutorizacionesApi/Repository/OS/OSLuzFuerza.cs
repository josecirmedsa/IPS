using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Repository.OS
{
    public class LuzFuerzaResponse
    {
        public string success { get; set; }
        public string name { get; set; }
        public string authcod { get; set; }
        public string error { get; set; }
    }

    public class OSLuzFuerza : ObrasSociales
    {
        public async Task<Afiliado> Eligibilidad(string credencial, int matricula)
        {
            var afiliado = new Afiliado();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://api.osgestion.com.ar/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var cred = credencial.Replace("-", "").Replace("/", "");
                    if (cred.Length == 10)
                    {
                        // var url = "api/crear-consulta/100998/" + cred + "/" + matricula;
                        var url = "api/crear-consulta/" + cred + "/" + matricula;

                        Task<HttpResponseMessage> response = client.GetAsync(url);
                        var resultado = await response.Result.Content.ReadAsStringAsync();
                        logResult(url.ToString(), resultado.ToString(), "E");
                        
                        var luz = (LuzFuerzaResponse)JsonConvert.DeserializeObject(resultado, typeof(LuzFuerzaResponse));


                        if (luz.success.ToUpper() == "TRUE")
                        {
                            afiliado.Name = luz.name;
                            afiliado.HasError = false;
                        }
                        else
                        {
                            afiliado.Name = luz.error;
                            afiliado.HasError = true;
                        }
                        afiliado.Plan = luz.authcod;
                    }
                    else
                    {
                        afiliado.Name = "Error el formato del carnet es incorrecto! Ej. xx-xxxxx-x/xx";
                        afiliado.HasError = true;
                    }
                }
            }
            catch (Exception ex)
            {
                afiliado.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", credencial + ";" + matricula, string.Empty);
                afiliado.Error.Mensaje =  Mensajes.Get("ServidorNoResponde") ;
                afiliado.Name = Mensajes.Get("ServidorNoResponde");
            }
            return afiliado;
        }

        public AutorizacionOs Autorizar(Authorize model, string codAuth)
        {
            return SetAutorizacionOs(model, codAuth);
        }

        private AutorizacionOs SetAutorizacionOs(Authorize model, string codAuth)
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
                autorizacionOs.NnroAfiliado = model.Credencial;
                autorizacionOs.CnomAfiliado = model.AfiliadoNombre;
                autorizacionOs.Idpre = Convert.ToInt32(model.PrestadorId);
                autorizacionOs.Ncodosoc = osrepository.GetOSbyId(10);
                autorizacionOs.Nestado = 0;
                autorizacionOs.NidUsuario = Convert.ToInt32(model.UserId);
                autorizacionOs.CcodAnulacion = "";
                autorizacionOs.Idfacturador = Convert.ToInt32(model.FacturadorId);
                autorizacionOs.CnroAutorizacion = codAuth;
                autorizacionOs.ResultadoAutorizacion = "Autorizada";
                autorizacionOs.EstadoAutorizacion = "Autorizada";
                autorizacionOs.DfecAutorizacion = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                autorizacionOs.CcodinternoAutorizacion = codAuth;

                AutorizacionOsDet autorizacionOsDet = new AutorizacionOsDet();
                foreach (var item in model.Prestaciones)
                {

                    autorizacionOsDet.PracticaId = item.CodPres;
                    autorizacionOsDet.PracticaDetAuth = "Autorizada";
                    autorizacionOsDet.PracticaCantAprob = item.Cant.ToString();
                    autorizacionOsDet.PracticaAuthNr = codAuth;
                    autorizacionOsDet.PracticaIdEstado = "Autorizada";
                    autorizacionOsDet.PracticaDes = item.Descripcion;
                }
                autorizacionOs.AutorizacionOsDet.Add(autorizacionOsDet);
            }
            catch (Exception ex)
            {
                autorizacionOs.SetError(GetType().Name, GetMethod.ErrorLine(ex), ex.Message, ex.InnerException?.ToString() ?? "", model, string.Empty);
            }
            return autorizacionOs;
        }
    }
}
