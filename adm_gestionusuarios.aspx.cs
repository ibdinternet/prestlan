using IBD.Web;
using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Prestlan
{
    public partial class adm_gestionusuarios : System.Web.UI.Page
    {
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.VerAccesoAdministracion))
            {
                Response.Redirect("~/noautorizado.aspx");
            }
        }

        public IQueryable<ListUsuario> ListView1_GetData()
        {
            var data = (from u in _db.Usuario
                        select new ListUsuario
                        {
                            Id = u.Id,
                            Nombre = u.nombre,
                            Email = u.email,
                            Clave = u.clave,
                            FechaAlta = u.fechaAlta,
                            FechaBaja = u.fechaBaja,
                            UltimoLogin = u.ultimoLogin,
                            Rol_Id = u.Rol_Id,
                            Rol = u.Rol.descripcion,
                            Empresa_Id = u.Empresa_Id,
                            Empresa = u.Empresa != null ? u.Empresa.Nombre : "",
                            Ip = u.ip
                        });
            return data.OrderBy(x => x.Empresa_Id).AsQueryable();
        }


        public void ListView1_UpdateItem(int id)
        {
            Prestlan.Models.Usuario usuario = _db.Usuario.Find(id);
            if (usuario == null)
            {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(usuario);
            if (ModelState.IsValid)
            {
                // Hay que manipular la clave para introducirla encryptada          
                string clave = ((TextBox)ListView1.EditItem.FindControl("tbClaveEdit")).Text;
                if (clave != "") usuario.clave = Utilidades.Autentificacion.CrearPassword(clave);
                _db.SaveChanges();

            }
        }

        public void ListView1_InsertItem()
        {
            var usuario = new Prestlan.Models.Usuario();
            TryUpdateModel(usuario);
            if (ModelState.IsValid)
            {
                string email = ((TextBox)ListView1.InsertItem.FindControl("tbEmail")).Text;
                if (Utilidades.Autentificacion.EsEmailUnico(email))
                {
                    // Hay que manipular la clave para introducirla encryptada
                    usuario.clave = Utilidades.Autentificacion.CrearPassword(((TextBox)ListView1.InsertItem.FindControl("tbClave")).Text);
                    usuario.idioma = "es";
                    usuario.fechaAlta = DateTime.Now;
                    _db.Usuario.Add(usuario);
                    _db.SaveChanges();
                }
                else
                {
                    //// ModelState.AddModelError("", String.Format("Error: El email <{0}> debe ser único.", email));
                    //msgError.Text = "Error: El email [" + email + "] debe ser único";
                    //msgError.Visible = true;
                    return;
                }
            }
            Response.Redirect("adm_gestionusuarios.aspx");
        }

        // El nombre de parámetro del id. debe coincidir con el valor DataKeyNames establecido en el control
        public void ListView1_DeleteItem(int id)
        {
            // Comprobar quue ningún documento está apuntando a este tipo de documento a borrar
            var d = _db.Documento.FirstOrDefault(x => x.TipoDocumento_Id == id);
            if (d == null)
            {
                var u = _db.Usuario.Find(id);
                if (u != null)
                {
                    if (!u.MovimientosDeDocumento.Any())
                    {
                        _db.Usuario.Remove(u);
                        _db.SaveChanges();
                    }
                    else
                    {
                        msgError.Text = "Error: Hay movimientos de documento de este usuario, por lo que no es posible borrarlo";
                        msgError.Visible = true;
                    }
                }
            }
            else
            {
                msgError.Text = "Error: Hay documentos que utilizan este tipo de documento, por lo que no es posible borrarlo";
                msgError.Visible = true;
            }
        }

        public IQueryable<Prestlan.Models.Rol> GetRoles()
        {
            return _db.Rol;
        }
        public IQueryable<Prestlan.Models.Empresa> GetEmpresas()
        {
            return _db.Empresa.OrderBy(x => x.TipoEmpresa_Id);
        }
        protected void lbNuevo_Click(object sender, EventArgs e)
        {
            ListView1.InsertItemPosition = InsertItemPosition.FirstItem;
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            ListView1.FindControl("thClave").Visible = (ListView1.EditIndex >= 0 || ListView1.InsertItemPosition == InsertItemPosition.FirstItem);
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlTableCell tdClave = (HtmlTableCell)e.Item.FindControl("tdClave");
                if (tdClave != null) tdClave.Visible = (ListView1.EditIndex >= 0 || ListView1.InsertItemPosition == InsertItemPosition.FirstItem);
            }
        }

        protected void ListView1_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            ListView1.InsertItemPosition = InsertItemPosition.None;
        }

        protected void ListView1_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView1.InsertItemPosition = InsertItemPosition.None;
        }

        protected void CancelInsert_Click(object sender, EventArgs e)
        {
            ListView1.InsertItemPosition = InsertItemPosition.None;
        }

        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {
            Label lbl = ListView1.FindControl("lblGestiondeusuarios") as Label;
            if (lbl != null)
            {
                lbl.Text = Traduce.getTG("gestiondeusuarios");
            }
        }

    }
}