using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Prestlan.handlers {
    /// <summary>
    /// Abre documentos comprobrando seguridad y existencia del id de fichero indicado
    /// </summary>
    public class opendoc : IHttpHandler, System.Web.SessionState.IRequiresSessionState {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        public void ProcessRequest(HttpContext context) {
            if (context.Request.QueryString["id"] != null) {
                int Fichero_id = Convert.ToInt32(context.Request.QueryString["id"].ToString());
                if (PermisosOK(Fichero_id)) {
                    var fichero = _db.Fichero.Find(Fichero_id);
                    string path = context.Server.MapPath(ConfigurationManager.AppSettings["RepositorioDocumentos"].ToString() + "\\" + fichero.nombreFS.Substring(0, 2) + "\\" + fichero.nombreFS);
                    if (File.Exists(path)) {
                        using (FileStream fs = File.OpenRead(path)) {
                            string contentType = "";
                            string ext = Path.GetExtension(path);
                            switch (ext) {
                                case ".doc":
                                    contentType = "application/msword";
                                    break;
                                case ".docx":
                                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                                    break;
                                case ".gif":
                                    contentType = "image/gif";
                                    break;
                                case ".png":
                                    contentType = "image/png";
                                    break;
                                case ".jpg":
                                    contentType = "image/jpeg";
                                    break;
                                case ".jpeg":
                                    contentType = "image/jpeg";
                                    break;
                                case ".pdf":
                                    contentType = "application/pdf";
                                    break;
                                default:
                                    contentType = "text/plain";
                                    break;
                            }
                            context.Response.ContentType = contentType;
                            CopyStream(fs, context.Response.OutputStream);
                        }
                    } else {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("No se ha encontrado el archivo: " + fichero.nombre);
                    }
                } else {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("No está autorizado");
                }
            } else {
                context.Response.ContentType = "text/plain";
                context.Response.Write("No se ha especificado un archivo");
            }
        }

        public bool IsReusable {
            get {
                return false;
            }
        }

        private static void CopyStream(Stream input, Stream output) {
            byte[] buffer = new byte[32768];
            while (true) {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0) return;
                output.Write(buffer, 0, read);
            }
        }

        private bool PermisosOK(int Fichero_Id)
        {
            int usuarioId = Utilidades.Autentificacion.GetUsuarioID();
            if (Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.FicherosDescargarTodo))
            {
                return true;
            }
            else if (Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.FicherosDescargarSoloEmpresa))
            {
                var user = _db.Usuario.FirstOrDefault(x => x.Id == usuarioId);
                return _db.Documento.Where(x => x.Fichero_Id == Fichero_Id).SelectMany(x => x.Propietario).Any(x => x.Empresa_Id == user.Empresa_Id);
            }
            else
            {
                return false;
            }
        }
    }
}