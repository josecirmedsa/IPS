using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Autorizacion
{
    public class AutorizacionOsDet
    {
        public string PracticaId { get; set; }
        private string _practicaDes;

        public string PracticaDes
        {
            get { return _practicaDes; }
            set { _practicaDes = value.Substring(0, Math.Min(30, value.Length)); }
        }

        private string _practicaIdEstado;
        public string PracticaIdEstado
        {
            get { return _practicaIdEstado; }
            set { _practicaIdEstado = value.Substring(0, Math.Min(10, value.Length)); }
        }

        public string PracticaCantAprob { get; set; }
        private string _practicaDetAuth;
        public string PracticaDetAuth
        {
            get { return _practicaDetAuth; }
            set { _practicaDetAuth = value.Substring(0, Math.Min(75, value.Length)); }
        }

        public string PracticaAuthNr { get; set; }
    }
}
