using System;
using System.Linq;
using System.Web.ModelBinding;
using System.Web.UI;
using Prestlan.Models;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO.Compression;
using System.IO;
using System.Data.Entity;
using LinqKit;
using System.Text;
using System.Configuration;

namespace Prestlan
{
    public partial class generar_zip : System.Web.UI.Page
    {
        protected PRESTLANEntities _db = new PRESTLANEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (PermisosOK())
                {
                    List<EmpresaTrabajador> data = new List<EmpresaTrabajador>();
                    data.Add(new EmpresaTrabajador(GetEmpresas().FirstOrDefault().Id));
                    Repeater1.DataSource = data;
                    Repeater1.DataBind();
                }
            }
        }
        private bool PermisosOK()
        {
            return Utilidades.Autentificacion.comprobarPermiso(Utilidades.Autentificacion.GetUsuarioID(), TipoPermiso.VerAccesoGenerarZIP);
        }
        public IQueryable<Prestlan.Models.Requisitos> lbRequisitos_GetData()
        {
            return _db.Requisitos;
        }

        public IQueryable<Prestlan.Models.Empresa> GetEmpresas()
        {
            var data = _db.Empresa.Where(x => x.Eliminado != true).OrderBy(x => x.TipoEmpresa_Id);
            return data;
        }

        protected void Repeater1_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            EmpresaTrabajador empresaTrabajador = e.Item.DataItem as EmpresaTrabajador;
            if (empresaTrabajador != null)
            {
                DropDownList ddlEmpresa = e.Item.FindControl("ddlEmpresa") as DropDownList;
                ddlEmpresa.DataSource = _db.Empresa.Where(x => x.Eliminado != true).OrderBy(x => x.TipoEmpresa_Id).ToList<Prestlan.Models.Empresa>();
                ddlEmpresa.DataBind();
                ddlEmpresa.SelectedValue = empresaTrabajador.Empresa_Id.ToString();
                ListBox lbTrabajadores = e.Item.FindControl("lbTrabajadores") as ListBox;
                lbTrabajadores.DataSource = _db.Trabajador.Where(x => x.Empresa_Id == empresaTrabajador.Empresa_Id && x.Eliminado != true).ToList<Prestlan.Models.Trabajador>();
                lbTrabajadores.DataBind();
                if (empresaTrabajador.Trabajadores.Count > 0)
                {
                    foreach (int idTrabajador in empresaTrabajador.Trabajadores)
                    {
                        foreach (ListItem item in lbTrabajadores.Items)
                        {
                            if (item.Value == idTrabajador.ToString())
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        protected void AddNewRow_Click(object sender, EventArgs e)
        {
            RepeaterAction("add", 0);
        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlEmpresa = (DropDownList)sender;
            int idEmpresa = int.Parse(ddlEmpresa.SelectedValue);
            RepeaterItem item = ddlEmpresa.Parent as RepeaterItem;
            if (item != null)
            {
                ListBox lbTrabajadores = item.FindControl("lbTrabajadores") as ListBox;
                if (lbTrabajadores != null)
                {
                    lbTrabajadores.DataSource = _db.Trabajador.Where(x => x.Empresa_Id == idEmpresa).ToList<Prestlan.Models.Trabajador>();
                    lbTrabajadores.DataBind();
                }
            }
        }

        protected void lnkEliminarFila_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            RepeaterItem rItem = lnk.Parent as RepeaterItem;
            RepeaterAction("delete", rItem.ItemIndex); 
        }

        private void RepeaterAction(string action, int deleteIdnex)
        {
            List<EmpresaTrabajador> data = new List<EmpresaTrabajador>();
            for (int i = 0; i < Repeater1.Items.Count; i++)
            {
                if (action != "delete" || i != deleteIdnex)
                {
                    DropDownList ddlEmpresa = Repeater1.Items[i].FindControl("ddlEmpresa") as DropDownList;
                    ListBox lbTrabajadores = Repeater1.Items[i].FindControl("lbTrabajadores") as ListBox;
                    if (lbTrabajadores != null && ddlEmpresa != null)
                    {
                        List<int> trabajadores = new List<int>();
                        foreach (ListItem item in lbTrabajadores.Items)
                        {
                            if (item.Selected)
                            {
                                trabajadores.Add(int.Parse(item.Value));
                            }
                        }
                        data.Add(new EmpresaTrabajador(int.Parse(ddlEmpresa.SelectedValue), trabajadores));
                    }
                }
            }
            if (action == "add")
            {
                data.Add(new EmpresaTrabajador(GetEmpresas().FirstOrDefault().Id));
            }            
            Repeater1.DataSource = data;
            Repeater1.DataBind();
        }

        protected void GenerarZIP_Click(object sender, EventArgs e)
        {
            if (ValidarGeneracionZIP())
            {
                //Obtengo las empresas, trabajadores y requisitos seleccionados
                StringBuilder strListaRequisitos = new StringBuilder();
                StringBuilder strListaEmpresas = new StringBuilder();
                StringBuilder strListaTrabajadores = new StringBuilder();
                foreach (ListItem lItem in lbRequisitos.Items)
                {
                    //Requisitos
                    if (lItem.Selected)
                    {
                        strListaRequisitos.AppendFormat("{0},", lItem.Value);
                    }                    
                }
                foreach (RepeaterItem rItem in Repeater1.Items)
                {
                    //Empresas
                    DropDownList ddlEmpresa = rItem.FindControl("ddlEmpresa") as DropDownList;
                    strListaEmpresas.AppendFormat("{0},", ddlEmpresa.SelectedValue);

                    //Trabajadores
                    ListBox lbTrabajadores = rItem.FindControl("lbTrabajadores") as ListBox;
                    foreach (ListItem lItem in lbTrabajadores.Items)
                    {
                        if (lItem.Selected)
                        {
                            strListaTrabajadores.AppendFormat("{0},", lItem.Value);
                        }                        
                    }
                }

                //Obtengo los documentos que se van a incluir en el zip
                List<SELECT_DocumentosGenerarZIP_Result> data = _db.SELECT_DocumentosGenerarZIP(strListaEmpresas.ToString(), 
                                                                                                strListaTrabajadores.ToString(), 
                                                                                                strListaRequisitos.ToString()).OrderBy(x => x.Empresa_Id).ToList<SELECT_DocumentosGenerarZIP_Result>();

                //Si no existe la carpeta temp la creo
                if (!System.IO.Directory.Exists(Server.MapPath("~/Temp")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Temp"));
                }

                //Borro todo el contenido de Temp
                System.IO.Directory.Delete(Server.MapPath("~/Temp"), true);

                //Creo una carpeta con nombre unico dentro de temp
                Guid guid = Guid.NewGuid();
                string dirPath = Server.MapPath(String.Format("~/Temp/{0}",guid.ToString()));
                System.IO.Directory.CreateDirectory(dirPath);

                //Copio los ficheros en el directorio
                /******************************************************************************************************************/
                int? empresaId = null;
                string empresaPath = "";
                int? trabajadorId = null;
                string trabajadorPath = "";
                string filePath = "";
                string copyFilePath = "";
                string extension = "";
                string repositorioDocumentos = ConfigurationManager.AppSettings["RepositorioDocumentos"].ToString();
                foreach (SELECT_DocumentosGenerarZIP_Result result in data)
                {
                    if (empresaId == null || empresaId != result.Empresa_Id)
                    {
                        //Creo la carpeta de empresa
                        empresaPath = String.Format("{0}/{1}", dirPath, result.NombreEmpresa);
                        System.IO.Directory.CreateDirectory(SanitizeDirectoryName(empresaPath));
                    }
                    if (result.Trabajador_Id == null)
                    {
                        //Documentos de empresa
                        filePath = Server.MapPath(repositorioDocumentos + "\\" + result.nombreFS.Substring(0, 2) + "\\" + result.nombreFS);
                        if (System.IO.File.Exists(filePath))
                        {
                            extension = System.IO.Path.GetExtension(filePath);
                            copyFilePath = empresaPath + "\\" + SanitizeFileName(result.TipoDocumento) + extension;
                            System.IO.File.Copy(filePath, copyFilePath);
                        }
                    }
                    else
                    {
                        //Documentos de trabajador
                        if (trabajadorId == null || trabajadorId != result.Trabajador_Id)
                        {
                            trabajadorPath = String.Format("{0}/{1}", empresaPath, result.NombreTrabajador);
                            System.IO.Directory.CreateDirectory(SanitizeDirectoryName(trabajadorPath));
                        }
                        filePath = Server.MapPath(repositorioDocumentos + "\\" + result.nombreFS.Substring(0, 2) + "\\" + result.nombreFS);
                        if (System.IO.File.Exists(filePath))
                        {
                            extension = System.IO.Path.GetExtension(filePath);
                            copyFilePath = trabajadorPath + "\\" + SanitizeFileName(result.TipoDocumento) + extension;
                            System.IO.File.Copy(filePath, copyFilePath);
                        }
                    }
                }
                /******************************************************************************************************************/
                //Creo el fichero .zip
                Guid guidFileFolder = Guid.NewGuid();
                string dirPathFile = Server.MapPath(String.Format("~/Temp/{0}", guidFileFolder.ToString()));
                System.IO.Directory.CreateDirectory(dirPathFile);
                string zipFileName = SanitizeFileName(txtNombreFichero.Text) + ".zip";
                string zipFilePath = string.Format("{0}/{1}.zip",dirPathFile, SanitizeFileName(txtNombreFichero.Text));
                ZipFile.CreateFromDirectory(dirPath, zipFilePath, CompressionLevel.Fastest, false);

                //Envio el fichero al navegador
                if (File.Exists(zipFilePath))
                {
                    var fileInfo = new System.IO.FileInfo(zipFilePath);
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", zipFileName));
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    Response.WriteFile(zipFilePath);
                    Response.End();
                }
            }
        }

        protected void ComprobarDoc_Click(object sender, EventArgs e)
        {
            if (ValidarGeneracionZIP())
            {
                //Obtengo las empresas, trabajadores y requisitos seleccionados
                StringBuilder strListaRequisitos = new StringBuilder();
                StringBuilder strListaEmpresas = new StringBuilder();
                StringBuilder strListaTrabajadores = new StringBuilder();
                foreach (ListItem lItem in lbRequisitos.Items)
                {
                    //Requisitos
                    if (lItem.Selected)
                    {
                        strListaRequisitos.AppendFormat("{0},", lItem.Value);
                    }                    
                }
                foreach (RepeaterItem rItem in Repeater1.Items)
                {
                    //Empresas
                    DropDownList ddlEmpresa = rItem.FindControl("ddlEmpresa") as DropDownList;
                    strListaEmpresas.AppendFormat("{0},", ddlEmpresa.SelectedValue);

                    //Trabajadores
                    ListBox lbTrabajadores = rItem.FindControl("lbTrabajadores") as ListBox;
                    foreach (ListItem lItem in lbTrabajadores.Items)
                    {
                        if (lItem.Selected)
                        {
                            strListaTrabajadores.AppendFormat("{0},", lItem.Value);
                        }                        
                    }
                }

                GridViewDocEmpresa.DataSource = _db.SELECT_DocumentosGenerarZIP_InfoEmpresa(strListaEmpresas.ToString(), strListaRequisitos.ToString()).ToList();
                GridViewDocEmpresa.DataBind();

                GridViewDocTrabajadores.DataSource = _db.SELECT_DocumentosGenerarZIP_InfoTrabajadores(strListaTrabajadores.ToString(), strListaRequisitos.ToString()).ToList();
                GridViewDocTrabajadores.DataBind();
            }
        }

        private string SanitizeDirectoryName(string directoryName)
        {
            var invalids = System.IO.Path.GetInvalidPathChars();
            var newName = String.Join("_", directoryName.Split(invalids, StringSplitOptions.RemoveEmptyEntries));
            return newName;
        }
        private string SanitizeFileName(string fileName)
        {
            var invalids = System.IO.Path.GetInvalidFileNameChars();
            var newName = String.Join("_", fileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            return newName;
        }

        private bool ValidarGeneracionZIP()
        {
            //TODO: Mostrar mensajes mas especificos, estilos css, etc...
            if (string.IsNullOrEmpty(txtNombreFichero.Text) ||
                lbRequisitos.GetSelectedIndices().Length <= 0 ||
                Repeater1.Items.Count <= 0)
            {
                msgValidacion.Text = "No ha rellenado todos los datos necesarios";
                return false;
            }
            else
            {
                msgValidacion.Text = "";
                return true;
            }
        }
    }

    public class EmpresaTrabajador
    {
        public int Empresa_Id { get; set; }
        public List<int> Trabajadores { get; set; }

        public EmpresaTrabajador(int empresaId)
        {
            Empresa_Id = empresaId;
            Trabajadores = new List<int>();
        }
        public EmpresaTrabajador(int empresaId, List<int> trabajadores)
        {
            Empresa_Id = empresaId;
            Trabajadores = trabajadores;
        }
    }
}