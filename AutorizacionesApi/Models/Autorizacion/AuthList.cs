using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Autorizacion
{
    public class AuthList
    {
        [Required]
        public int PrestadorId { get; set; }
        [Required]
        public int OsId { get; set; }

        private string _desde;
        [Required]
        public string Desde
        {
            get
            {
                return Convert.ToDateTime(_desde).ToShortDateString();
            }
            set { _desde = value; }
        }

        private string _hasta;
        [Required]
        public string Hasta
        {
            get
            {
                return Convert.ToDateTime(_hasta).ToShortDateString();
            }
            set { _hasta = value; }
        }
    }
}
