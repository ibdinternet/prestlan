using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class homologaciones : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.VerAccesoHomologaciones))
            {
                Response.Redirect("~/noAutorizado.aspx");
            }
        }
    }
}