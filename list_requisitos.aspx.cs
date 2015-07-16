using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using Prestlan.Models;
namespace Prestlan
{
    public partial class list_requisitos : System.Web.UI.Page
    {
        private GridViewHelper _helper;
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (PermisosOK())
                {
                    BindGrid();
                }
                else
                {
                    Response.Redirect("~/noAutorizado.aspx");
                }                
            }
            else
            {
                ComprobarPostBack();
            }
        }
        private bool PermisosOK()
        {
            int usuarioId = Utilidades.Autentificacion.GetUsuarioID();
            return Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.RequisitosListar);
            //Permitir edicion o no segun permiso se comprueba en getData y el js
        }
        private void ComprobarPostBack()
        {
            string ctrlname = Request.Form["__EVENTTARGET"];
            if (ctrlname == "EliminarRequisito")
            {
                string argument = Request.Form["__EVENTARGUMENT"];
                EliminarRequisito(int.Parse(argument));
            }
        }

        private void EliminarRequisito(int id)
        {
            var requisito = _db.Requisitos.Include(x => x.TipoDocumento).FirstOrDefault(x => x.Id == id);
            _db.Entry(requisito).State = EntityState.Deleted;

            foreach (TipoDocumento item in requisito.TipoDocumento)
            {
                _db.Entry(item).State = EntityState.Deleted;
            }
            _db.SaveChanges();
            BindGrid();
        }

        protected void agrupaGrid()
        {
            // Para hacer group de columnas
            _helper = new GridViewHelper(GridViewRequisitos, false);
            _helper.RegisterGroup("Requisito", true, true);
            _helper.ApplyGroupSort();
        }

        public void BindGrid()
        {
            agrupaGrid();
            var data = getData();

            if (data != null && data.Length > 0)
            {
                GridViewRequisitos.DataSource = data;
            }
            else
            {
                GridViewRequisitos.DataSource = null;
            }
            GridViewRequisitos.DataBind();

        }

        protected Array getData()
        {
            int usuarioId = Utilidades.Autentificacion.GetUsuarioID();
            bool permisoEdicion = Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.RequisitosEditar);
            string idioma = Session["Idioma"].ToString();
            return (from req in _db.Requisitos.Where(t => t.TipoDocumento.Any())
                    from td in _db.TipoDocumento.Where(r => r.Requisitos.Contains(req))
                    select new
                    {
                        RequisitoId = req.Id,
                        Requisito = req.descripcion + "<span style='float:right' data-requisito-id='" + (permisoEdicion == true ? req.Id : 0) + "' class='jsBotoneraRequisito'></span>",
                        TipoDocumentoId = td.Id,
                        TipoDocumento = td.TipoDocumento_translation.FirstOrDefault(x => x.Idioma_codigo == idioma).Descripcion,
                        EsDocumentoDeEmpresa = td.EsDocumentoDeEmpresa
                    }).ToArray();
        }

        protected void GridViewRequisitos_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void GridViewRequisitos_PreRender(object sender, EventArgs e)
        {

        }
    }
}