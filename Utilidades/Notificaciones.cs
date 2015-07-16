using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using Prestlan.Models;

namespace Prestlan.Utilidades {
    public class Notificaciones {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        public static void enviarNotificacion(string tn, int mid, string titulo, string asunto, string nombreEmisor, string correoEmisor, string correoDestinatario, NameValueCollection Campos) {
            string cuerpo_mensaje = Utilidades.Correo.getFormulario(tn, Campos);
            bool estadoEnvio = Utilidades.Correo.SendCorreo(nombreEmisor, correoEmisor, correoDestinatario, "PRESTLAN: " + asunto, cuerpo_mensaje);
            var notificacion = new Prestlan.Models.Notificaciones();
            notificacion.asunto = asunto;
            notificacion.contenido = cuerpo_mensaje;
            notificacion.correoDestinatario = correoDestinatario;
            notificacion.correoEmisor = correoEmisor;
            notificacion.fechaNotificacion = DateTime.Now;
            notificacion.MovimientosDeDocumento_Id = mid;
            notificacion.envioCorreoExito = estadoEnvio;
            _db.Notificaciones.Add(notificacion);
            _db.SaveChanges();
        }

    }
}