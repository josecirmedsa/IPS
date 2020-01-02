using WebApi.Models.Autorizacion;
using WebApi.Utils;

namespace WebApi.Models
{
    public class Errores
    {
        public Errores()
        {
            Error = new Error();
            ShowMessage = "";
            ModelError = false;
            HasError = false;
        }
        public bool HasError { get; set; }
        public string ShowMessage { get; set; }
        public Error Error { get; set; }

        public bool ModelError { get; set; }

        public void SetError(string clase, int errorLine, string mensaje, string innerException, object model, string query, bool sendMail = true)
        {
            HasError = true;
            Error.Clase = clase;
            Error.ErrorLine = errorLine;
            Error.Mensaje = mensaje;
            Error.InnerException = innerException;
            Error.Model = model;
            Error.Modulo = GetMethod.Current();
            Error.Query = query;
            if (sendMail)
            {
                SendMail.Send(Error);
            }

        }
    }
}
