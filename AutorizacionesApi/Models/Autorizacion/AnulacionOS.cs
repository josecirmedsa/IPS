namespace WebApi.Models.Autorizacion
{
    public class AnulacionOS : Errores
    {
        public AnulacionOS()
        {
            this.Error = new Error();
        }

        public string IdAuth { get; set; }
        public string Estado { get; set; }
        public string Nestado { get; set; }
        public string Fecha { get; set; }
        public string CodAnulacion { get; set; }
    }
}