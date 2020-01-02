using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public static class Mensajes
    {
        static Dictionary<string, string> mensaje = new Dictionary<string, string>(){
            {"AfiIne","Afiliado inexistente o inhabilitado"},
            {"ServidorNoResponde","El servidor de la Obra social no responde. Imposible realizar la operación. Inténtelo nuevamente en 5 minutos"},
            {"AnulFail", "No se pudo realizar la anulación!" }

                                                                    };

        public static string Get(string key)
        {
            return mensaje[key];
        }

    }
}
