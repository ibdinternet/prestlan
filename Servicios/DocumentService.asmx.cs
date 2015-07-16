using IBD.Web;
using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;


namespace Prestlan {


    /// <summary>
    /// Descripción breve de DocumentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class DocumentService : System.Web.Services.WebService {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        //[WebMethod]
        //public ListDocumento GetDocumentoById_Old(int did) {
        //    var documento = (from d in _db.Documento
        //                     where d.Id == did
        //                     select new ListDocumento {
        //                         Id = did.ToString(),
        //                         Titulo = d.Titulo,
        //                         Autor = d.Usuario.nombre,
        //                         Descripcion = d.Descripcion,
        //                         Estado = d.TipoEstado.descripcion
        //                     }).FirstOrDefault();
        //    return documento;
        //}

        [WebMethod(EnableSession=true)]
        public ListDocumento GetDocumentoById(int did) {
            var documento = (from d in _db.Documento
                             where d.Id == did
                             select d)
                             .AsEnumerable()
                             .Select (x => new ListDocumento() {
                                 Id = did.ToString(),
                                 Titulo = x.Titulo,
                                 Autor = x.Usuario.nombre,
                                 Descripcion = x.Descripcion,
                                 Estado_Id = x.Estado_Id,
                                 Etapa_Id = x.Etapa_Id,
                                 Estado = x.TipoEstado.descripcion,
                                 EstadoDescripcion = "",
                                 Etapa = x.TipoEtapa.descripcion,
                                 EtapaDescripcion = "",
                             }).FirstOrDefault();

            documento.EstadoDescripcion = Traduce.getTG(documento.Estado);
            documento.EtapaDescripcion = Traduce.getTG(documento.Etapa);
            documento.PuedeValidar = Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.ValidacionesEditar);
            return documento;
        }

        [WebMethod(EnableSession = true)]
        public string eliminarBorrador(int did) {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación
            return Utilidades.OperacionesDocumentos.eliminarBorrador(uid, did);
        }

        [WebMethod(EnableSession = true)]
        public string pasarACaducado(int did) {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación
            return Utilidades.OperacionesDocumentos.pasarACaducado(uid, did);
        }

        [WebMethod(EnableSession = true)]
        public string pasarABorrador(int did) {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación
            return Utilidades.OperacionesDocumentos.pasarABorrador(uid, did);
        }

        [WebMethod(EnableSession = true)]
        public string pasarARechazado(int did) {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación
            return Utilidades.OperacionesDocumentos.pasarARechazado(uid, did);
        }

        [WebMethod(EnableSession = true)]
        public string pasarAPublicado(int did) {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación            
            return Utilidades.OperacionesDocumentos.pasarAPublicado(uid, did);
            
        }

        [WebMethod(EnableSession = true)]
        public string pasarAPublicadoDesdeListaDocumentos(int did)
        {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación            
            if (Utilidades.OperacionesDocumentos.pasarARevision(uid, did) == "OK")
            {
                return Utilidades.OperacionesDocumentos.pasarAPublicado(uid, did);
            }           
            else
            {
                return "ERROR";
            }
        }

        [WebMethod(EnableSession = true)]
        public string pasarARevision(int did) {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación
            return Utilidades.OperacionesDocumentos.pasarARevision(uid, did);
        }

        [WebMethod(EnableSession = true)]
        public EstadoRetorno obtenerNombreUsuarioRevisor(int did) {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación
            return Utilidades.OperacionesDocumentos.obtenerNombreUsuarioRevisor(uid, did);
        }

        [WebMethod(EnableSession = true)]
        public bool sendReclamacion(int pid, string contenido) {
            int uid = Utilidades.Autentificacion.GetUsuarioID(); // uid del usuario que intenta la operación
            if (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.ReclamacionesEditar)) {
                // Se notifica a la empresa a la que pertenece el trabajador propietario del documento
                string correoNotificacion = _db.Propietario.Find(pid).Empresa.CorreosNotificaciones;
                string nombreUsuario = Utilidades.Autentificacion.GetNombre(uid);
                string emailUsuario = Utilidades.Autentificacion.GetUsuarioEmail(uid);
                string asunto = Traduce.getTG("hayreclamacion");
                NameValueCollection Campos = new NameValueCollection();
                Campos.Add("USUARIO", nombreUsuario);
                Campos.Add("CUERPO", contenido);
                string cuerpo_mensaje = Utilidades.Correo.getFormulario("Reclamacion", Campos);
                bool estado = Utilidades.Correo.SendCorreo(nombreUsuario, emailUsuario, correoNotificacion, asunto, cuerpo_mensaje);

                anotarReclamación(emailUsuario, correoNotificacion, asunto, cuerpo_mensaje, estado);
                return true;
            } else {
                return false;
            }
        }
        
        private void anotarReclamación(string correoEmisor, string correoDestinatario, string asunto, string contenido, bool envioCorreoExito) {
            using (_db) {
                var reclamacion = new Reclamaciones();
                reclamacion.asunto = asunto;
                reclamacion.contenido = contenido;
                reclamacion.correoDestinatario = correoDestinatario;
                reclamacion.correoEmisor = correoEmisor;
                reclamacion.envioCorreoExito = envioCorreoExito;
                reclamacion.fechaReclamacion = DateTime.Now;
                _db.Reclamaciones.Add(reclamacion);
                _db.SaveChanges();
            }
        }

    }
}
