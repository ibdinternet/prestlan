using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class MasterPage_previous : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (Session["NombreUsuario"] != null) {
                lblNombreUsuario.Text = Session["NombreUsuario"].ToString();
            }
        }
        protected void bLogout_Click(object sender, EventArgs e) {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}