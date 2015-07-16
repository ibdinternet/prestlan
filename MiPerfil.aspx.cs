using IBD.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class MiPerfil : System.Web.UI.Page {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        protected void Page_Load(object sender, EventArgs e) {
            lblVersion.Text = ConfigurationManager.AppSettings["VersionAplicacion"].ToString();
        }
        
        public Prestlan.Models.Usuario FormView1_GetItem() {
            int Usuario_id = Utilidades.Autentificacion.GetUsuarioID();
            log.Debug("Consulta de perfil: uid " + Usuario_id.ToString());
            return _db.Usuario.Find(Usuario_id);
        }

        
        public void FormView1_UpdateItem(int id) {
            Prestlan.Models.Usuario usuario = _db.Usuario.Find(id);

            if (usuario == null) {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(usuario);
            if (ModelState.IsValid) {
                string clave = ((TextBox)FormView1.FindControl("tbClave")).Text;
                if (clave != "") {
                    usuario.clave = Utilidades.Autentificacion.CrearPassword(clave);
                }
                _db.SaveChanges();

                Response.Redirect("Default.aspx");
            }
        }
      
    }
}