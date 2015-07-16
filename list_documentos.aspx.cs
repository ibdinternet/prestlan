using IBD.Web;
using LinqKit;
using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Collections;
using System.Linq.Expressions;

namespace Prestlan {
    public partial class list_documentos : System.Web.UI.Page {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        private GridViewHelper _helper;
        private static string _tipoGrid = "Nombre";

        string _empleados = "";
        string _empresas = "";
        string _tipodocumentos = "";
        string _estados = "";
        public bool PermitirEdicion { get; set; }
        public bool Permiso_DocumentosListarTodo { get; set; }
        public bool Permiso_DocumentosListarSoloEmpresa { get; set; }
        protected override void LoadViewState(object savedState)
        {
            Dictionary<string, object> viewState = (Dictionary<string, object>)savedState;
            base.LoadViewState(viewState["base"]);
            PermitirEdicion = (bool)viewState["PermitirEdicion"];
            Permiso_DocumentosListarTodo = (bool)viewState["Permiso_DocumentosListarTodo"];
            Permiso_DocumentosListarSoloEmpresa = (bool)viewState["Permiso_DocumentosListarSoloEmpresa"];
        }

        protected override object SaveViewState()
        {
            Dictionary<string, object> viewState = new Dictionary<string, object>();
            viewState.Add("base", base.SaveViewState());
            viewState.Add("PermitirEdicion", PermitirEdicion);
            viewState.Add("Permiso_DocumentosListarTodo", Permiso_DocumentosListarTodo);
            viewState.Add("Permiso_DocumentosListarSoloEmpresa", Permiso_DocumentosListarSoloEmpresa);
            return viewState;
        }

