using System.Collections.Generic;

namespace WebApi.Models.Autorizacion
{
    public class AuthListOutputList : Errores
    {
        public AuthListOutputList()
        {
            Error = new Error();
            List = new List<AuthListOutput>();
        }
        public List<AuthListOutput> List { get; set; }
    }
}
