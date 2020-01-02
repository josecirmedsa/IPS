namespace WebApi.Models.Prestacion
{
    public class Prestacion
    {
        private string _CodPres;

        public string CodPres
        {
            get { return _CodPres.Substring(0, 6); }
            set { _CodPres = value; }
        }

        public int Cant { get; set; }/**/
        public string CodigoEsp { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public string TipoNome { get; set; }
    }
}