        protected void Page_Load(object sender, EventArgs e) {
            setTraducciones();
            if (!Page.IsPostBack) {
                if (PermisosOK())
                {
                    BindGrid("");
                }
                else
                {
                    Response.Redirect("~/noAutorizado.aspx");
                }
            }            
        }
        private bool PermisosOK()
        {
            bool result = false;
            PermitirEdicion = false;
            int UsuarioId = Utilidades.Autentificacion.GetUsuarioID();
            if (UsuarioId > 0)
            {
                Permiso_DocumentosListarTodo = Utilidades.Autentificacion.comprobarPermiso(UsuarioId, TipoPermiso.DocumentosListarTodo);
                Permiso_DocumentosListarSoloEmpresa = Utilidades.Autentificacion.comprobarPermiso(UsuarioId, TipoPermiso.DocumentosListarSoloEmpresa);

                if (Permiso_DocumentosListarTodo)
                {
                    lbEmpresas.Visible = true;
                    lEmpresas.Visible = true;
                    lnkValidar.Visible = true;
                    lnkValidar.ToolTip = IBD.Web.Traduce.getTG("validar");
                }
                else
                {
                    lbEmpresas.Visible = false;
                    lEmpresas.Visible = false;
                    lnkValidar.Visible = false;
                }
                if (Permiso_DocumentosListarTodo || Permiso_DocumentosListarSoloEmpresa)
                {
                    if (Utilidades.Autentificacion.comprobarPermiso(UsuarioId, TipoPermiso.DocumentosEditarTodo) ||
                        Utilidades.Autentificacion.comprobarPermiso(UsuarioId, TipoPermiso.DocumentosEditarSoloEmpresa))
                    {
                        PermitirEdicion = true;
                    }
                    result = true;
                }
            }
            return result;
        }
        private void setTraducciones() {
            GridView1.Columns[2].HeaderText = Traduce.getTG("tipodedocumento");
            GridView1.Columns[3].HeaderText = Traduce.getTG("nombre");
            GridView1.Columns[4].HeaderText = Traduce.getTG("versiones");
            GridView1.Columns[5].HeaderText = Traduce.getTG("caducidad");
            GridView1.Columns[6].HeaderText = Traduce.getTG("fechacreado");
            //GridView1.Columns[1].HeaderText = Traduce.getTG("tipodedocumento");
            //GridView1.Columns[2].HeaderText = Traduce.getTG("nombre");
            //GridView1.Columns[3].HeaderText = Traduce.getTG("versiones");
            //GridView1.Columns[4].HeaderText = Traduce.getTG("caducidad");
            //GridView1.Columns[5].HeaderText = Traduce.getTG("fechacreado");
        }
        private void capturarParametros() {
            _empleados = "";
            _empresas = "";
            _tipodocumentos = "";
            _estados = "";
            foreach (ListItem empleado in lbEmpleados.Items)
            {
                if (empleado.Selected)
                {
                    _empleados = _empleados + empleado.Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(_empleados)) _empleados = _empleados.Substring(0, _empleados.Length - 1);
            foreach (ListItem empresa in lbEmpresas.Items)
            {
                if (empresa.Selected)
                {
                    _empresas = _empresas + empresa.Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(_empresas)) _empresas = _empresas.Substring(0, _empresas.Length - 1);
            foreach (ListItem tipoDocumento in lbTipoDocumentos.Items)
            {
                if (tipoDocumento.Selected)
                {
                    _tipodocumentos = _tipodocumentos + tipoDocumento.Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(_tipodocumentos)) _tipodocumentos = _tipodocumentos.Substring(0, _tipodocumentos.Length - 1);
            foreach (ListItem estado in lbEstados.Items)
            {
                if (estado.Selected)
                {
                    _estados = _estados + estado.Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(_estados)) _estados = _estados.Substring(0, _estados.Length - 1);
        }
        protected void agrupaGrid() {
            // Para hacer group de columnas
            _helper = new GridViewHelper(GridView1, false);
            _helper.RegisterGroup(_tipoGrid, true, true);
            _helper.ApplyGroupSort();
        }
        protected void setComboOrdenacion() {
            if (_tipoGrid == "Nombre") {
                ddOrdenacion.Items.Clear();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem(Traduce.getTG("fechaascendente"), "Fecha ASC"));
                items.Add(new ListItem(Traduce.getTG("fechadescendente"), "Fecha DESC"));
                items.Add(new ListItem(Traduce.getTG("caducidadascendente"), "Caducidad ASC"));
                items.Add(new ListItem(Traduce.getTG("caducidaddescendente"), "Caducidad DESC"));
                items.Add(new ListItem(Traduce.getTG("propietarioascendente"), "Propietario ASC"));
                items.Add(new ListItem(Traduce.getTG("propietariodescendente"), "Propietario DESC"));
                ddOrdenacion.Items.AddRange(items.ToArray());
            } else {
                ddOrdenacion.Items.Clear();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem(Traduce.getTG("fechaascendente"), "Fecha ASC"));
                items.Add(new ListItem(Traduce.getTG("fechadescendente"), "Fecha DESC"));
                items.Add(new ListItem(Traduce.getTG("caducidadascendente"), "Caducidad ASC"));
                items.Add(new ListItem(Traduce.getTG("caducidaddescendente"), "Caducidad DESC"));
                items.Add(new ListItem(Traduce.getTG("tipodedocascendente"), "TipoDocumento ASC"));
                items.Add(new ListItem(Traduce.getTG("tipodedocdescendente"), "TipoDocumento DESC"));
                ddOrdenacion.Items.AddRange(items.ToArray());
            }
        }

        protected IList<ListDocumentoPropietarioSearch> getData(string orden) {
            int Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();
            Usuario user = _db.Usuario.Find(Usuario_Id);
            int empresaUsuario = user.Empresa_Id;
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            var data = (from d in _db.Documento.Where(x => x.Eliminado != true && x.Id == x.DocumentoVersion_Id)
                       from p in d.Propietario
                        join tdt in _db.TipoDocumento_translation.Where(x => x.Idioma_codigo == idioma) on d.TipoDocumento_Id equals tdt.Tipodocumento_Id
                       select new ListDocumentoPropietarioSearch
                        {
                            Propietario_Id = p.Id,
                            Documento_Id = d.Id,
                            TipoDocumento = tdt.Descripcion,
                            FechaCreado = d.FechaCreado,
                            Nombre = p.Trabajador_Id.HasValue ? p.Trabajador.Nombre : p.Empresa.Nombre,
                            FechaCaducidad = d.FechaCaducidad,
                            Empresa_Id = p.Empresa_Id,
                            Trabajador_Id = p.Trabajador_Id,
                            TipoDocumento_Id = d.TipoDocumento.Id,
                            Fichero_Id = d.Fichero_Id,
                            Estado_Id = d.Estado_Id,
                        }).Where(x => Permiso_DocumentosListarTodo == true || 
                                    (Permiso_DocumentosListarSoloEmpresa && x.Empresa_Id == empresaUsuario));

            Expression<Func<ListDocumentoPropietarioSearch, bool>> predicado_where_empresas = null;
            Expression<Func<ListDocumentoPropietarioSearch, bool>> predicado_where_empleados = null;
            Expression<Func<ListDocumentoPropietarioSearch, bool>> predicado_where_tdocumentos = null;
            Expression<Func<ListDocumentoPropietarioSearch, bool>> predicado_where_estados = null;
            if (_empresas != "") {
                predicado_where_empresas = PredicateBuilder.False<ListDocumentoPropietarioSearch>();
                List<int> empresas_id = _empresas.Split(',').Select(int.Parse).ToList();
                foreach (int el in empresas_id) {
                    int temp = el;
                    predicado_where_empresas = predicado_where_empresas.Or(p => p.Empresa_Id == temp);
                }
            }
            if (_empleados != "") {
                predicado_where_empleados = PredicateBuilder.False<ListDocumentoPropietarioSearch>();
                List<int> empleados_id = _empleados.Split(',').Select(int.Parse).ToList();
                foreach (int el in empleados_id) {
                    int temp = el;
                    predicado_where_empleados = predicado_where_empleados.Or(p => p.Trabajador_Id == temp);
                }
            }
            if (_tipodocumentos != "") {
                predicado_where_tdocumentos = PredicateBuilder.False<ListDocumentoPropietarioSearch>();
                List<int> tipodocumentos_id = _tipodocumentos.Split(',').Select(int.Parse).ToList();
                foreach (int el in tipodocumentos_id) {
                    int temp = el;
                    predicado_where_tdocumentos = predicado_where_tdocumentos.Or(p => p.TipoDocumento_Id == temp);
                }
            }
            if (_estados != "")
            {
                predicado_where_estados = PredicateBuilder.False<ListDocumentoPropietarioSearch>();
                List<int> estados_id = _estados.Split(',').Select(int.Parse).ToList();
                foreach (int el in estados_id)
                {
                    int temp = el;
                    predicado_where_estados = predicado_where_estados.Or(p => p.Estado_Id == temp);
                }
            }
            switch (orden) {
                case "Fecha ASC":
                    if (_tipoGrid == "Nombre") data = data.OrderBy(o => o.Nombre).ThenBy(f => f.FechaCreado);
                    else data = data.OrderBy(o => o.TipoDocumento).ThenBy(f => f.FechaCreado);
                    break;
                case "Fecha DESC":
                    if (_tipoGrid == "Nombre") data = data.OrderBy(o => o.Nombre).ThenByDescending(f => f.FechaCreado);
                    else data = data.OrderBy(o => o.TipoDocumento).ThenByDescending(f => f.FechaCreado);
                    break;
                case "Caducidad ASC":
                    if (_tipoGrid == "Nombre") data = data.OrderBy(o => o.Nombre).ThenBy(f => f.FechaCaducidad);
                    else data = data.OrderBy(o => o.TipoDocumento).ThenBy(f => f.FechaCaducidad);
                    break;
                case "Caducidad DESC":
                    if (_tipoGrid == "Nombre") data = data.OrderBy(o => o.Nombre).ThenByDescending(f => f.FechaCaducidad);
                    else data = data.OrderBy(o => o.TipoDocumento).ThenByDescending(f => f.FechaCaducidad);
                    break;
                case "Propietario ASC":
                    data = data.OrderBy(o => o.Nombre);
                    break;
                case "Propietario DESC":
                    data = data.OrderByDescending(o => o.Nombre);
                    break;
                case "TipoDocumento ASC":
                    data = data.OrderBy(o => o.TipoDocumento).ThenBy(f => f.Nombre);
                    break;
                case "TipoDocumento DESC":
                    data = data.OrderByDescending(o => o.TipoDocumento).ThenBy(f => f.Nombre);
                    break;
                default:
                    if (_tipoGrid == "Nombre") data = data.OrderBy(o => o.Nombre).ThenBy(f => f.FechaCreado);
                    else data = data.OrderBy(o => o.TipoDocumento).ThenBy(f => f.FechaCreado);
                    ddOrdenacion.SelectedIndex = 0;
                    break;
            }
            if (_empresas != "" || _empleados != "" || _tipodocumentos != "" || _estados != "")
            {
                data = data.AsExpandable();
                if (_empresas != "")
                {
                    data = data.Where(predicado_where_empresas);
                }
                if (_empleados != "")
                {
                    data = data.Where(predicado_where_empleados);
                }
                if (_tipodocumentos != "")
                {
                    data = data.Where(predicado_where_tdocumentos);
                }
                if (_estados != "")
                {
                    data = data.Where(predicado_where_estados);
                }
                return data.ToList();
            }
            else
            {
                return data.ToList();
            }
        }        

        public void BindGrid(string orden) {
            capturarParametros();
            if (!Page.IsPostBack) setComboOrdenacion();
            agrupaGrid();
            var data = getData(orden);

            if (data != null && data.Count > 0) {
                GridView1.DataSource = data;
            } else {
                GridView1.DataSource = null;
            }
            GridView1.DataBind();
        }

        protected void ddOrdenacion_SelectedIndexChanged(object sender, EventArgs e) {
            BindGrid(ddOrdenacion.SelectedValue);
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e) {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                // e.Row.Cells[4].Text es la celda de versiones
                string salida = "";
                int num_version = 0;
                ListDocumentoPropietarioSearch item = e.Row.DataItem as ListDocumentoPropietarioSearch;
                var data = (from d in _db.Documento
                            join est in _db.TipoEstado on d.Estado_Id equals est.Id
                            join eta in _db.TipoEtapa on d.Etapa_Id equals eta.Id
                            where d.DocumentoVersion_Id == item.Documento_Id && d.Eliminado == false
                            orderby d.Id
                            select new DocumentoFichero
                            {
                                Documento_Id = d.Id,
                                Estado_Id = d.Estado_Id,
                                Estado = est.descripcion,
                                Etapa_Id = d.Etapa_Id,
                                Etapa = eta.descripcion,
                                FechaCaducidad = d.FechaCaducidad,
                                Fichero_Id = d.Fichero_Id
                            }).ToList();
                bool esultimo = false;
                
                foreach (DocumentoFichero doc in data)
                {
                    num_version++;
                    if (num_version == data.Count) esultimo = true;
                    if (num_version > (data.Count - 5))
                    {
                        salida += "<a href='#' class='" + getClassEstadoDocumento(doc) + "' onclick='generateDialog(event, " + doc.Documento_Id.ToString() + ", " + num_version.ToString() + ", " + esultimo.ToString().ToLower() + ", " + doc.Fichero_Id + ")'>" + num_version.ToString() + ".0</a>&nbsp;";
                    }
                    else
                    {
                        salida += "<a href='#' class='" + getClassEstadoDocumento(doc) + " hidden' onclick='generateDialog(event, " + doc.Documento_Id.ToString() + ", " + num_version.ToString() + ", " + esultimo.ToString().ToLower() + ", " + doc.Fichero_Id + ")'>" + num_version.ToString() + ".0</a>&nbsp;";
                    }
                    
                }
                e.Row.Cells[4].Text = salida;
            }
        }

        /// <summary>
        /// Establece la clase que representa estado del documento detectando además si el documento está caducado:
        /// borrador, publicado, caducado
        /// Si detectamos un documento caducable lo caducamos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        protected string getClassEstadoDocumento(DocumentoFichero item)
        {
            if (item.Estado_Id != (int)TiposDeEstado.CADUCADO)
            {
                if (item.FechaCaducidad.HasValue)
                {
                    if ((DateTime.Now - item.FechaCaducidad.Value).TotalDays >= 1)
                    {
                        int uid = Utilidades.Autentificacion.GetUsuarioID();
                        Utilidades.OperacionesDocumentos.pasarACaducado(uid, item.Documento_Id);
                        return "caducado";
                    }
                    else if (item.Estado_Id == (int)TiposDeEstado.BORRADOR)
                    {
                        //Revision o rechazo
                        return item.Etapa.ToLower();
                    }
                }
                else if (item.Estado_Id == (int)TiposDeEstado.BORRADOR)
                {
                    //Revision o rechazo
                    return item.Etapa.ToLower();
                }
            }
            return item.Estado.ToLower();
        }


        protected void lnkPropietario_Click(object sender, EventArgs e) {
            lnkPropietario.CssClass = "btn btn-default linkactive";
            lnkTipoDocumento.CssClass = "btn btn-default link";
            _tipoGrid = "Nombre";
            BindGrid("");
        }

        protected void lnkTipoDocumento_Click(object sender, EventArgs e) {
            lnkPropietario.CssClass = "btn btn-default link";
            lnkTipoDocumento.CssClass = "btn btn-default linkactive";
            _tipoGrid = "TipoDocumento";
            BindGrid("");
        }

        public IList GetEmpresas()
        {
            return _db.Empresa.Where(x => x.Eliminado != true).Select(x => new
            {
                x.Id,
                x.Nombre
            }).ToList();
        }
        public IList GetTrabajadores()
        {
            if (Permiso_DocumentosListarTodo)
            {
                var trabajadores = _db.Trabajador.Where(x => x.Eliminado != true).OrderBy(x => x.Empresa.Nombre).Select(x => new ListTrabajador
                {
                    Id = x.Id,
                    Nombre = x.Nombre + "[@*separador*@]" + x.Empresa.Nombre,
                    Empresa_Id = x.Empresa_Id
                });
                return trabajadores.ToList();
            }
            else
            {
                Usuario user = _db.Usuario.Find(Utilidades.Autentificacion.GetUsuarioID());
                var trabajadores = _db.Trabajador.Where(x => x.Eliminado != true && x.Empresa_Id == user.Empresa_Id).OrderBy(x => x.Empresa.Nombre).Select(x => new ListTrabajador
                {
                    Id = x.Id,
                    Nombre = x.Nombre + "[@*separador*@]" + x.Empresa.Nombre,
                    Empresa_Id = x.Empresa_Id
                });
                return trabajadores.ToList();
            }              
        }
        public IList GetTipoDocumentos()
        {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            return _db.TipoDocumento_translation.Where(a => a.Idioma_codigo == idioma).Select(x => new
            {
                Id = x.Tipodocumento_Id,
                x.Descripcion
            }).ToList();
        }
        public IList GetActividades()
        {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            return _db.Actividad_translation.Where(a => a.Idioma_codigo == idioma).Select(x => new {
                Id = x.Actividad_Id,
                x.Descripcion
            }).ToList();
        }
        public IList GetEstados()
        {
            var estados = _db.TipoEstado.Select(x => new
            {
                x.Id,
                x.descripcion
            }).ToList();
            return estados;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            BindGrid("");
        }
    }
}