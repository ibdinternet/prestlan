using IBD.Web;
using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan
{
    public partial class empresa : System.Web.UI.Page
    {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        /// <summary>
        /// Solo puede editar algunos campos
        /// </summary>
        public bool ModoEdicionSubcontrata { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (PermisosOK())
                {

                }
                else
                {
                    Response.Redirect("~/noautorizado.aspx");
                }
            }
        }
        private bool PermisosOK()
        {
            bool result = false;
            ModoEdicionSubcontrata = false;
            int Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();
            if (Usuario_Id > 0)
            {
                if (Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.EmpresasEditarTodo))
                {
                    result = true;
                }
                else if (Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.EmpresasEditarSoloEmpresa))
                {
                    if (Request.QueryString["mode"] != null)
                    {
                        string mode = Request.QueryString["mode"].ToLower();
                        if (mode == "edit")
                        {
                            result =  Utilidades.Autentificacion.EsEmpresaUsuario(int.Parse(Request.QueryString["id"].ToString()));
                            if (result == true)
                            {
                                ModoEdicionSubcontrata = true;
                            }
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
        protected override void LoadViewState(object savedState)
        {
            Dictionary<string, object> viewState = (Dictionary<string, object>)savedState;
            base.LoadViewState(viewState["base"]);
            ModoEdicionSubcontrata = (bool)viewState["ModoEdicionSubcontrata"];
        }

        protected override object SaveViewState()
        {
            Dictionary<string, object> viewState = new Dictionary<string, object>();
            viewState.Add("base", base.SaveViewState());
            viewState.Add("ModoEdicionSubcontrata", ModoEdicionSubcontrata);
            return viewState;
        }

        protected void FormView1_Init(object sender, EventArgs e)
        {
            if (Request.QueryString["mode"] != null)
            {
                string mode = Request.QueryString["mode"].ToString();
                if (mode == "Read")
                {
                    FormView1.ChangeMode(FormViewMode.ReadOnly);
                }
                else if (mode == "Edit")
                {
                    FormView1.ChangeMode(FormViewMode.Edit);
                }
                else if (mode == "Insert")
                {
                    FormView1.ChangeMode(FormViewMode.Insert);
                }
            }
        }

        private void DeshabilitarCamposEdicionSubcontrata()
        {

        }

        public void FormView1_InsertItem()
        {
            using (_db)
            {
                var item = new Prestlan.Models.Empresa();

                TryUpdateModel(item);

                if (ModelState.IsValid)
                {
                    item.Activa = true;
                    item.FechaAlta = DateTime.Now;

                    _db.Empresa.Add(item);

                    try
                    {
                        _db.SaveChanges();
                        msgEstado.Text = Traduce.getTG("empresainsertok");
                        panelEstado.Visible = true;
                        FormView1.Visible = false;
                    }
                    catch (DbEntityValidationException ex)
                    {
                        msgError.Text = Utilidades.Errores.AnotarEntityValidationException(ex);
                        panelError.Visible = true;
                    }

                }
            }
        }

        protected void ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("list_empresa.aspx");
            }
        }
        public IQueryable<Prestlan.Models.Actividad_translation> GetActividades()
        {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            return _db.Actividad_translation.Where(a => a.Idioma_codigo == idioma);
        }
        public IQueryable<Prestlan.Models.Tipoidentificador_translation> GetTipoidentificador()
        {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            var x = _db.Tipoidentificador_translation.Where(a => a.Idioma_codigo == idioma);
            return x;
        }
        public IQueryable<Prestlan.Models.TipoEmpresa_translation> GetTipoEmpresa()
        {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            return _db.TipoEmpresa_translation.Where(a => a.Idioma_codigo == idioma);
        }

        public Prestlan.Models.Empresa FormView1_GetItem([QueryString]int? Id)
        {
            if (Id.HasValue)
            {
                return _db.Empresa.First(e => e.Id == Id.Value);
            }
            return null;
        }

        public void FormView1_UpdateItem(int Id)
        {
            Prestlan.Models.Empresa item = _db.Empresa.Find(Id);
            if (item == null)
            {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", Id));
                return;
            }
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                try
                {
                    _db.SaveChanges();
                    msgEstado.Text = Traduce.getTG("empresaeditok");
                    panelEstado.Visible = true;
                    FormView1.Visible = false;
                }
                catch (DbEntityValidationException ex)
                {
                    msgError.Text = Utilidades.Errores.AnotarEntityValidationException(ex);
                    panelError.Visible = true;
                }
            }
        }

    }
}