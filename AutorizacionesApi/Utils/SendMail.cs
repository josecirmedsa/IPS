using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using WebApi.Models;
using WebApi.Models.Autorizacion;

namespace WebApi.Utils
{
    public static class SendMail
    {
        public static void Send(Error error)
            {
                var Body = "";
                // set body-message and encoding
                Body = "<p>Clase: <b>" + error.Clase + "</b></p>";
                Body += "<p>Modulo: <b>" + error.Modulo + "</b></p>";
                Body += "<p>Error Line: <b> " + error.ErrorLine + " </b></p>";
                Body += "<p>Mensaje: <b> " + error.Mensaje + " </b></p>";
                Body += "<p>Inner Exception: <b> " + error.InnerException + " </b></p>";
                Body += "<p>Dia: <b> " + error.Dia + " </b></p>";
                Body += "<p> Model: <b> " + JsonConvert.SerializeObject(error.Model) + " </b></p>";
                Body += "<p> Query: <b> " + error.Query + " </b></p>";

                Send(Body);
            }



            public static void SendMailAutirizacion(AutorizacionVer autorizacion, string email)
            {
                var Body = "";

                var estado = "";
                switch (autorizacion.Estado)
                {
                    case "Autorizada":
                        estado = "green";
                        break;
                    case "Rechazada":
                        estado = "red";
                        break;
                    case "Autorizada Parcial":
                        estado = "orange";
                        break;
                    default:
                        estado = "black";
                        break;
                }


                    Body += "<table style='width:100%'>";
                    Body += "    <tr>";
                    Body += "        <td colspan='2' style='padding-left: 2%;'>";
                    Body += "		    <img src='https://cirmedsa.com.ar/images/logo.png' style='height: 100px;width: 100px;'>";
                    Body += "        </td>";
                    Body += "        <td colspan='8'>";
                    Body += "            <h3 style='font-size: 34px; line-height: 1.11111111; letter-spacing: -1px; font-family: Raleway,sans-serif;font-weight: 900;margin: 0;text-transform: uppercase;color: #039;text-align: center;'>Circulo Medico Salta</h3>";
                    Body += "            <h6 style='font-size: 20px;line-height: 1.25;color: #21c2f8;font-weight: 700;text-transform: none;margin: 0;text-align: center;'>Asoc.civil sin fines de Lucro</h6>";
                    Body += "        </td>";
                    Body += "    </tr>";
                    Body += "    <tr>";
                    Body += "        <td colspan='12' style='padding-bottom: 10px;text-align: center;height:35px; font-size: 20px'>";
                    Body += "            <h7 style='padding-bottom: 10px;text-align: center;height:35px'>Comprobante de Autorización ON-LINE</h7>";
                    Body += "        </td>";
                    Body += "    </tr>";
                    Body += "    <tr>";
                    Body += "        <td colspan='4'><b>Autorización Nº:</b> <i> " + autorizacion.AuthNr + "</i></td>";
                    Body += "        <td colspan='4'><b>Fecha:</b> " + autorizacion.Fecha + "</td>";
                    Body += "        <td colspan='4'><b>Estado:</b> <span style='font-size:1.5em;color:" + estado + " '> " + autorizacion.Estado + "</span></td>";
                    Body += "    </tr>";
                    Body += "    <tr>";
                    Body += "        <td colspan='6'><b>Afiliado:</b> " + autorizacion.Afiliado + "</td>";
                    Body += "        <td colspan='3'><b>Plan:</b> " + autorizacion.Plan + "</td>";
                    Body += "        <td colspan='3'><b>Cond.IVA:</b> " + autorizacion.Iva + "</td>";
                    Body += "    </tr>";
                    Body += "    <tr>";
                    Body += "        <td colspan='6'><b>Nº Identificacion:</b> " + autorizacion.IdentificacionNro + "</td>";
                    Body += "        <td colspan='6'><b>Aseguradora:</b> " + autorizacion.Aseguradora + "</td>";
                    Body += "    </tr>";
                    Body += "    <tr>";
                    Body += "        <td colspan='6'><b>Profesional:</b> " + autorizacion.Profesional + "</td>";
                    Body += "        <td colspan='6'><b>Matricula:</b> " + autorizacion.Matricula + "</td>";
                    Body += "    </tr>";
                    Body += "    <tr>";
                    Body += "        <td colspan='12' style='text-align:center;margin-bottom: 10px; font-size: 18px'>";
                    Body += "            <b>Prestaciones</b>";
                    Body += "        </td>";
                    Body += "    </tr>";
                    Body += "    <tr>";
                    Body += "        <td colspan='5'><b>Prestación</b></td>";
                    Body += "        <td colspan='1'><b>Cant.</b></td>";
                    Body += "        <td colspan='2'><b>Estado</b></td>";
                    Body += "        <td colspan='4'><b>Observación</b></td>";
                    Body += "    </tr>";
                    foreach (var item in autorizacion.Detalle)
                    {
                        Body += "    <tr>";
                        Body += "        <td colspan='5'> " + item.Prestacion + "</td>";
                        Body += "        <td colspan='1'> " + item.Cantidad + "</td>";
                        Body += "        <td colspan='2'> " + item.Estado + " </td>";
                        Body += "        <td colspan='4'> " + item.Observacion + "</td>";
                        Body += "    </tr>";
                    }
                    Body += "</table>";

                Send(Body);
            }

