using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Prestlan.Utilidades {
    public class Correo {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string getFormulario(string form, NameValueCollection Campos) {            
            string file = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PlantillasCorreo"].ToString() + "\\" + "Notificacion_" + form + ".html");
            StreamReader sr = new StreamReader(file);
            string data = sr.ReadToEnd();
            sr.Close();
            foreach (string el in Campos.AllKeys) data = data.Replace("##" + el + "##", Campos[el]);
            return data;
        }
        /// <summary>
        /// Envío de correo mediante formulario
        /// </summary>
        /// <param name="nombreFrom"></param>
        /// <param name="msgFrom"></param>
        /// <param name="msgTo"></param>
        /// <param name="msgSubject"></param>
        /// <param name="msgBody"></param>
        public static bool SendCorreo(string nombreFrom, string msgFrom, string msgTo, string msgSubject, string msgBody) {
            bool estado = true;
            log.Debug("Entrando en SendCorreo(): msgFrom: " + msgFrom + ", msgTo: " + msgTo + ", msgSubject: " + msgSubject);

            SmtpClient client = new SmtpClient();
            MailAddress from = new MailAddress(msgFrom, nombreFrom, Encoding.UTF8);
            MailAddress to = new MailAddress(msgTo);
            MailMessage message = new MailMessage(from, to);
            message.IsBodyHtml = true;
            message.Body = msgBody;
            // message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = msgSubject;
            // message.SubjectEncoding = System.Text.Encoding.UTF8;
            //  message.Bcc.Add(bcc);
            try {
                client.Send(message);
            } catch (SmtpException smtpExc) {
                estado = false;
                log.Debug("Error enviando correo: " + smtpExc.ToString());
            } finally {
                message.Dispose();
            }
            log.Debug("Saliendo de SendCorreo().");
            return estado;
            
        }
    }
}