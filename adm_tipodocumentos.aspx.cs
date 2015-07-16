using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IBD.Web;

namespace Prestlan
{
    public partial class adm_tipodocumentos : System.Web.UI.Page
    {
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.VerAccesoAdministracion))
            {
                Response.Redirect("~/noautorizado.aspx");
            }

            // Traducciones
            ((Label)ListView1.FindControl("lblTiposdedocumentos")).Text = Traduce.getTG("tiposdedocumentos");
        }
        public IQueryable<ListTipoDocumento> GetData()
        {
            var data = (from td in _db.TipoDocumento
                        from tdt in _db.TipoDocumento_translation.Where(x => x.Idioma_codigo == "es" && x.Tipodocumento_Id == td.Id).DefaultIfEmpty()
                        select new ListTipoDocumento
                        {
                            Id = td.Id,
                            MultiplesPropietarios = td.MultiplesPropietarios,
                            NivelCriticidad = td.NivelCriticidad,
                            Idioma_codigo = "es",
                            Descripcion = tdt.Descripcion,
                            TextoAyuda = tdt.TextoAyuda,
                            Plantilla = tdt.Plantilla,
                            EsDocumentoDeEmpresa = td.EsDocumentoDeEmpresa
                        });
            return data.AsQueryable();
        }

        protected string procesarFichero(FileUpload f)
        {
            string filePath = "";
            if (f != null)
            {
                string path = Server.MapPath(ConfigurationManager.AppSettings["RepositorioPlantillas"].ToString());
                if (f.HasFile)
                {
                    String fileExtension = System.IO.Path.GetExtension(f.FileName).ToLower();
                    String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".pdf", ".doc", ".docx" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            filePath = path + "\\" + f.FileName;
                        }
                    }
                }
            }
            return filePath;
        }

        public void ListView1_InsertItem()
        {
            var ltd = new Prestlan.Models.ListTipoDocumento();
            var td = new Prestlan.Models.TipoDocumento();

            TryUpdateModel(ltd);

            if (ModelState.IsValid)
            {

                td.MultiplesPropietarios = ltd.MultiplesPropietarios;
                td.NivelCriticidad = ltd.NivelCriticidad;
                td.EsDocumentoDeEmpresa = ltd.EsDocumentoDeEmpresa;

                foreach (Prestlan.Models.Idiomas idioma in _db.Idiomas)
                {
                    var tdt = new Prestlan.Models.TipoDocumento_translation();
                    tdt.Descripcion = ltd.Descripcion;
                    tdt.Idioma_codigo = idioma.codigo;
                    tdt.Tipodocumento_Id = td.Id;
                    tdt.TextoAyuda = ltd.TextoAyuda;
                    td.TipoDocumento_translation.Add(tdt);
                }

                _db.TipoDocumento.Add(td);
                _db.SaveChanges();

            }
        }

        public void ListView1_UpdateItem(int id)
        {
            var td = _db.TipoDocumento.Find(id);
            var tdt = _db.TipoDocumento_translation.FirstOrDefault(x => x.Tipodocumento_Id == id && x.Idioma_codigo == "es");

            if (td == null)
            {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(td);
            if (ModelState.IsValid)
            {
                td.MultiplesPropietarios = ((CheckBox)ListView1.EditItem.FindControl("cbMultiplesPropietarios")).Checked;
                td.NivelCriticidad = Convert.ToInt32(((TextBox)ListView1.EditItem.FindControl("tbNivelCriticidad")).Text);
                td.EsDocumentoDeEmpresa = ((CheckBox)ListView1.EditItem.FindControl("chkEsDocumentoDeEmpresa")).Checked;
                tdt.Descripcion = ((TextBox)ListView1.EditItem.FindControl("tbDescripcion")).Text;
                tdt.TextoAyuda = ((TextBox)ListView1.EditItem.FindControl("tbTextoAyuda")).Text;
                _db.SaveChanges();
            }
        }

        // El nombre de parámetro del id. debe coincidir con el valor DataKeyNames establecido en el control
        public void ListView1_DeleteItem(int id)
        {
            // Comprobar quue ningún documento está apuntando a este tipo de documento a borrar
            var d = _db.Documento.FirstOrDefault(x => x.TipoDocumento_Id == id);
            if (d == null)
            {
                var td = _db.TipoDocumento.Find(id);
                if (td != null)
                {
                    if (!td.Requisitos.Any())
                    {
                        _db.TipoDocumento.Remove(td);
                        foreach (TipoDocumento_translation tdt in _db.TipoDocumento_translation.Where(x => x.Tipodocumento_Id == id))
                        {
                            _db.TipoDocumento_translation.Remove(tdt);
                        }
                        foreach (Requisitos r in td.Requisitos)
                        {
                            td.Requisitos.Remove(r);
                        }
                        _db.SaveChanges();
                    }
                    else
                    {
                        msgError.Text = "Error: Hay requisitos que utilizan este tipo de documento, por lo que no es posible borrarlo";
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

        protected void lbNuevo_Click(object sender, EventArgs e)
        {
            ListView1.InsertItemPosition = InsertItemPosition.FirstItem;
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

    }
}