using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestlan.Utilidades;
using System.Data.Entity.Validation;

namespace Prestlan {
    public partial class Login : System.Web.UI.Page {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        

        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e) {
            int Usuario_Id = Autentificacion.Autenticar(Login1.UserName, Login1.Password);
            if (Usuario_Id > 0) {
                Session.Add("Usuario_Id", Usuario_Id);
                e.Authenticated = true;
            } else {
                log.Warn("Intento de autentificación fallido: " + Login1.UserName);
                e.Authenticated = false;
            }
        }

        protected void Login1_LoggedIn(object sender, EventArgs e) {
            // Gestionamos la detección del rol y el redirect a la página correspondiente
            if (Session["Usuario_Id"] != null) {
                int Usuario_Id = Convert.ToInt32(Session["Usuario_Id"].ToString());
                int Rol_Id = Autentificacion.GetRol(Usuario_Id);
                string NombreUsuario = Autentificacion.GetNombre(Usuario_Id);
                Session.Add("NombreUsuario", NombreUsuario);
                Session.Add("Idioma", Autentificacion.GetIdioma(Usuario_Id));
                // Actualizar la fecha del último login y dirección IP de la conexión
                Prestlan.Models.Usuario usuario = _db.Usuario.Find(Usuario_Id);
                usuario.ultimoLogin = DateTime.Now;
                usuario.ip = Utilidades.Tools.GetUser_IP();
                _db.SaveChanges();

                log.Info("Usuario autentificado: " + NombreUsuario + " (uid: " + Usuario_Id.ToString() + ")");

                // Distribuimos al usuario según su rol
                switch (Rol_Id) {
                    case 1: //ADMIN
                        Response.Redirect("Default.aspx");
                        break;
                    default:
                        Response.Redirect("Default.aspx");
                        break;
                }
            }

        }
    }
}