using IBD.Web;
using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Prestlan.Servicios {
    /// <summary>
    /// Descripción breve de NotificacionesService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class NotificacionesService : System.Web.Services.WebService {
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        [WebMethod(EnableSession = true)]
        public int getNumeroNotificaciones() {
            int uid = Utilidades.Autentificacion.GetUsuarioID();
            var docs = getDataNotificaciones();
            return docs.Count;
        }

        [WebMethod(EnableSession = true)]
        public List<ListNotificaciones> getNotificaciones() {
            var docs = getDataNotificaciones();
            return docs;
        }

        //protected IQueryable<ListNotificaciones> getDataNotificaciones_OLD() {
        //    var docs = (from d in _db.Documento
        //                where d.FechaCaducidad.HasValue &&
        //                (DbFunctions.AddDays(DateTime.Now, _db.Configuracion.FirstOrDefault().alerta_caducidad_1) > d.FechaCaducidad
        //                || DbFunctions.AddDays(DateTime.Now, _db.Configuracion.FirstOrDefault().alerta_caducidad_2) > d.FechaCaducidad)
        //                select new ListNotificaciones {
        //                    Descripcion = "El documento \"" + d.Titulo + "\" caducará en " + DbFunctions.DiffDays(DateTime.Now, d.FechaCaducidad.Value).ToString() + " días"
        //                });
        //    return docs;
        //}

        protected List<ListNotificaciones> getDataNotificaciones() {
            var config = _db.Configuracion.FirstOrDefault();
            int usuarioId = Utilidades.Autentificacion.GetUsuarioID();
            Usuario user = _db.Usuario.Find(usuarioId);
            bool mostrarTodo = Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.NotificacionesCaducidadListarTodo);
            bool mostrarSoloEmpresa = Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.NotificacionesCaducidadListarSoloEmpresa);

            //var docs = _db.Documento.Where(d => d.Eliminado != true && 
            //                                    d.Caduca == true && 
            //                                    d.FechaCaducidad.HasValue && 
            //                                    d.FechaCaducidad > DateTime.Now &&
            //                                    (mostrarTodo == true || (mostrarSoloEmpresa == true && d.Propietario.Any(x => x.Empresa_Id == user.Empresa_Id))) &&
            //                                (DbFunctions.AddDays(DateTime.Now, config.alerta_caducidad_1) > d.FechaCaducidad
            //                              || DbFunctions.AddDays(DateTime.Now, config.alerta_caducidad_2) > d.FechaCaducidad))
            //                              .AsEnumerable()
            //                              .Select(x => new ListNotificaciones() {
            //                                  Documento_Id = x.Id,
            //                                  Descripcion =  String.Format(Traduce.getTG("eldocumentoxcaducaraenydias"), x.Titulo, (x.FechaCaducidad.Value - DateTime.Now).TotalDays.ToString("0"))
            //                              }).ToList();

            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            var docs = (from d in _db.Documento.Where(d => d.Id == d.DocumentoVersion_Id &&
                                                            d.Eliminado != true &&
                                                            d.Caduca == true &&
                                                            d.Estado_Id != (int)TiposDeEstado.BORRADOR &&
                                                            d.FechaCaducidad.HasValue &&
                                                            (mostrarTodo == true || (mostrarSoloEmpresa == true && d.Propietario.Any(x => x.Empresa_Id == user.Empresa_Id))) &&
                                                            (DbFunctions.AddDays(DateTime.Now, config.alerta_caducidad_1) > d.FechaCaducidad
                                                          || DbFunctions.AddDays(DateTime.Now, config.alerta_caducidad_2) > d.FechaCaducidad))
                                from p in d.Propietario 
                                join tdt in _db.TipoDocumento_translation.Where(x => x.Idioma_codigo == idioma) on d.TipoDocumento_Id equals tdt.Tipodocumento_Id
                                select new {
                                    Documento_Id = d.Id,
                                    TipoDocumento = tdt.Descripcion,
                                    Propietario = p.Trabajador_Id.HasValue ? p.Trabajador.Nombre : p.Empresa.Nombre,
                                    FechaCaducidad = d.FechaCaducidad
                                }).AsEnumerable()
                              .Select(x => new ListNotificaciones()
                              {
                                  Documento_Id = x.Documento_Id,
                                  EstaCaducado = (x.FechaCaducidad.Value - DateTime.Now).TotalDays > 0,
                                  Descripcion = (x.FechaCaducidad.Value - DateTime.Now).TotalDays > 0 ? String.Format(Traduce.getTG("eldocumentoxcaducaraenydias"), String.Format("{0} - {1}", x.TipoDocumento, x.Propietario) , (x.FechaCaducidad.Value - DateTime.Now).TotalDays.ToString("0")) :
                                                String.Format(Traduce.getTG("eldocumentoxcaducado"), String.Format("{0} - {1}", x.TipoDocumento, x.Propietario))
                              }).OrderBy(x => x.EstaCaducado).ToList();

            return docs;
        }

    }
}
