using System;
using System.IO;

namespace WebApi.Repository.OS
{
    public class ObrasSociales
    {
        private readonly string Path;
        public ObrasSociales()
        {
            Path = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin")) + "Logs" + @"/" + this.GetType().Name + @"/" + DateTime.Now.ToString("yyyyMMdd") + @"/";

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        public void logResult(string request, string response, string type)
        {
            try
            {
                File.WriteAllText(Path + type + DateTime.Now.Ticks + ".txt", request + Environment.NewLine + Environment.NewLine + Environment.NewLine + response);
            }
            catch (Exception ex)
            {
                ///////////////////////////////////////////////////////////////////
                ////////////////////////Log de Archivo/////////////////////////////
                /// Si hay error al escribir no hay problema seguir con el cod ////
                //////////////////////////////////////////////////////////////////
            }
        }
    }
}
