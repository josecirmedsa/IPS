using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Repository.OS
{
    public static class HL7
    {
        public static string[] DecifraHL7(string MenHL7)
        {
            var HL7 = new string[100];
            var inicio = 0;
            var final = 0;
            var indice = 0;
            while (final != -1)
            {
                final = MenHL7.IndexOf("\n", inicio);
                if (final != -1)
                {
                    HL7[indice] = MenHL7.Substring(inicio, final - inicio);
                    inicio = final + 1;
                    indice++;
                }
                else
                {
                    HL7[indice] = MenHL7.Substring(inicio);
                    final = -1;
                }
            }
            return HL7;
        }

        public static string[] DecifraHL7Sancor(string MenHL7)
        {
            var HL7 = new string[100];
            var inicio = 0;
            var final = 0;
            var indice = 0;
            while (final != -1)
            {
                final = MenHL7.IndexOf("\r", inicio);
                if (final != -1)
                {
                    HL7[indice] = MenHL7.Substring(inicio, final - inicio);
                    inicio = final + 1;
                    indice++;
                }
                else
                {
                    HL7[indice] = MenHL7.Substring(inicio);
                    final = -1;
                }
            }
            return HL7;
        }

        public static string CampoHL7(string dato, int campo, int subindice)
        {
            var inicio = 0;
            int final;
            for (var indice = 0; indice < campo; indice++)
            {
                final = dato.IndexOf("|", inicio);
                inicio = final + 1;
            }
            final = dato.IndexOf("|", inicio);
            var texto = final != -1 ? dato.Substring(inicio, final - inicio) : dato.Substring(inicio);
            if (subindice <= 0) return texto;
            {
                inicio = 0;
                for (var indice = 1; indice < subindice; indice++)
                {
                    if (inicio == 0 && indice > 1)
                    {
                        return "";
                    }
                    final = texto.IndexOf("^", inicio);
                    inicio = final + 1;
                }
                final = texto.IndexOf("^", inicio);
                return final != -1 ? texto.Substring(inicio, final - inicio) : texto.Substring(inicio);
            }
        }

    }
}
