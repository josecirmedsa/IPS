using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Autorizacion
{
    public class Elegibilidad
    {
        [Required(ErrorMessage = "La Matricula es un campo requerido")]
        public string IdPre { get; set; }

        [Required(ErrorMessage = "La Credencial es un campo requerido")]
        public string Credencial { get; set; }

        [Required(ErrorMessage = "La Obra Social es un campo Requerido")]
        [Range(0, 15, ErrorMessage = "La Obra Social esta fuera de Rango")]
        public int OsId { get; set; }

        public string UserId { get; set; }
    }
}