            internal static void SendTest()
            {
                var Body = "";

                // set body-message and encoding
                Body = "<p>Clase: <b>" + "Prueba" + "</b></p>";
                Body += "<p>Modulo: <b>" + "Prueba" + "</b></p>";
                Body += "<p>Error Line: <b> " + "Prueba" + " </b></p>";
                Body += "<p>Mensaje: <b> " + "Prueba" + " </b></p>";
                Body += "<p>Inner Exception: <b> " + "Prueba" + " </b></p>";
                Body += "<p>Dia: <b> " + "Prueba" + " </b></p>";
                Body += "<p> Model: <b> " + "Prueba" + " </b></p>";
                Body += "<p> Query: <b> " + "Prueba" + " </b></p>";

                Send(Body);
            }

        private static void Send(string Body)
        {
            string _server = "10.5.5.15";
            string _entorno = "Local";
            string _usr = "informacion";
            string _psw = "infor2011";
            string Path = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin")) + @"/SendMailError/" + DateTime.Now.ToString("yyyyMMdd") + @"/";
            string email = "jsalvador@cirmedsa.com.ar";
            try
            {
                var server = _server;
                var entorno = _entorno;

                var mySmtpClient = new SmtpClient(server)
                {
                    UseDefaultCredentials = false,
                    Port = 587

                };
                NetworkCredential basicAuthenticationInfo;

                var usr = _usr;
                var psw = _psw;
                basicAuthenticationInfo = new NetworkCredential(usr, psw);

                // set smtp-client with basicAuthentication
                mySmtpClient.Credentials = basicAuthenticationInfo;

                // add from,to mailaddresses
                var from = new MailAddress(email);

                var to = new MailAddress(email);

                var myMail = new MailMessage(from, to);

                // add ReplyTo
                var replyto = new MailAddress(email);
                myMail.ReplyToList.Add(replyto);

                //#if DEBUG
                //                var Bcc = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["otherMails"]);
                //                myMail.Bcc.Add(Bcc);
                //#endif

                // set subject and encoding
                myMail.Subject = "Server Error - " + entorno;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                myMail.Body = Body;
              
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
              
                // text or html
                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
            }

            catch (SmtpException ex)
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                File.WriteAllText(Path + DateTime.Now.Ticks + ".txt", ex.Message);
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                File.WriteAllText(Path + DateTime.Now.Ticks + ".txt", ex.Message);
            }
        }
    }
}
