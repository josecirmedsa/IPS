using System;

namespace WebApi.Models.Autorizacion
{
    public class OsStatus
    {
        ///Todo Revisar Esto!

        public static DateTime? Medife { get; set; }
        private static DateTime? Swiss { get; set; }

        public static DateTime? TimeOut { get; set; }

        private static DateTime? TimeOutLast { get; set; }

        public static bool TimeOutSendMail
        {
            get
            {

                var sendMail = true;
                if (!TimeOutLast.HasValue)
                {
                    if (TimeOut.HasValue)
                    {
                        TimeOutLast = TimeOut;
                    }
                }
                else
                {
                    if (TimeOut.HasValue)
                    {
                        TimeSpan span = DateTime.Now - (TimeOutLast ?? DateTime.Now);
                        if (span.TotalSeconds > 5 && span.TotalSeconds < 300)
                        {
                            sendMail = false;
                        }
                        if (span.TotalSeconds > 300)
                        {
                            TimeOutLast = DateTime.Now;
                        }
                    }
                    else
                    {
                        TimeOutLast = null;
                    }
                }
                return sendMail;
            }

        }


        public static bool checkSwiss(bool hasError)
        {
            //Devuelve true en caso que tenga que mandar el mail
            //false en caso que no tenga que mandar el mail
            if (hasError)
            {
                if (!Swiss.HasValue)
                {
                    Swiss = DateTime.Now;
                    return true;
                }
                else
                {
                    TimeSpan span = DateTime.Now - (Swiss ?? DateTime.Now);
                    if (span.TotalSeconds > 300)
                    {
                        Swiss = null;
                    }
                    return false;
                }
            }
            else
            {
                Swiss = null;
                return false;
            }

        }
    }
}
