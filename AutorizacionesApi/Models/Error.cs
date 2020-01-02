using System;

namespace WebApi.Models
{
    public class Error
    {
        public Error()
        {
            Dia = DateTime.Now;
           // Modulo = GetMethod.Current();
        }
        public string Clase { get; set; }
        public int ErrorLine { get; set; }
        public string Mensaje { get; set; }
        public string InnerException { get; set; }
        public object Model { get; set; }
        public string Modulo { get; set; }
        public DateTime Dia { get; }
        public string Query { get; set; }
        public bool OSError { get; set; }
    }
}
