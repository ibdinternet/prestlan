using LinqKit;
using Prestlan.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class reclamaciones : System.Web.UI.Page {
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        private GridViewHelper _helper;
        private static string _tipoGrid = "Nombre";

        string _empleados = "";
        string _empresas = "";
        string _tipodocumentos = "";
        protected void Page_Load(object sender, EventArgs e) {            
            if (!Page.IsPostBack) {
                if (PermisosOK())
                {
                    BindGrid("");
                    setComboOrdenacion();
                }
                else
                {
                    Response.Redirect("~/noautorizado.aspx");
                }
            }
        }
        public bool PermitirEdicion { get; set; }
        protected override void LoadViewState(object savedState)
        {
            Dictionary<string, object> viewState = (Dictionary<string, object>)savedState;
            base.LoadViewState(viewState["base"]);
            PermitirEdicion = (bool)viewState["PermitirEdicion"];
        }

        protected override object SaveViewState()
        {
            Dictionary<string, object> viewState = new Dictionary<string, object>();
            viewState.Add("base", base.SaveViewState());
            viewState.Add("PermitirEdicion", PermitirEdicion);
            return viewState;
        }
        private bool PermisosOK()
        {
            PermitirEdicion = Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.ReclamacionesEditar);
            return Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.ReclamacionesListar);
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
                items.Add(new ListItem("Fecha ascendente", "Fecha ASC"));
                items.Add(new ListItem("Fecha descendente", "Fecha DESC"));
                items.Add(new ListItem("Caducidad ascendente", "Caducidad ASC"));
                items.Add(new ListItem("Caducidad desncendente", "Caducidad DESC"));
                items.Add(new ListItem("Propietario ascendente", "Propietario ASC"));
                items.Add(new ListItem("Propietario descendente", "Propietario DESC"));
                ddOrdenacion.Items.AddRange(items.ToArray());
            } else {
                ddOrdenacion.Items.Clear();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem("Fecha ascendente", "Fecha ASC"));
                items.Add(new ListItem("Fecha descendente", "Fecha DESC"));
                items.Add(new ListItem("Caducidad ascendente", "Caducidad ASC"));
                items.Add(new ListItem("Caducidad desncendente", "Caducidad DESC"));
                items.Add(new ListItem("Tipo de documento ascendente", "TipoDocumento ASC"));
                items.Add(new ListItem("Tipo de documento descendente", "TipoDocumento DESC"));
                ddOrdenacion.Items.AddRange(items.ToArray());
            }
        }
        protected IList<ListDocumentoPropietario> getData(string orden) {
            var predicado_where = PredicateBuilder.False<ListDocumentoPropietario>();
            var data = (from p in _db.Propietario
                        where p.Documento.FirstOrDefault().Id == p.Documento.FirstOrDefault().DocumentoVersion_Id
                        && p.Documento.FirstOrDefault().Eliminado == false
                        select new ListDocumentoPropietario {
                            Trabajador_Id = p.Trabajador_Id,
                            Empresa_Id = p.Empresa_Id,
                            TipoDocumento_Id = p.Documento.FirstOrDefault().TipoDocumento_Id,
                            Propietario_Id = p.Id,
                            Documento_Id = p.Documento.FirstOrDefault().Id,
                            TipoDocumento = p.Documento.FirstOrDefault().TipoDocumento.TipoDocumento_translation.FirstOrDefault().Descripcion,
                            FechaCreado = p.Documento.FirstOrDefault().FechaCreado,
                            Nombre = p.Trabajador_Id.HasValue ? p.Trabajador.Nombre : p.Empresa.Nombre,
                            FechaCaducidad = p.Documento.FirstOrDefault().FechaCaducidad,
                            Etapa = p.Documento.FirstOrDefault().TipoEtapa.descripcion
                        });
            Expression<Func<ListDocumentoPropietario, bool>> predicado_where_empresas = null;
            Expression<Func<ListDocumentoPropietario, bool>> predicado_where_empleados = null;
            Expression<Func<ListDocumentoPropietario, bool>> predicado_where_tdocumentos = null;
            if (_empresas != "")
            {
                predicado_where_empresas = PredicateBuilder.False<ListDocumentoPropietario>();
                List<int> empresas_id = _empresas.Split(',').Select(int.Parse).ToList();
                foreach (int el in empresas_id)
                {
                    int temp = el;
                    predicado_where_empresas = predicado_where_empresas.Or(p => p.Empresa_Id == temp);
                }
            }
            if (_empleados != "")
            {
                predicado_where_empleados = PredicateBuilder.False<ListDocumentoPropietario>();
                List<int> empleados_id = _empleados.Split(',').Select(int.Parse).ToList();
                foreach (int el in empleados_id)
                {
                    int temp = el;
                    predicado_where_empleados = predicado_where_empleados.Or(p => p.Trabajador_Id == temp);
                }
            }
            if (_tipodocumentos != "")
            {
                predicado_where_tdocumentos = PredicateBuilder.False<ListDocumentoPropietario>();
                List<int> tipodocumentos_id = _tipodocumentos.Split(',').Select(int.Parse).ToList();
                foreach (int el in tipodocumentos_id)
                {
                    int temp = el;
                    predicado_where_tdocumentos = predicado_where_tdocumentos.Or(p => p.TipoDocumento_Id == temp);
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
            if (_empresas != "" || _empleados != "" || _tipodocumentos != "")
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
                return data.ToList();
            }
            else
            {
                return data.ToList();
            }
        }

        public void BindGrid(string orden) {
            capturarParametros();
            agrupaGrid();
            var data = getData(orden);

            if (data != null && data.Count > 0) {
                GridView1.DataSource = data;
            } else {
                GridView1.DataSource = null;
            }
            GridView1.DataBind();
        }

        private void capturarParametros()
        {
            _empleados = "";
            _empresas = "";
            _tipodocumentos = "";
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
                int propietario_id = Convert.ToInt32(GridView1.DataKeys[e.Row.RowIndex].Values[0].ToString());
                // int documento_id = Convert.ToInt32(GridView1.Rows[e.Row.RowIndex].Cells[1].Text);
                int document_id = _db.Propietario.Find(propietario_id).Documento.FirstOrDefault().Id;
                var data = (from d in _db.Documento
                            where d.DocumentoVersion_Id == document_id && d.Eliminado == false
                            orderby d.Id
                            select
                            d.Id).ToList();
                foreach (int i in data) {
                    num_version++;
                    if (num_version > (data.Count - 5))
                    {
                        salida += "<a href='#' class='" + getClassEstadoDocumento(i) + "' onclick='generateDialog(event, " + propietario_id.ToString() + ")'>" + num_version.ToString() + ".0</a>&nbsp;";
                    }
                    else
                    {
                        salida += "<a href='#' class='" + getClassEstadoDocumento(i) + " hidden' onclick='generateDialog(event, " + propietario_id.ToString() + ")'>" + num_version.ToString() + ".0</a>&nbsp;";
                    }                    
                }
                e.Row.Cells[4].Text = salida;
            }
        }
        //protected string getClassEstadoDocumento(int id) {
        //    var q = (from d in _db.Documento
        //             where d.Id == id
        //             select d.TipoEstado.descripcion);
        //    return q.FirstOrDefault().ToLower();
        //}

        /// <summary>
        /// Establece la clase que representa estado del documento detectando además si el documento está caducado:
        /// borrador, publicado, caducado
        /// Si detectamos un documento caducable lo caducamos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        protected string getClassEstadoDocumento(int id)
        {
            var documento = _db.Documento.Find(id);
            if (documento.Estado_Id != (int)TiposDeEstado.CADUCADO)
            {
                if (documento.FechaCaducidad.HasValue)
                {
                    if ((DateTime.Now - documento.FechaCaducidad.Value).TotalDays >= 1)
                    {
                        int uid = Utilidades.Autentificacion.GetUsuarioID();
                        Utilidades.OperacionesDocumentos.pasarACaducado(uid, id);
                        return "caducado";
                    }
                    else if (documento.Estado_Id == (int)TiposDeEstado.BORRADOR)
                    {
                        //Revision o rechazo
                        return documento.TipoEtapa.descripcion.ToLower();
                    }
                }
                else if (documento.Estado_Id == (int)TiposDeEstado.BORRADOR)
                {
                    //Revision o rechazo
                    return documento.TipoEtapa.descripcion.ToLower();
                }
            }
            return documento.TipoEstado.descripcion.ToLower();
        }

        protected void lbPropietario_Click(object sender, EventArgs e) {
            lbPropietario.CssClass = "btn btn-default linkactive";
            lbTipoDocumento.CssClass = "btn btn-default link";
            _tipoGrid = "Nombre";
            setComboOrdenacion();
            BindGrid("");
        }

        protected void lbTipoDocumento_Click(object sender, EventArgs e) {
            lbPropietario.CssClass = "btn btn-default link";
            lbTipoDocumento.CssClass = "btn btn-default linkactive";
            _tipoGrid = "TipoDocumento";
            setComboOrdenacion();
            BindGrid("");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
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
            var trabajadores = _db.Trabajador.Where(x => x.Eliminado != true).OrderBy(x => x.Empresa.Nombre).Select(x => new ListTrabajador
            {
                Id = x.Id,
                Nombre = x.Nombre + "[@*separador*@]" + x.Empresa.Nombre,
                Empresa_Id = x.Empresa_Id
            });
            return trabajadores.ToList();
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
    }
}