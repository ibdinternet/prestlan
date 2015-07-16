using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prestlan.Models;
using System.Collections.Specialized;
using Prestlan.Utilidades;
using System.Data.Entity;

namespace Prestlan.Utilidades {
    public class OperacionesDocumentos {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        public static string pasarACaducado(int uid, int did) {
            //Ha esta funcion se le llama al cargar el listado para que caduque 'automaticamente' los documentos con fecha caducada
            //Por lo que solo es necesario tener el permiso de ver lista de documentos, reclamaciones o validaciones
            if (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarTodo) ||
                (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarSoloEmpresa) && Utilidades.Autentificacion.EsDocumentoEmpresaUsuario(did))||
                Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.ReclamacionesListar) ||
                Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.ValidacionesListar))
            {
                // Obtener documento
                var documento = _db.Documento.Find(did);
                string tipoDoc = _db.TipoDocumento_translation.FirstOrDefault(x => x.Idioma_codigo == "es" && x.Tipodocumento_Id == documento.TipoDocumento_Id).Descripcion;
                var propietario = documento.Propietario.FirstOrDefault();
                string titulo = tipoDoc + " - " + (propietario.Trabajador != null ? propietario.Trabajador.Nombre : propietario.Empresa.Nombre);

                // Detección de errores
                if (documento == null) return Error.getError(TiposDeError.DOCUMENTO_NO_EXISTE);
                if (documento.Estado_Id == (int)TiposDeEstado.CADUCADO) return Error.getError(TiposDeError.DOCUMENTO_YA_CADUCADO);
                if (!documento.FechaCaducidad.HasValue) return Error.getError(TiposDeError.DOCUMENTO_SIN_FECHA_CADUCIDAD);

                if ((DateTime.Now - documento.FechaCaducidad.Value).TotalDays >= 1)
                {
                    documento.Etapa_Id = (int)TiposDeEtapa.CADUCAR;
                    documento.Estado_Id = (int)TiposDeEstado.CADUCADO;
                    _db.SaveChanges();

                    // Anotar movimiento
                    int movimiento_Id = anotarMovimiento(uid, did, documento.Etapa_Id, documento.Estado_Id, "Paso a caducado");

                    // Emitir notificaciones
                    string nombreEmisor = Utilidades.Autentificacion.GetNombre(uid);
                    NameValueCollection Campos = new NameValueCollection();
                    Campos.Add("USUARIOEMISOR", nombreEmisor);
                    Campos.Add("TITULODOCUMENTO", titulo);

                    Utilidades.Notificaciones.enviarNotificacion(
                        TiposDeNotificaciones.Caducado.ToString(),
                        movimiento_Id,
                        documento.Titulo,
                        "Documento caducado",
                        nombreEmisor,
                        Utilidades.Autentificacion.GetUsuarioEmail(uid),
                        Utilidades.Autentificacion.GetUsuarioEmail(documento.Usuario_Id), Campos);
                    return "OK";
                }
                else
                {
                    return Error.getError(TiposDeError.DOCUMENTO_AUN_NO_CADUCADO); // ¡El documento no ha caducado todavía! (¿Podríamos forzar el caducado de documentos?)
                }
            }
            else
            {
                return Error.getError(TiposDeError.USUARIO_NO_AUTORIZADO);
            } 
        }

        public static string pasarARechazado(int uid, int did) {
            // Comprobar permisos
            if (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarTodo) ||
                (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarSoloEmpresa) && Utilidades.Autentificacion.EsDocumentoEmpresaUsuario(did)))
            {
                // Obtener documento
                var documento = _db.Documento.Find(did);
                string tipoDoc = _db.TipoDocumento_translation.FirstOrDefault(x => x.Idioma_codigo == "es" && x.Tipodocumento_Id == documento.TipoDocumento_Id).Descripcion;
                var propietario = documento.Propietario.FirstOrDefault();
                string titulo = tipoDoc + " - " + (propietario.Trabajador != null ? propietario.Trabajador.Nombre : propietario.Empresa.Nombre);

                // Detección de errores
                if (documento == null) return Error.getError(TiposDeError.DOCUMENTO_NO_EXISTE);
                if (documento.Etapa_Id != (int)TiposDeEtapa.REVISION) return Error.getError(TiposDeError.DOCUMENTO_NO_EN_REVISION);

                // Cambiar etapa
                documento.Etapa_Id = (int)TiposDeEtapa.RECHAZO;
                documento.Estado_Id = (int)TiposDeEstado.BORRADOR;
                _db.SaveChanges();

                // Anotar movimiento
                int movimiento_Id = anotarMovimiento(uid, did, documento.Etapa_Id, documento.Estado_Id, "Paso a rechazado");

                // Emitir notificaciones
                string nombreEmisor = Utilidades.Autentificacion.GetNombre(uid);
                NameValueCollection Campos = new NameValueCollection();
                Campos.Add("USUARIOEMISOR", nombreEmisor);
                Campos.Add("TITULODOCUMENTO", titulo);

                Utilidades.Notificaciones.enviarNotificacion(
                    TiposDeNotificaciones.Rechazado.ToString(),
                    movimiento_Id,
                    documento.Titulo,
                    "Documento rechazado",
                    nombreEmisor,
                    Utilidades.Autentificacion.GetUsuarioEmail(uid),
                    Utilidades.Autentificacion.GetUsuarioEmail(documento.Usuario_Id), Campos);
                return "OK";
            }    
            else
            {
                return Error.getError(TiposDeError.USUARIO_NO_AUTORIZADO);
            }              
        }

        public static string pasarARevision(int uid, int did) {
            if (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarTodo) ||
                (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarSoloEmpresa) && Utilidades.Autentificacion.EsDocumentoEmpresaUsuario(did)))
            {
                // Obtener documento
                var documento = _db.Documento.Find(did);
                string tipoDoc = _db.TipoDocumento_translation.FirstOrDefault(x => x.Idioma_codigo == "es" && x.Tipodocumento_Id == documento.TipoDocumento_Id).Descripcion;
                var propietario = documento.Propietario.FirstOrDefault();
                string titulo = tipoDoc + " - " + (propietario.Trabajador != null ? propietario.Trabajador.Nombre : propietario.Empresa.Nombre);
                //var propietario documento.Propietario.FirstOrDefault();
                // Detección de errores
                if (documento == null) return Error.getError(TiposDeError.DOCUMENTO_NO_EXISTE);
                if (documento.Etapa_Id != (int)TiposDeEtapa.BORRADOR && documento.Etapa_Id != (int)TiposDeEtapa.RECHAZO) return Error.getError(TiposDeError.DOCUMENTO_NO_ES_BORRADOR);

                // Cambiar etapa
                documento.Etapa_Id = (int)TiposDeEtapa.REVISION;
                _db.SaveChanges();

                // Anotar movimiento                            
                int movimiento_Id = anotarMovimiento(uid, did, documento.Etapa_Id, documento.Estado_Id, "Paso a validación");

                // Emitir notificaciones
                // Notificar a validador(es)
                // Recopilar todos los usuarios del tipo validador y notificar
                var uv = (from u in _db.Usuario
                          where u.Rol.Id == (int)TiposDeRoles.VALIDADOR
                          select u);
                foreach (var u in uv)
                {
                    string nombreEmisor = Utilidades.Autentificacion.GetNombre(uid);
                    NameValueCollection Campos = new NameValueCollection();
                    Campos.Add("USUARIOEMISOR", nombreEmisor);
                    //Campos.Add("TITULODOCUMENTO", documento.Titulo);
                    Campos.Add("TITULODOCUMENTO", titulo);
                    Utilidades.Notificaciones.enviarNotificacion(
                        TiposDeNotificaciones.PasoARevision.ToString(),
                        movimiento_Id,
                        documento.Titulo,
                        "Documento pendiente de validar",
                        nombreEmisor,
                        Utilidades.Autentificacion.GetUsuarioEmail(uid),
                        u.email, Campos);
                }
                return "OK";
            }
            else
            {
                return Error.getError(TiposDeError.USUARIO_NO_AUTORIZADO);
            }
        }

        public static string pasarAPublicado(int uid, int did) {
            if (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarTodo) ||
                (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarSoloEmpresa) && Utilidades.Autentificacion.EsDocumentoEmpresaUsuario(did)))
            {
                // Obtener documento
                var documento = _db.Documento.Find(did);

                // Detección de errores
                if (documento == null) return Error.getError(TiposDeError.DOCUMENTO_NO_EXISTE);
                if (documento.Etapa_Id != (int)TiposDeEtapa.REVISION) return Error.getError(TiposDeError.DOCUMENTO_NO_EN_REVISION);

                // Cambiar etapa
                documento.Etapa_Id = (int)TiposDeEtapa.PUBLICACION;
                documento.Estado_Id = (int)TiposDeEstado.PUBLICADO;

                // Actualizamos la versión del documento de todos los documentos anteriores               
                // Asignamos el nuevo ID a todos los documentos de la cadena
                var docs = (from d in _db.Documento
                            where d.DocumentoVersion_Id == documento.Id || d.DocumentoVersion_Id == documento.DocumentoVersion_Id
                            orderby d.Id
                            select
                            d).ToList();
                foreach (var item in docs)
                {
                    item.DocumentoVersion_Id = documento.Id;
                }

                _db.SaveChanges();

                // Anotar movimiento
                int movimiento_Id = anotarMovimiento(uid, did, documento.Etapa_Id, documento.Estado_Id, "Paso a publicado");
                return "OK";
            }
            else
            {
                return Error.getError(TiposDeError.USUARIO_NO_AUTORIZADO);
            }
        }

        /// <summary>
        /// Pasa al estado de borrador
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="did"></param>
        /// <returns></returns>
        public static string pasarABorrador(int uid, int did) {
            if (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarTodo) ||
                (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarSoloEmpresa) && Utilidades.Autentificacion.EsDocumentoEmpresaUsuario(did)))
            {
                // Obtener documento
                var documento = _db.Documento.Find(did);

                // Detección de errores
                if (documento == null) return Error.getError(TiposDeError.DOCUMENTO_NO_EXISTE);
                if (documento.Etapa_Id != (int)TiposDeEtapa.PUBLICACION &&
                    documento.Etapa_Id != (int)TiposDeEtapa.REVISION) return Error.getError(TiposDeError.DOCUMENTO_NO_PUBLICADO);

                // Cambiar etapa
                documento.Etapa_Id = (int)TiposDeEtapa.BORRADOR;
                documento.Estado_Id = (int)TiposDeEstado.BORRADOR;
                _db.SaveChanges();

                // Anotar movimiento
                int movimiento_Id = anotarMovimiento(uid, did, documento.Etapa_Id, documento.Estado_Id, "Paso a borrador");
                return "OK";
            }
            else
            {
                return Error.getError(TiposDeError.USUARIO_NO_AUTORIZADO);
            }
        }

        /// <summary>
        /// Elimina un documento que está en estado de BORRADOR
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="did"></param>
        /// <returns></returns>
        public static string eliminarBorrador(int uid, int did) {
            if (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarTodo) ||
                (Utilidades.Autentificacion.comprobarPermiso(uid, TipoPermiso.DocumentosEditarSoloEmpresa) && Utilidades.Autentificacion.EsDocumentoEmpresaUsuario(did)))
            {
                // Obtener documento
                var documento = _db.Documento.Find(did);

                // Detección de errores
                if (documento == null) return Error.getError(TiposDeError.DOCUMENTO_NO_EXISTE);
                if (documento.Estado_Id != (int)TiposDeEstado.BORRADOR) return Error.getError(TiposDeError.DOCUMENTO_NO_ES_BORRADOR);

                // Eliminar
                documento.Eliminado = true;
                documento.FechaActualizado = DateTime.Now;

                // Anotar movimiento
                int movimiento_Id = anotarMovimiento(uid, did, documento.Etapa_Id, documento.Estado_Id, "Borrador eliminado");
                // ¿Debemos comprobar si hay alguna otra versión del documento?, y si no es así ¿hay que borrar el fichero asociado? FIXME
                return "OK";
            }
            else
            {
                return Error.getError(TiposDeError.USUARIO_NO_AUTORIZADO);
            }
        }

        public static EstadoRetorno obtenerNombreUsuarioRevisor(int uid, int did) {
            // Obtener documento
            var documento = _db.Documento.Find(did);

            // Detección de errores
            if (documento == null) return new EstadoRetorno { estado = "ERROR", mensaje = Error.getError(TiposDeError.DOCUMENTO_NO_EXISTE) };
            if (documento.Estado_Id != (int)TiposDeEstado.PUBLICADO) return new EstadoRetorno { estado = "ERROR", mensaje = Error.getError(TiposDeError.DOCUMENTO_NO_PUBLICADO) };

            var mov = (from m in _db.MovimientosDeDocumento
                       where m.TipoEstado_Id == (int)TiposDeEstado.PUBLICADO
                       && m.Documento_Id == did
                       select m.Usuario.nombre).FirstOrDefault();
            return new EstadoRetorno { estado = "OK", mensaje = mov.ToString() };
        }

        /// <summary>
        /// Anotar movimiento
        /// </summary>
        /// <param name="uid">uid del Usuario que pide la anotación</param>
        /// <param name="did">did del documento</param>
        /// <param name="TipoEtapa_Id"></param>
        /// <param name="TipoEstado_id"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        private static int anotarMovimiento(int uid, int did, int TipoEtapa_Id, int TipoEstado_id, string descripcion) {
            var movimiento = new MovimientosDeDocumento();
            movimiento.descripcion = descripcion;
            movimiento.fechaOperacion = DateTime.Now;
            movimiento.Documento_Id = did;
            movimiento.Usuario_Id = uid;
            movimiento.TipoEtapa_Id = TipoEtapa_Id;
            movimiento.TipoEstado_Id = TipoEstado_id;
            _db.MovimientosDeDocumento.Add(movimiento);
            _db.SaveChanges();
            return movimiento.Id;
        }
    }
}