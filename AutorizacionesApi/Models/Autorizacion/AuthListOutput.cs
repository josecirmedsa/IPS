using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Autorizacion
{
    public class AuthListOutput : Errores
    {
        public int Id { get; set; }
        public string AuthNr { get; set; }
        public string Fecha { get; set; }
        public string Matricula { get; set; }
        public string Afiliado { get; set; }
        public string AfiNr { get; set; }
        public string Prestacion { get; set; }
        public string Cant { get; set; }
        public string Estado { get; set; }
        public bool Presentado { get; set; }
    }
}
