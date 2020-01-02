using System.Collections.Generic;

namespace WebApi.Models.Autorizacion
{
    public class Authorize
    {

        public Authorize(int OSId, string PrestadorId, string FacturadorId, string Credencial, string Tipo, List<Prestacion.Prestacion> Prestaciones, string UserId)
        {
            this.OSId = OSId;
            this.PrestadorId = PrestadorId;
            this.FacturadorId = FacturadorId;
            this.Credencial = Credencial;
            this.Tipo = Tipo;
            this.Prestaciones = Prestaciones;
            this.UserId = UserId;
        }


        public int OSId { get; set; }
        public string OsNombre { get; set; }

        public string Prestador { get; set; }
        public string PrestadorId { get; set; }
        public string FacturadorId { get; set; }
        public Efector Efector { get; set; }
        public string UserId { get; set; }

        private string _afiliadoNombre;
        public string AfiliadoNombre
        {
            get
            {
                var index = _afiliadoNombre.IndexOf('(');
                return index > 0 ? _afiliadoNombre.Substring(0, index).Trim() : _afiliadoNombre.Trim();
            }
            set { _afiliadoNombre = value; }
        }

        public string AfiliadoPlan { get; set; }
        public string Credencial { get; set; }

        public string Tipo { get; set; }
        public List<Prestacion.Prestacion> Prestaciones { get; set; }
    }
}
