namespace WebApi.Models.Autorizacion
{
    public class Efector : Errores
    {
        public string Name { get; set; }
        public string Cuit { get; set; }
        public int Matricula { get; set; }

        public string Email { get; set; }
    }
}
