using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
using System.Collections;
using Prestlan.Models;
using LinqKit;

namespace Prestlan
{
    public partial class list_historico : System.Web.UI.Page
    {
        protected Prestlan.Models.PRESTLANEntities _db = new Prestlan.Models.PRESTLANEntities();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // El tipo devuelto puede ser modificado a IEnumerable, sin embargo, para ser compatible con paginación y ordenación, se deben agregar los siguientes parametros:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable GridViewDocumentosVersiones_GetData(int maximumRows, int startRowIndex, out int totalRowCount, string sortByExpression)
        {
            //if (string.IsNullOrEmpty(sortByExpression)) sortByExpression = "Id";
            int empresa_id = ddlEmpresas.SelectedValue != null && !string.IsNullOrEmpty(ddlEmpresas.SelectedValue) ? int.Parse(ddlEmpresas.SelectedValue) : -1;
            int tipodocumento_id = ddlTipoDocumentos.SelectedValue != null && !string.IsNullOrEmpty(ddlTipoDocumentos.SelectedValue) ? int.Parse(ddlTipoDocumentos.SelectedValue) : -1;
            int estado_id = ddlEstado.SelectedValue != null && !string.IsNullOrEmpty(ddlEstado.SelectedValue) ? int.Parse(ddlEstado.SelectedValue) : -1;
            bool soloEliminados = chkElimiandos.Checked;
            sortByExpression = "DocumentoVersion_Id, FechaCreado";
            var data = (from doc in _db.Documento.Where(x => soloEliminados == false || x.Eliminado == true)
                        from p in doc.Propietario
                        join td in _db.TipoDocumento_translation.Where(x => x.Idioma_codigo == "es") on doc.TipoDocumento_Id equals td.Tipodocumento_Id
                        join est in _db.TipoEstado on doc.Estado_Id equals est.Id
                        join eta in _db.TipoEtapa on doc.Etapa_Id equals eta.Id
                        select new DocumentoHistorico
                        {
                            Id = doc.Id,
                            Titulo = doc.Titulo,
                            DocumentoVersion_Id = doc.DocumentoVersion_Id,
                            FechaCaducidad = doc.FechaCaducidad,
                            FechaCreado = doc.FechaCreado,
                            Estado = est.descripcion,
                            Estado_Id = est.Id,
                            Etapa = eta.descripcion,
                            TipoDocumento = td.Descripcion,
                            TipoDocumento_Id = td.Id,
                            Fichero_Id = doc.Fichero_Id,
                            Eliminado = doc.Eliminado,
                            Caduca = doc.Caduca,
                            Empresa_Id = p.Empresa_Id
                        }).Distinct().Where(x => (empresa_id == -1 || x.Empresa_Id == empresa_id) &&
                                                 (tipodocumento_id == -1 || x.TipoDocumento_Id == tipodocumento_id) &&
                                                 (estado_id == -1 || x.Estado_Id == estado_id));
            totalRowCount = data.Count();
            return data.OrderBy(sortByExpression).Skip(startRowIndex).Take(maximumRows);
        }

        int documento_version_id;
        int version;
        protected void GridViewDocumentosVersiones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DocumentoHistorico doc = e.Row.DataItem as DocumentoHistorico;
            if (doc != null)
            {
                if (documento_version_id == 0 || documento_version_id != doc.DocumentoVersion_Id.Value)
                {
                    version = 1;
                    documento_version_id = doc.DocumentoVersion_Id.Value;
                }
                else
                {
                    version = version + 1;
                }
                Literal lVersion = e.Row.Cells[1].FindControl("lVersion") as Literal;
                lVersion.Text = version.ToString();
            }
        }


        public List<ItemLista> GetEmpresas()
        {
            List<ItemLista> data = _db.Empresa.Where(x => x.Eliminado != true).Select(x => new ItemLista()
            {
                Id = x.Id,
                Descripcion = x.Nombre
            }).ToList<ItemLista>();

            data.Insert(0, new ItemLista() { Id = -1, Descripcion = "Seleccione..." });
            return data;
        }
        public List<ItemLista> GetTipoDocumentos()
        {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            List<ItemLista> data = _db.TipoDocumento_translation.Where(a => a.Idioma_codigo == idioma).Select(x => new ItemLista()
            {
                Id = x.Tipodocumento_Id,
                Descripcion = x.Descripcion
            }).ToList<ItemLista>();
            data.Insert(0, new ItemLista() { Id = -1, Descripcion = "Seleccione..." });
            return data;
        }
        public IList GetEstados()
        {
            List<ItemLista> data = _db.TipoEstado.Select(x => new ItemLista()
            {
                Id = x.Id,
                Descripcion = x.descripcion
            }).ToList<ItemLista>();
            data.Insert(0, new ItemLista() { Id = -1, Descripcion = "Seleccione..." });
            return data;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            GridViewDocumentosVersiones.DataBind();
        }
    }
    public class ItemLista
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }
    }
    public class DocumentoHistorico
    {

        public int Id { get; set; }

        public string Titulo { get; set; }

        public int? DocumentoVersion_Id { get; set; }

        public DateTime? FechaCaducidad { get; set; }

        public DateTime FechaCreado { get; set; }

        public string Estado { get; set; }

        public string Etapa { get; set; }

        public string TipoDocumento { get; set; }

        public int Fichero_Id { get; set; }

        public bool Eliminado { get; set; }

        public bool Caduca { get; set; }

        public int? Empresa_Id { get; set; }

        public int? Trabajador_Id { get; set; }

        public int TipoDocumento_Id { get; set; }

        public int Estado_Id { get; set; }
    }
}