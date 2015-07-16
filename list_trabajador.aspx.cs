using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Collections;
using LinqKit;
namespace Prestlan {
    public partial class list_trabajador : System.Web.UI.Page {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        private GridViewHelper _helper;

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                if (PermisosOK())
                {
                    BindGrid();
                }
                else
                {
                    Response.Redirect("~/noautorizado.aspx");
                }
            }
        }
        private bool PermisosOK()
        {
            int UsuarioId = Utilidades.Autentificacion.GetUsuarioID();
            Permiso_TrabajadoresListarTodo = Utilidades.Autentificacion.comprobarPermiso(UsuarioId, TipoPermiso.TrabajadoresListarTodo);
            Permiso_TrabajadoresListarSoloEmpresa = Utilidades.Autentificacion.comprobarPermiso(UsuarioId, TipoPermiso.TrabajadoresListarSoloEmpresa);
            pnlFiltroEmpresa.Visible = Permiso_TrabajadoresListarTodo;
            if (Permiso_TrabajadoresListarTodo || Permiso_TrabajadoresListarSoloEmpresa)
            {
                if (Utilidades.Autentificacion.comprobarPermiso(UsuarioId, TipoPermiso.TrabajadoresEditarTodo) ||
                    Utilidades.Autentificacion.comprobarPermiso(UsuarioId, TipoPermiso.TrabajadoresEditarSoloEmpresa))
                {
                    PermitirEdicion = true;
                }
                else
                {
                    PermitirEdicion = false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PermitirEdicion { get; set; }
        public bool Permiso_TrabajadoresListarTodo { get; set; }
        public bool Permiso_TrabajadoresListarSoloEmpresa { get; set; }
        protected override void LoadViewState(object savedState)
        {
            Dictionary<string, object> viewState = (Dictionary<string, object>)savedState;
            base.LoadViewState(viewState["base"]);
            PermitirEdicion = (bool)viewState["PermitirEdicion"];
            Permiso_TrabajadoresListarTodo = (bool)viewState["Permiso_TrabajadoresListarTodo"];
            Permiso_TrabajadoresListarSoloEmpresa = (bool)viewState["Permiso_TrabajadoresListarSoloEmpresa"];
        }

        protected override object SaveViewState()
        {
            Dictionary<string, object> viewState = new Dictionary<string, object>();
            viewState.Add("base", base.SaveViewState());
            viewState.Add("PermitirEdicion", PermitirEdicion);
            viewState.Add("Permiso_TrabajadoresListarTodo", Permiso_TrabajadoresListarTodo);
            viewState.Add("Permiso_TrabajadoresListarSoloEmpresa", Permiso_TrabajadoresListarSoloEmpresa);
            return viewState;
        }

        protected void agrupaGrid() {
            // Para hacer group de columnas
            _helper = new GridViewHelper(GridView1, false);
            _helper.RegisterGroup("Empresa", true, true);
            _helper.ApplyGroupSort();
        }
       
        protected IList<ListTrabajador> getData() {
            var predicado_where = PredicateBuilder.False<ListTrabajador>();
            string idioma = Session["Idioma"].ToString();
            int Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();
            Usuario user = _db.Usuario.Find(Usuario_Id);
            int empresaUsuario = user.Empresa_Id;
            var data = (from t in _db.Trabajador.Where(x => x.Eliminado != true)
                        orderby t.Empresa.Nombre          
                    select new ListTrabajador {
                        Id = t.Id,
                        Nombre = t.Nombre,
                        Empresa = t.Empresa.Nombre,
                        ValorIdentificador = t.ValorIdentificador,
                        Actividad = t.Actividad.FirstOrDefault().Actividad_translation.FirstOrDefault(a => a.Idioma_codigo == idioma).Descripcion,
                        Empleado = t.TipoTrabajador.TipoTrabajador_translation.FirstOrDefault(a => a.Idioma_codigo == idioma).Descripcion,
                        Activo = t.Activo,
                        FechaAlta = t.FechaAlta,
                        Empresa_Id = t.Empresa_Id
                    }).Where(x => Permiso_TrabajadoresListarTodo == true || (Permiso_TrabajadoresListarSoloEmpresa && x.Empresa_Id == empresaUsuario));

            if (_empresas != "")
            {
                List<int> empresas_id = _empresas.Split(',').Select(int.Parse).ToList();
                foreach (int el in empresas_id)
                {
                    int temp = el;
                    predicado_where = predicado_where.Or(p => p.Empresa_Id == temp);
                }
            }
            if (_empresas != "") return data.AsExpandable().Where(predicado_where).ToList();
            else return data.ToList();
        }

        public void BindGrid() {
            capturarParametros();
            agrupaGrid();
            var data = getData();

            if (data != null && data.Count > 0) {
                GridView1.DataSource = data;
            } else {
                GridView1.DataSource = null;
            }
            GridView1.DataBind();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e) {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        public IList GetEmpresas()
        {
            return _db.Empresa.Where(x => x.Eliminado != true).Select(x => new
            {
                x.Id,
                x.Nombre
            }).ToList();
        }

        string _empresas = "";
        private void capturarParametros()
        {
            _empresas = "";
            foreach (ListItem empresa in lbEmpresas.Items)
            {
                if (empresa.Selected)
                {
                    _empresas = _empresas + empresa.Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(_empresas)) _empresas = _empresas.Substring(0, _empresas.Length - 1);
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            var cell = ((LinkButton)sender).Parent;
            int id = int.Parse(((HiddenField)cell.FindControl("HiddenIdTrabajador")).Value);
            //En realidad no se elimina, se marca como eliminado

            //Marco como eliminado el trabajador
            Trabajador trabajador = _db.Trabajador.Find(id);
            trabajador.Eliminado = true;

            //Marco como eliminados los documentos del trabajador
            var documentos = _db.Documento.Where(x => x.Propietario.FirstOrDefault(p => p.Trabajador_Id == id) != null);
            foreach (Documento doc in documentos)
            {
                doc.Eliminado = true;
            }

            _db.SaveChanges();
            BindGrid();
        }

        //protected void lbEditar_Click(object sender, EventArgs e) {
        //    string selectedValue = Request.Form["rbTrabajador"];
        //    if (!string.IsNullOrEmpty(selectedValue))
        //    {
        //        Response.Redirect("trabajador.aspx?mode=Edit&id=" + selectedValue); 
        //    }                       
        //}

    }
}