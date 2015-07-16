using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestlan.Models;
using System.Configuration;
using LinqKit;
using System.Collections;
using System.Linq.Expressions;

namespace Prestlan {
    public partial class validaciones : System.Web.UI.Page {

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
        private bool PermisosOK()
        {
            int usuarioId = Utilidades.Autentificacion.GetUsuarioID();
            pnlAcciones.Visible = Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.ValidacionesEditar);
            return Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.ValidacionesListar);
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

        protected IList<ListDocumentoPropietarioValidacion> getData(string orden) {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            var data = (from d in _db.Documento.Where(x => x.Eliminado != true && x.Etapa_Id == (int)TiposDeEtapa.REVISION)
                        from p in d.Propietario
                        join tdt in _db.TipoDocumento_translation.Where(x => x.Idioma_codigo == idioma) on d.TipoDocumento_Id equals tdt.Tipodocumento_Id
                        join est in _db.TipoEstado on d.Estado_Id equals est.Id
                        join eta in _db.TipoEtapa on d.Etapa_Id equals eta.Id
                        select new ListDocumentoPropietarioValidacion
                        {
                            Id = d.Id,
                            DocumentoVersion_Id = d.DocumentoVersion_Id,
                            Empresa_Id = p.Empresa_Id,
                            Trabajador_Id = p.Trabajador_Id,
                            TipoDocumento_Id = d.TipoDocumento_Id,                            
                            TipoDocumento = tdt.Descripcion,
                            FechaCreado = d.FechaCreado,
                            Nombre = p.Trabajador_Id.HasValue ? p.Trabajador.Nombre : p.Empresa.Nombre,
                            FechaCaducidad = d.FechaCaducidad,
                            Creador = d.Usuario.nombre,
                            Revisor = "",
                            Archivo = d.Fichero.nombre,
                            Fichero_id = d.Fichero.Id
                        });
            Expression<Func<ListDocumentoPropietarioValidacion, bool>> predicado_where_empresas = null;
            Expression<Func<ListDocumentoPropietarioValidacion, bool>> predicado_where_empleados = null;
            Expression<Func<ListDocumentoPropietarioValidacion, bool>> predicado_where_tdocumentos = null;
            if (_empresas != "")
            {
                predicado_where_empresas = PredicateBuilder.False<ListDocumentoPropietarioValidacion>();
                List<int> empresas_id = _empresas.Split(',').Select(int.Parse).ToList();
                foreach (int el in empresas_id)
                {
                    int temp = el;
                    predicado_where_empresas = predicado_where_empresas.Or(p => p.Empresa_Id == temp);
                }
            }
            if (_empleados != "")
            {
                predicado_where_empleados = PredicateBuilder.False<ListDocumentoPropietarioValidacion>();
                List<int> empleados_id = _empleados.Split(',').Select(int.Parse).ToList();
                foreach (int el in empleados_id)
                {
                    int temp = el;
                    predicado_where_empleados = predicado_where_empleados.Or(p => p.Trabajador_Id == temp);
                }
            }
            if (_tipodocumentos != "")
            {
                predicado_where_tdocumentos = PredicateBuilder.False<ListDocumentoPropietarioValidacion>();
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

                // Rellenar versiones
                // e.Row.Cells[4].Text es la celda de versiones
                string salida = "";
                int num_version = 0;
                ListDocumentoPropietarioValidacion item = e.Row.DataItem as ListDocumentoPropietarioValidacion;
                var data = (from d in _db.Documento
                            join est in _db.TipoEstado on d.Estado_Id equals est.Id
                            join eta in _db.TipoEtapa on d.Etapa_Id equals eta.Id
                            where d.DocumentoVersion_Id == item.DocumentoVersion_Id && d.Eliminado != true
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
                        if (esultimo)
                        {
                            salida += "<a class='" + getClassEstadoDocumento(doc) + "' id='docVersion_" + doc.Documento_Id.ToString() + "' >" + num_version.ToString() + ".0</a>&nbsp;";
                        }
                        else salida += "<a class='" + getClassEstadoDocumento(doc) + "'>" + num_version.ToString() + ".0</a>&nbsp;";
                    }
                    else
                    {
                        if (esultimo) {
                            salida += "<a class='" + getClassEstadoDocumento(doc) + "' id='docVersion_" + doc.Documento_Id.ToString() + "'>" + num_version.ToString() + ".0</a>&nbsp;";
                        }
                        else salida += "<a class='" + getClassEstadoDocumento(doc) + "'>" + num_version.ToString() + ".0</a>&nbsp;";
                    }

                }
                e.Row.Cells[4].Text = salida;

                //// Rellenar revisor
                //var doc = _db.Documento.Find(documento_id);
                //if (doc.Estado_Id == (int)TiposDeEstado.PUBLICADO) {
                //    e.Row.Cells[6].Text = Utilidades.OperacionesDocumentos.obtenerNombreUsuarioRevisor(Utilidades.Autentificacion.GetUsuarioID(), documento_id).mensaje;
                //} else {
                //    e.Row.Cells[6].Text = "<span id='docRevisor_" + documento_id.ToString() + "'></span>";
                //}
            }
        }
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

        protected void GridView1_DataBound(object sender, EventArgs e) {
            // Ocultar/mostrar la columnas adecuadas
            if (_tipoGrid == "Nombre") {
                GridView1.Columns[2].Visible = false;
                GridView1.Columns[3].Visible = true;
            } else {
                GridView1.Columns[2].Visible = true;
                GridView1.Columns[3].Visible = false;
            }
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