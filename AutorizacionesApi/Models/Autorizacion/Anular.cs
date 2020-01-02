using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Autorizacion
{
    public class Anular : Errores
    {
        [Required]
        public int OSId;
        [Required]
        public int AuthId { get; set; }
        [Required]
        public string Matricula { get; set; }
        [Required]
        public string Credencial { get; set; }

        public string Cuit { get; set; }
    }
}