using System.Collections.Generic;

namespace WebApi.Models.Autorizacion
{
    public class AutorizacionVer : Errores
    {
        public AutorizacionVer()
        {
            Detalle = new List<AutorizacionVerDet>();
        }
        public int Id { get; set; }
        public string AuthNr { get; set; }
        public string Fecha { get; set; }
        public string Afiliado { get; set; }
        public string Plan { get; set; }
        public string Iva { get; set; }
        public string IdentificacionNro { get; set; }
        public string Aseguradora { get; set; }
        public string Matricula { get; set; }
        public string Profesional { get; set; }
        public List<AutorizacionVerDet> Detalle { get; set; }
        public string AuthNrAnulacion { get; set; }
        public string Estado { get; set; }
    }
}
