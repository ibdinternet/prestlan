using IBD.Web;
using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class adm_tipotrabajador : System.Web.UI.Page {
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        protected void Page_Load(object sender, EventArgs e) {
            if (!Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.VerAccesoAdministracion))
            {
                Response.Redirect("~/noautorizado.aspx");
            }
        }

        public void ListView1_InsertItem() {
            var tt = new Prestlan.Models.TipoTrabajador();
            var ttt = new Prestlan.Models.TipoTrabajador_translation();

            TryUpdateModel(tt);
            if (ModelState.IsValid) {
                ttt.Descripcion = ((TextBox)ListView1.InsertItem.FindControl("tbDescripcion")).Text;
                tt.Activo = ((CheckBox)ListView1.InsertItem.FindControl("cbActivo")).Checked;
                _db.TipoTrabajador.Add(tt);
                ttt.TipoTrabajador_Id = tt.Id;
                ttt.Idioma_codigo = "es";
                _db.TipoTrabajador_translation.Add(ttt);
                _db.SaveChanges();
            }
        }

        // El nombre de parámetro del id. debe coincidir con el valor DataKeyNames establecido en el control
        public void ListView1_UpdateItem(int id) {
            var tt = _db.TipoTrabajador.Find(id);
            var ttt = _db.TipoTrabajador_translation.FirstOrDefault(x => x.TipoTrabajador_Id == id);

            if (tt == null) {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(tt);
            if (ModelState.IsValid) {
                tt.Activo = ((CheckBox)ListView1.EditItem.FindControl("cbActivo")).Checked;
                ttt.Descripcion = ((TextBox)ListView1.EditItem.FindControl("tbDescripcion")).Text;
                _db.SaveChanges();
            }
        }

        public void ListView1_DeleteItem(int id) {
            // Comprobar quue ningún trabajador está apuntando a este tipo de trabajador a borrar
            var t = _db.Trabajador.FirstOrDefault(x => x.TipoTrabajador_Id == id);
            if (t == null) {
                var tt = _db.TipoTrabajador.Find(id);
                var ttt = _db.TipoTrabajador_translation.FirstOrDefault(x => x.TipoTrabajador_Id == id);
                if (ttt != null) _db.TipoTrabajador_translation.Remove(ttt);
                if (tt != null) _db.TipoTrabajador.Remove(tt);
                _db.SaveChanges();
            } else {
                msgError.Text = "Error: Hay trabajadores que utilizan este tipo de trabajador, por lo que no es posible borrarlo";
                msgError.Visible = true;
            }
        }

        public IQueryable<ListTipoTrabajador> ListView1_GetData() {
            string idioma = "es";
            if (Session["Idioma"] != null) {
                idioma = Session["Idioma"].ToString();
            }
            var data = (from t in _db.TipoTrabajador
                        where t.TipoTrabajador_translation.FirstOrDefault().Idioma_codigo == idioma
                        select new ListTipoTrabajador {
                            Id = t.Id,
                            Descripcion = t.TipoTrabajador_translation.FirstOrDefault().Descripcion,
                            Activo = t.Activo
                        });
            return data.AsQueryable();
        }

        protected void lbNuevo_Click(object sender, EventArgs e) {
            ListView1.InsertItemPosition = InsertItemPosition.FirstItem;
        }

        protected void ListView1_ItemInserted(object sender, ListViewInsertedEventArgs e) {
            ListView1.InsertItemPosition = InsertItemPosition.None;
        }

        protected void ListView1_ItemEditing(object sender, ListViewEditEventArgs e) {
            ListView1.InsertItemPosition = InsertItemPosition.None;
        }

        protected void CancelInsert_Click(object sender, EventArgs e) {
            ListView1.InsertItemPosition = InsertItemPosition.None;
        }

        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {
            Label lbl = ListView1.FindControl("lblTiposdetrabajadores") as Label;
            if (lbl != null)
            {
                lbl.Text = Traduce.getTG("tiposdetrabajadores");
            }
        }


    }
}