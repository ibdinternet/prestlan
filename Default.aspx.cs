using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            int Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();
            if (Usuario_Id > 0) {
                asignarAccesos(Usuario_Id);
            } else {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
            HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("body1");
            body.Attributes.Add("class", "NoMenu"); 
        }
        protected void asignarAccesos(int Usuario_Id) {

            if (!IsPostBack)
            {
                // Documentos
                li_documentos.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoDocumentos);
            
                // Homologaciones
                //li_homologaciones.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.HomologarEmpresas) || Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.HomologarTrabajadores); ;
                li_homologaciones.Visible = false;

                // Administración
                li_administracion.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoAdministracion);
                        
                // Validación
                li_validaciones.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoValidaciones);

                // Reclamaciones
                li_reclamaciones.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoReclamaciones);

                // Trabajadores
                li_trabajadores.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoTrabajadores);

                // Subcontratas
                li_empresas.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoSubcontratas);

                // Generar zip
                li_generarzip.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoGenerarZIP);

                // requisitos
                li_requisitos.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoRequisitos);

                // Empresa (solo para subcontratas)
                li_edicionEmpresa.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoEmpresa);
                if (li_edicionEmpresa.Visible)
                {
                    using (PRESTLANEntities _db = new PRESTLANEntities())
                    {
                        Usuario user = _db.Usuario.FirstOrDefault(x => x.Id == Usuario_Id);
                        linkEdicionEmpresaMenu.NavigateUrl = String.Format("~/empresa.aspx?mode=Edit&id={0}", user.Empresa_Id);
                    }
                }
            }            
        }
    }
}