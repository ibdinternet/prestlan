using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using IBD.Web;

namespace Prestlan {
    public partial class trabajador : System.Web.UI.Page {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                if (PermisosOK())
                {
                    if (Request.QueryString["mode"] != null) {
                        string mode = Request.QueryString["mode"].ToString();
                        if (mode == "Edit") {
                            // No se puede cambiar el trabajador de empresa                        

                            ListBox lbActividades = FormView1.FindControl("lbActividadesTrabajador") as ListBox;
                            if (lbActividades != null)
                            {
                                int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                                Prestlan.Models.Trabajador trabajador = _db.Trabajador.Find(id);
                                foreach (int i in trabajador.Actividad.Select(a => a.Id)) {
                                    foreach (ListItem li in lbActividades.Items)
                                    {
                                        if (li.Value == i.ToString()) {
                                            li.Selected = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
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
            int usuarioId = Utilidades.Autentificacion.GetUsuarioID();
            if (usuarioId > 0)
            {
                if (Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.TrabajadoresEditarTodo))
                {
                    result = true;
                }
                else if (Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.TrabajadoresEditarSoloEmpresa))
                {
                    if (Request.QueryString["mode"] != null)
                    {
                        string mode = Request.QueryString["mode"].ToLower();
                        if (mode == "edit")
                        {
                            result =  Utilidades.Autentificacion.EsTrabajadorEmpresaUsuario(int.Parse(Request.QueryString["id"].ToString()));
                        }
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public Prestlan.Models.Trabajador FormView1_GetItem([QueryString]int? id) {
            if (id.HasValue) {                
                return _db.Trabajador.Find(id.Value);
            }
            return null;
        }

        public void FormView1_UpdateItem(int id) {
            Prestlan.Models.Trabajador trabajador = _db.Trabajador.Find(id);            
            if (trabajador == null) {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(trabajador);
            if (ModelState.IsValid) {

                //if (((DropDownList)FormView1.FindControl("ddEmpresa")).SelectedValue != "-1") { // Ajeno
                //    trabajador.TipoTrabajador_Id = 2;
                //    trabajador.Empresa_Id = Convert.ToInt32(((DropDownList)FormView1.FindControl("ddEmpresa")).SelectedValue);
                //} else { // Propio
                //    trabajador.TipoTrabajador_Id = 1;
                //    trabajador.Empresa_Id = Utilidades.Autentificacion.GetEmpresaID();
                //}

                trabajador.Actividad.ToList().ForEach(r => trabajador.Actividad.Remove(r));
                var selectedActividades = from ListItem i in ((ListBox)FormView1.FindControl("lbActividadesTrabajador")).Items where i.Selected select i;
                foreach (var p in selectedActividades) {
                    var actividad = _db.Actividad.Find(int.Parse(p.Value));
                    trabajador.Actividad.Add(actividad);
                }
               
                try {
                    _db.SaveChanges();
                    msgEstado.Text = Traduce.getTG("trabajadoreditok");
                    panelEstado.Visible = true;
                    FormView1.Visible = false;
                } catch (DbEntityValidationException ex) {
                    msgError.Text = Utilidades.Errores.AnotarEntityValidationException(ex);
                    panelError.Visible = true;
                } 

            }
        }

        protected void FormView1_Init(object sender, EventArgs e) {
            if (Request.QueryString["mode"] != null) {
                string mode = Request.QueryString["mode"].ToString();
                if (mode == "Read") {
                    FormView1.ChangeMode(FormViewMode.ReadOnly);
                } else if (mode == "Edit") {
                    FormView1.ChangeMode(FormViewMode.Edit);
                } else if (mode == "Insert") {
                    FormView1.ChangeMode(FormViewMode.Insert);
                }
            }
        }

        public void FormView1_InsertItem() {
            using (_db) {
                var trabajador = new Prestlan.Models.Trabajador();
                TryUpdateModel(trabajador);
                if (ModelState.IsValid) {
                    if (((DropDownList)FormView1.FindControl("ddEmpresa")).SelectedValue != "-1") { // Ajeno
                        trabajador.TipoTrabajador_Id = 2;
                        trabajador.Empresa_Id = Convert.ToInt32(((DropDownList)FormView1.FindControl("ddEmpresa")).SelectedValue);                                       
                    } else { // Propio
                        trabajador.TipoTrabajador_Id = 1;
                        trabajador.Empresa_Id = Utilidades.Autentificacion.GetEmpresaID();
                    }
                    //int tipoTrabajador = Convert.ToInt32(((RadioButtonList)FormView1.FindControl("rbTrabajador")).SelectedValue);
                    //trabajador.TipoTrabajador_Id = Convert.ToInt32(((RadioButtonList)FormView1.FindControl("rbTrabajador")).SelectedValue);
                    //trabajador.Empresa_Id = (tipoTrabajador == 1) ? Convert.ToInt32(Session["ID_empresa_Actual"].ToString()) : Convert.ToInt32(((DropDownList)FormView1.FindControl("ddEmpresa")).SelectedValue);                                       

                    trabajador.Autonomo = ((CheckBox)FormView1.FindControl("cbEmpleado")).Checked;

                    trabajador.FechaAlta = DateTime.Now;
                    trabajador.FechaBaja = DateTime.MaxValue;
                    trabajador.Activo = true;                                                            
                    
                    var selectedActividades = from ListItem i in ((ListBox)FormView1.FindControl("lbActividadesTrabajador")).Items where i.Selected select i;
                    foreach (var p in selectedActividades) {
                        var actividad = _db.Actividad.Find(int.Parse(p.Value));
                        trabajador.Actividad.Add(actividad);                            
                    }
                    _db.Trabajador.Add(trabajador);
                    try {                        
                        _db.SaveChanges();
                        msgEstado.Text = Traduce.getTG("trabajadorinsertok");
                        panelEstado.Visible = true;
                        FormView1.Visible = false;
                    } catch (DbEntityValidationException ex) {
                        msgError.Text = Utilidades.Errores.AnotarEntityValidationException(ex);
                        panelError.Visible = true;
                    }

                }
            }
        }

        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e) {
            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase)) {
                Response.Redirect("~/list_trabajador.aspx");
            }

        }

        public IQueryable<Prestlan.Models.Empresa> GetEmpresas() {
            int Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();
            Usuario user = _db.Usuario.Find(Usuario_Id);
            if (Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.TrabajadoresEditarTodo))
            {
                return _db.Empresa.Where(x => x.Eliminado != true).OrderBy(x => x.TipoEmpresa_Id);
            }
            else if (Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.TrabajadoresEditarSoloEmpresa))
            {
                return _db.Empresa.Where(x => x.Eliminado != true).OrderBy(x => x.TipoEmpresa_Id).Where(x => x.Id == user.Empresa_Id);
            }
            else
            {
                return null;
            }
        }
        public IQueryable<Prestlan.Models.Tipoidentificador_translation> GetTipoidentificador() {
            string idioma = "es";
            if (Session["Idioma"] != null) {
                idioma = Session["Idioma"].ToString();
            }
            return _db.Tipoidentificador_translation.Where(a => a.Idioma_codigo == idioma);
        }
        public List<ListTipoTrabajador> GetTipoTrabajador() {
            string idioma = "es";
            if (Session["Idioma"] != null) {
                idioma = Session["Idioma"].ToString();
            }
            return (from t in _db.TipoTrabajador
                    where t.TipoTrabajador_translation.FirstOrDefault().Idioma_codigo == idioma
                        select new ListTipoTrabajador {
                            Id = t.Id,
                            Descripcion = t.TipoTrabajador_translation.FirstOrDefault().Descripcion
                        }).ToList();            
        }
        public List<ListActividades> GetActividades() {
            string idioma = "es";
            if (Session["Idioma"] != null) {
                idioma = Session["Idioma"].ToString();
            }
            List<ListActividades> collection = _db.Actividad_translation.Where(x => x.Idioma_codigo == idioma).Select(a => new ListActividades
                                                {
                                                    Id = a.Actividad_Id,
                                                    Descripcion = a.Descripcion
                                                }).ToList();
            return collection;             
        }
    }
}