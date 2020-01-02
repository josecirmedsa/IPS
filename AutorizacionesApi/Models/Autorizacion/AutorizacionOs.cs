using System;
using System.Collections.Generic;

namespace WebApi.Models.Autorizacion
{
    public class AutorizacionOs : Errores
    {
        public AutorizacionOs()
        {
            Error = new Error();
            AutorizacionOsDet = new List<AutorizacionOsDet>();
        }

        public int NIdAutorizacion { get; set; }
        public string CnroAutorizacion { get; set; }
        public int Idpre { get; set; }
        public int Ncodosoc { get; set; }
        //public string NnroAfiliado { get; set; }
        private string _nnroAfiliado;

        public string NnroAfiliado
        {
            get { return _nnroAfiliado; }
            set { _nnroAfiliado = value.Substring(0, Math.Min(20, value.Length)); }
        }

        public string ResultadoAutorizacion { get; set; }

        private string _dfecAutorizacion;
        public string DfecAutorizacion
        {
            get
            {
                var index = _dfecAutorizacion.IndexOf(' ');
                return index > 0 ? _dfecAutorizacion.Substring(0, index).Trim() : _dfecAutorizacion.Trim();
            }
            set { _dfecAutorizacion = value; }
        }


        public int Nestado { get; set; }
        public int NidUsuario { get; set; }
        public string DfecEstado { get; set; }
        //public string CnomAfiliado { get; set; }

        private string _cnomAfiliado;

        public string CnomAfiliado
        {
            get { return _cnomAfiliado; }
            set { _cnomAfiliado = value.Replace("'", "´").Substring(0, Math.Min(50, value.Length)); }
        }

        //public string CdescripcionPlan { get; set; }
        private string _cdescripcionPlan;

        public string CdescripcionPlan
        {
            get { return _cdescripcionPlan; }
            set { _cdescripcionPlan = value.Substring(0, Math.Min(30, value.Length)); }
        }

        //public string CdescripcionIva { get; set; }
        private string _cdescripcionIva;

        public string CdescripcionIva
        {
            get { return _cdescripcionIva; }
            set { _cdescripcionIva = value.Substring(0, Math.Min(20, value.Length)); }
        }

        public string EstadoAutorizacion { get; set; }
        public string CcodinternoAutorizacion { get; set; }
        public string CcodAnulacion { get; set; }
        public string DfecAnulacion { get; set; }
        public int Idfacturador { get; set; }

        public List<AutorizacionOsDet> AutorizacionOsDet { get; set; }
        public string Mensaje { get; set; }

        public string Tipo { get; set; }
    }
}
