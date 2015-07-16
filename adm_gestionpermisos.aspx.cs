using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class adm_gestionpermisos : System.Web.UI.Page {
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        protected void Page_Load(object sender, EventArgs e) {
            if (!Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.VerAccesoAdministracion)) {
                Response.Redirect("~/noautorizado.aspx");
            }
        }

        protected void ListView1_ItemEditing(object sender, ListViewEditEventArgs e) {
            ListView1.InsertItemPosition = InsertItemPosition.None;
        }

        protected void CancelInsert_Click(object sender, EventArgs e) {
            ListView1.InsertItemPosition = InsertItemPosition.None;
        }


        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e) {
            if (e.Item.ItemType == ListViewItemType.DataItem) {
                int id = Convert.ToInt32(ListView1.DataKeys[e.Item.DataItemIndex]["Id"].ToString());

                ((CheckBox)e.Item.FindControl("cbAdmin")).Checked = _db.Permiso.Any(x => x.Id == id && x.Rol.Any(r => r.descripcion == "ADMIN"));
                ((CheckBox)e.Item.FindControl("cbValidador")).Checked = _db.Permiso.Any(x => x.Id == id && x.Rol.Any(r => r.descripcion == "VALIDADOR"));
                ((CheckBox)e.Item.FindControl("cbSubcontrata")).Checked = _db.Permiso.Any(x => x.Id == id && x.Rol.Any(r => r.descripcion == "SUBCONTRATA"));
                ((CheckBox)e.Item.FindControl("cbFree")).Checked = _db.Permiso.Any(x => x.Id == id && x.Rol.Any(r => r.descripcion == "FREE"));

            }
        }

        public void ListView1_UpdateItem(int id) {
            var permiso = _db.Permiso.Find(id);

            if (permiso == null) {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(permiso);
            if (ModelState.IsValid) {
                // Quitamos todos los permisos para cada rol
                foreach (var r in _db.Rol) {
                    permiso.Rol.Remove(r);
                }

                // Añadimos de nuevo los seleccionados                

                if (((CheckBox)ListView1.EditItem.FindControl("cbAdmin")).Checked) {
                    var r = _db.Rol.FirstOrDefault(x => x.descripcion == "ADMIN");
                    permiso.Rol.Add(r);
                }
                if (((CheckBox)ListView1.EditItem.FindControl("cbValidador")).Checked) {
                    var r = _db.Rol.FirstOrDefault(x => x.descripcion == "VALIDADOR");
                    permiso.Rol.Add(r);
                }
                if (((CheckBox)ListView1.EditItem.FindControl("cbSubcontrata")).Checked) {
                    var r = _db.Rol.FirstOrDefault(x => x.descripcion == "SUBCONTRATA");
                    permiso.Rol.Add(r);
                }
                if (((CheckBox)ListView1.EditItem.FindControl("cbFree")).Checked) {
                    var r = _db.Rol.FirstOrDefault(x => x.descripcion == "FREE");
                    permiso.Rol.Add(r);
                }

                _db.SaveChanges();

            }
        }

        public IQueryable<Prestlan.Models.ListPermisos> ListView1_GetData() {
            var data = (from p in _db.Permiso
                        select new ListPermisos {
                            Id = p.Id,
                            Descripcion = p.descripcion,
                            Rol = p.Rol.FirstOrDefault().descripcion,
                            Rol_Id = p.Rol.FirstOrDefault().Id
                        });
            return data.AsQueryable();
        }

        protected void lbSalvarPermisos_Click(object sender, EventArgs e) {

        }
    }
}