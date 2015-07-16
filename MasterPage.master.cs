using IBD.Web;
using Prestlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prestlan {
    public partial class MasterPage : System.Web.UI.MasterPage {

        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        protected void Page_Load(object sender, EventArgs e) {

            if (Session["NombreUsuario"] != null) {
                lblNombreUsuario.Text = Session["NombreUsuario"].ToString();
            }
            // Traducciones
            tbBuscarDocumentos.Text = Traduce.getTG("buscardocumentos");
            if (!IsPostBack)
            {
                MostrarOcultarElementosMenu();
            }            
        }
               
        protected void bLogout_Click(object sender, EventArgs e) {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        public IQueryable<Prestlan.Models.Trabajador> GetTrabajadores() {
            if (!Page.IsPostBack) {
                return _db.Trabajador;
            } else return null;
        }
        public IQueryable<Prestlan.Models.TipoDocumento_translation> GetTipoDocumentos() {
            string idioma = "es";
            if (Session["Idioma"] != null) {
                idioma = Session["Idioma"].ToString();
            }
            if (!Page.IsPostBack) {
                return _db.TipoDocumento_translation.Where(a => a.Idioma_codigo == idioma);
            } else return null;
        }
        public IQueryable<Prestlan.Models.Actividad_translation> GetActividades() {
            string idioma = "es";
            if (Session["Idioma"] != null) {
                idioma = Session["Idioma"].ToString();
            }
            if (!Page.IsPostBack) {
                return _db.Actividad_translation.Where(a => a.Idioma_codigo == idioma);
            } else return null;
        }
        public IQueryable<Prestlan.Models.Empresa> GetEmpresas() {
            if (!Page.IsPostBack) {
                return _db.Empresa.OrderBy(x => x.TipoEmpresa_Id);
            } else return null;
        }

        protected void MostrarOcultarElementosMenu()
        {
            int Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();
            if (Usuario_Id > 0)
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
                li_subcontratas.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoSubcontratas);

                // Generar zip
                li_generar_zip.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoGenerarZIP);

                // requisitos
                li_requisitos.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoRequisitos);

                // Empresa (solo para subcontratas)
                li_edicionEmpresa.Visible = Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.VerAccesoEmpresa);
                if (li_edicionEmpresa.Visible)
                {
                    using(PRESTLANEntities _db =  new PRESTLANEntities())
                    {
                        Usuario user = _db.Usuario.FirstOrDefault(x => x.Id == Usuario_Id);
                        linkEdicionEmpresaMenu.PostBackUrl = String.Format("~/empresa.aspx?mode=Edit&id={0}", user.Empresa_Id);
                    }                    
                }
            }
        }
    }
}