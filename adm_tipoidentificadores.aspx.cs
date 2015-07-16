using IBD.Web;
using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace Prestlan {
    public partial class adm_tipoidentificadores : System.Web.UI.Page {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        protected void Page_Load(object sender, EventArgs e) {
            if (!Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.VerAccesoAdministracion))
            {
                Response.Redirect("~/noautorizado.aspx");
            }
        }

        public void ListView1_InsertItem() {
            var ti = new Prestlan.Models.TipoIdentificador();           

            TryUpdateModel(ti);
            if (ModelState.IsValid) {
                foreach (Prestlan.Models.Idiomas idioma in _db.Idiomas)
                {
                    var tit = new Prestlan.Models.Tipoidentificador_translation();
                    tit.Descripcion = ((TextBox)ListView1.InsertItem.FindControl("tbDescripcion")).Text;
                    tit.Idioma_codigo = idioma.codigo;
                    ti.Tipoidentificador_translation.Add(tit);
                }
                _db.TipoIdentificador.Add(ti);  
                _db.SaveChanges();
            }
        }

        
        public void ListView1_UpdateItem(int id) {
            Prestlan.Models.TipoIdentificador ti = _db.TipoIdentificador.Find(id);
            Prestlan.Models.Tipoidentificador_translation tit = _db.Tipoidentificador_translation.FirstOrDefault(x => x.Tipoidentificador_Id == id && x.Idioma_codigo == "es");

            if (ti == null) {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(ti);
            if (ModelState.IsValid) {
                tit.Descripcion = ((TextBox)ListView1.EditItem.FindControl("tbDescripcion")).Text;
                _db.SaveChanges();
            }
        }


        public IQueryable<ListTipoIdentificador> ListView1_GetData() {
            var data = (from t in _db.TipoIdentificador
                        from tt in _db.Tipoidentificador_translation.Where(x => x.Idioma_codigo == "es" && x.Tipoidentificador_Id == t.Id).DefaultIfEmpty()
                        select new ListTipoIdentificador
                        {
                            Id = t.Id,
                            Descripcion = tt != null ? tt.Descripcion : ""
                        });
            return data.AsQueryable();

            //Carga los datos del idioma seleccionado y si no hay el español
            //string idioma = "es";
            //if (Session["Idioma"] != null)
            //{
            //    idioma = Session["Idioma"].ToString();
            //}
            //var data = (from t in _db.TipoIdentificador
            //            from tt in _db.Tipoidentificador_translation.Where(x => x.Idioma_codigo == idioma && x.Tipoidentificador_Id == t.Id).DefaultIfEmpty()
            //            select new ListTipoIdentificador
            //            {
            //                Id = t.Id,
            //                Descripcion = tt != null ? tt.Descripcion : t.Tipoidentificador_translation.FirstOrDefault(x => x.Idioma_codigo == "es").Descripcion
            //            });
            //return data.AsQueryable();
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

        
        public void ListView1_DeleteItem(int id) {
            // Comprobar que ningún trabajador ni ninguna empresa está apuntando a este tipo de identificador a borrar
            var e = _db.Empresa.FirstOrDefault(x => x.TipoIdentificador_Id == id);
            var t = _db.Trabajador.FirstOrDefault(x => x.TipoIdentificador_Id == id);
            if (t == null && e == null) {
                var ti = _db.TipoIdentificador.Find(id);
                if (ti != null)
                {
                    _db.TipoIdentificador.Remove(ti);
                    foreach (Tipoidentificador_translation item in _db.Tipoidentificador_translation.Where(x => x.Tipoidentificador_Id == ti.Id))
                    {
                        _db.Tipoidentificador_translation.Remove(item);
                    }
                    _db.SaveChanges();
                }
            } else {
                msgError.Text = "Error: Hay trabajadores o empresas que utilizan este tipo de identificador, por lo que no es posible borrarlo";
                msgError.Visible = true;
            }
        }

        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {
            Label lbl = ListView1.FindControl("lblTiposdeifentificadores") as Label;
            if (lbl != null)
            {
                lbl.Text = Traduce.getTG("tiposdeidentificadores");
            }
        }              
    }
}