using System;
using WebApi.Utils;
using wsTraditum;

namespace WebApi.Repository.OS
{
    public class OsTraditum
    {
        public string Send(string mensaje)
        {
            const string user = "IA007160";
            const string pass = "IA007160";
            const string msgTipo = "SI";

            var service = new WebService_IASoapClient(WebService_IASoapClient.EndpointConfiguration.WebService_IASoap);

            try
            {
                // Instanciacion del proveedor de encriptacion
                var woCrypDES = new TripleDESUtil("1234567890123456ABCDEFGH");
                // Encriptacion de parametros y conversion de Byte a String codificada en Base64.
                var wtMessage = Convert.ToBase64String(woCrypDES.Encrypt(mensaje));
                var wtUser = Convert.ToBase64String(woCrypDES.Encrypt(user));
                var wtPwd = Convert.ToBase64String(woCrypDES.Encrypt(pass));
                var wtMsgType = Convert.ToBase64String(woCrypDES.Encrypt(msgTipo));
                // Procesamiento del mensaje mediante el servicio
                EnviarResponse a = service.EnviarAsync(wtMessage, wtUser, wtPwd, wtMsgType).Result;
                var b = a.Body.EnviarResult;
                return b;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
