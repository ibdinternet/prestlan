using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class requisitos : System.Web.UI.Page {
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack)
            {
                if (PermisosOK())
                {
                    inicializarAsignaciones();
                }      
                else
                {
                    Response.Redirect("~/noAutorizado.aspx");
                }
            }
        }
        private bool PermisosOK()
        {
            return Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.RequisitosEditar);
        }
        protected void inicializarAsignaciones()
        {
            if (Request.QueryString["mode"] != null)
            {
                string mode = Request.QueryString["mode"].ToString();
                if (mode == "Edit")
                {
                    int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                    Prestlan.Models.Requisitos requisito = _db.Requisitos.Find(id);
                    ListBox lbTiposDocumentos = (ListBox)FormView1.FindControl("lbTipoDocumentoRequisito");
                    foreach (Prestlan.Models.TipoDocumento tdoc in requisito.TipoDocumento)
                    {
                        foreach (ListItem li in lbTiposDocumentos.Items)
                        {
                            if (li.Value == tdoc.Id.ToString())
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        // El parámetro del id. debe coincidir con el valor DataKeyNames establecido en el control
        // o ser representado con un atributo proveedor de valor, por ejemplo [QueryString]int id
        public Prestlan.Models.Requisitos FormView1_GetItem([QueryString]int? Id)
        {
            if (Id.HasValue)
            {
                Prestlan.Models.Requisitos requisito = _db.Requisitos.Find(Id.Value);
                return requisito;
            }
            return null;
        }

        public void FormView1_UpdateItem(int id) {
            Prestlan.Models.Requisitos requisito = _db.Requisitos.Find(id);
            if (requisito == null) {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(requisito);
            if (ModelState.IsValid) {

                // Hay que eliminar todos los tipos de documentos previamente asignados.
                requisito.TipoDocumento.ToList().ForEach(r => requisito.TipoDocumento.Remove(r));                

                var selectedTd = from ListItem i in ((ListBox)FormView1.FindControl("lbTipoDocumentoRequisito")).Items where i.Selected select i;
                foreach (var t in selectedTd) {
                    var td = _db.TipoDocumento.Find(int.Parse(t.Value));
                    td.Requisitos.Add(requisito);
                }
                _db.SaveChanges();
                Response.Redirect("list_requisitos.aspx");
            }
        }

        public void FormView1_InsertItem() {
            using (_db) {
                var requisito = new Prestlan.Models.Requisitos();
                TryUpdateModel(requisito);
                 if (ModelState.IsValid) {
                    var selectedTd = from ListItem i in ((ListBox)FormView1.FindControl("lbTipoDocumentoRequisito")).Items where i.Selected select i;
                    foreach (var t in selectedTd) {                        
                        var td = _db.TipoDocumento.Find(int.Parse(t.Value));
                        td.Requisitos.Add(requisito);                                                
                    }
                    _db.SaveChanges();                    
                    Response.Redirect("list_requisitos.aspx");
                 }
            }
        }

        public IQueryable<Prestlan.Models.TipoDocumento_translation> GetTipoDocumento() {
            if (!Page.IsPostBack) {
                string idioma = "es";
                if (Session["Idioma"] != null) {
                    idioma = Session["Idioma"].ToString();
                }
                return _db.TipoDocumento_translation.Where(a => a.Idioma_codigo == idioma);
            } else return null;
        }

        protected void FormView1_Init(object sender, EventArgs e) {
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

        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("list_requisitos.aspx");
            }
        }
    }
}