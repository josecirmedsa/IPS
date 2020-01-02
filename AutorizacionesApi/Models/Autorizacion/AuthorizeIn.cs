using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Autorizacion
{
    public class AuthorizeIn
    {
        [Required(ErrorMessage = "La Obra Social es un campo Requerido")]
        [Range(0, 15, ErrorMessage = "La Obra Social esta fuera de Rango")]
        public int OSId { get; set; }

        [Required(ErrorMessage = "El Prestador es un campo requerido")]
        public string PrestadorId { get; set; }

        [Required(ErrorMessage = "El Facturador es un campo requerido")]
        public string FacturadorId { get; set; }

        [Required(ErrorMessage = "La credencial es un campo requerido")]
        public string Credencial { get; set; }

        [Required(ErrorMessage = "Se requiere al menos una prestación")]
        public List<Prestacion.Prestacion> Prestaciones { get; set; }

        public string Tipo { get; set; } //Usado Solo en Aca Salud M/B
    }
}
