using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using Prestlan.Models;
using System.Data.Common;
using IBD.Web;
using System.Data.Entity.Validation;

namespace Prestlan {
    public partial class list_empresa : System.Web.UI.Page {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        protected void Page_Load(object sender, EventArgs e) {
            setTraducciones();
            if (!PermisosOK())
            {
                Response.Redirect("~/noAutorizado.aspx");
            }
        }

        public bool PermitirEdicion { get; set; }
        public bool Permiso_EmpresasListarTodo { get; set; }
        public bool Permiso_EmpresasListarSoloEmpresa { get; set; }
        protected override void LoadViewState(object savedState)
        {
            Dictionary<string, object> viewState = (Dictionary<string, object>)savedState;
            base.LoadViewState(viewState["base"]);
            PermitirEdicion = (bool)viewState["PermitirEdicion"];
            Permiso_EmpresasListarTodo = (bool)viewState["Permiso_EmpresasListarTodo"];
            Permiso_EmpresasListarSoloEmpresa = (bool)viewState["Permiso_EmpresasListarSoloEmpresa"];
        }

        protected override object SaveViewState()
        {
            Dictionary<string, object> viewState = new Dictionary<string, object>();
            viewState.Add("base", base.SaveViewState());
            viewState.Add("PermitirEdicion", PermitirEdicion);
            viewState.Add("Permiso_EmpresasListarTodo", Permiso_EmpresasListarTodo);
            viewState.Add("Permiso_EmpresasListarSoloEmpresa", Permiso_EmpresasListarSoloEmpresa);
            return viewState;
        }

        private bool PermisosOK()
        {
            bool result = false;
            int Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();
            Permiso_EmpresasListarTodo = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.EmpresasListarTodo);
            Permiso_EmpresasListarSoloEmpresa = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.EmpresasListarSoloEmpresa);
            if (Permiso_EmpresasListarTodo || Permiso_EmpresasListarSoloEmpresa)
            {
                result = true;
            }
            if (Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.EmpresasEditarTodo) ||
                Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.EmpresasEditarSoloEmpresa))
            {
                PermitirEdicion = true;
            }
            else
            {
                PermitirEdicion = false;
            }
            return result;
        }

        private void setTraducciones() {
            if (ListView1.DataSource != null)
            {
                ((Label)ListView1.FindControl("lblListarSubcontrata")).Text = Traduce.getTG("listarsubcontrata");
                ((LinkButton)ListView1.FindControl("lnkNombre")).Text = Traduce.getTG("nombre");
                ((LinkButton)ListView1.FindControl("lnkActiva")).Text = Traduce.getTG("activa");
                ((LinkButton)ListView1.FindControl("lnkFechaAlta")).Text = Traduce.getTG("fechadealta");
                ((LinkButton)ListView1.FindControl("lnkFechaBaja")).Text = Traduce.getTG("fechadebaja");
                ((LinkButton)ListView1.FindControl("lnkTipoEmpresa")).Text = Traduce.getTG("tipodeempresa");
                ((LinkButton)ListView1.FindControl("lnkActividad")).Text = Traduce.getTG("actividad");
                ((NextPreviousPagerField)((DataPager)ListView1.FindControl("datapagerLV1")).Fields[0]).PreviousPageText = Traduce.getTG("anterior");
                ((NextPreviousPagerField)((DataPager)ListView1.FindControl("datapagerLV1")).Fields[2]).NextPageText = Traduce.getTG("siguiente");   
            }         
        }
        public IQueryable<ListEmpresa> GetData() {            
            string idioma = "es";
            if (Session["Idioma"] != null) 
            {
                idioma = Session["Idioma"].ToString();
            }

            Usuario user = _db.Usuario.Find(Utilidades.Autentificacion.GetUsuarioID());
            var q = (from e in _db.Empresa.Where(x => x.Eliminado != true)
                     join te in _db.TipoEmpresa on e.TipoEmpresa_Id equals te.Id
                     join tet in _db.TipoEmpresa_translation.Where(x => x.Idioma_codigo == idioma) on te.Id equals tet.Tipoempresa_Id

                     join ti in _db.TipoIdentificador on e.TipoIdentificador_Id equals ti.Id
                     join tit in _db.Tipoidentificador_translation.Where(x => x.Idioma_codigo == idioma) on te.Id equals tit.Tipoidentificador_Id

                     join a in _db.Actividad on e.Actividad_Id equals a.Id
                     join at in _db.Actividad_translation.Where(x => x.Idioma_codigo == idioma) on a.Id equals at.Actividad_Id

                     where Permiso_EmpresasListarTodo == true || (Permiso_EmpresasListarSoloEmpresa == true && e.Id == user.Empresa_Id)
                     select new ListEmpresa {
                         Id = e.Id,
                         ValorIdentificador = e.ValorIdentificador,
                         Nombre = e.Nombre,
                         Observaciones = e.Observaciones,
                         Direccion = e.Direccion,
                         SinNotificaciones = e.SinNotificaciones,
                         CorreosNotificaciones = e.CorreosNotificaciones,
                         Activa = e.Activa,
                         FechaAlta = e.FechaAlta,
                         FechaBaja = e.FechaBaja,
                         TipoIdentificador = tit.Descripcion,
                         TipoEmpresa = tet.Descripcion,
                         Actividad = at.Descripcion,
                         Tipoempresa_Id = e.TipoEmpresa_Id,
                         Actividad_Id = e.Actividad_Id
                     });
            return q.AsQueryable().OrderBy(x => x.Tipoempresa_Id);
        }

        // El nombre de parámetro del id. debe coincidir con el valor DataKeyNames establecido en el control
        public void ListView1_DeleteItem(int id)
        {
            //En realidad no se elimina, se marca como eliminado

            //Marco como eliminada la empresa
            Empresa empresa = _db.Empresa.Find(id);
            empresa.Eliminado = true;

            //Marco como eliminados los trabajadores de la empresa
            var trabajadores = _db.Trabajador.Where(x => x.Empresa_Id == id);
            foreach (Trabajador trabajador in trabajadores)
            {
                trabajador.Eliminado = true;
            }

            //Marco como eliminados los documentos de la empresa y de sus trabajadores
            var documentos = _db.Documento.Where(x => x.Propietario.FirstOrDefault(p => p.Empresa_Id == id) != null);
            foreach (Documento doc in documentos)
            {
                doc.Eliminado = true;
            }

            _db.SaveChanges();
        }
    }
}