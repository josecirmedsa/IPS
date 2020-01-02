using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Autorizacion
{
    public class Search
    {
        [Required]
        [Range(0, 10)]
        public string OsId { get; set; }
        //[Required]
        public string Cod { get; set; }
        //[Required]
        public string Descripcion { get; set; }
        public int PrestadorId { get; set; }
    }
}
