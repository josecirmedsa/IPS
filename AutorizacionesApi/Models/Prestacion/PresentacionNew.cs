using System;

namespace WebApi.Models.Prestacion
{
    public class PresentacionNew : Errores
    {
        public int Id { get; set; }
        public int OsId { get; set; }
        public int CmsOsId { get; set; }
        private string _hasta;
        public string Hasta
        {
            get { return Convert.ToDateTime(_hasta).ToShortDateString(); }
            set { _hasta = value; }
        }
        private string _desde;
        public string Desde
        {
            get { return Convert.ToDateTime(_desde).ToShortDateString(); }
            set { _desde = value; }
        }

        public string Matricula { get; set; }
    }
}
