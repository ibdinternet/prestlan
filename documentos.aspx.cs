using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestlan.Utilidades;
using System.Web.Security;
using System.IO;
using Prestlan.Models;
using IBD.Web;
using System.Data.Entity;

namespace Prestlan
{
    public partial class documentos : System.Web.UI.Page
    {

        protected PRESTLANEntities _db = new PRESTLANEntities();

        public int _documento_Id { get; set; }
        public string _mode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ObtenerDatosQueryString();
                if (PermisosOK())
                {
                    EstablecerModoFormView();
                    InicializarAsignaciones();
                    AplicarTraducciones();                    
                }
                else
                {
                    Response.Redirect("~/noAutorizado.aspx");
                }
            }
        }

        private void ObtenerDatosQueryString()
        {
            if (Request.QueryString["mode"] != null)
            {
                _mode = Request.QueryString["mode"].ToString().ToLower();
            }
            else
            {
                _mode = "insert";
            }
            if (_mode == "edit")
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = 0;
                    if (int.TryParse(Request.QueryString["id"].ToString(), out id))
                    {
                        _documento_Id = id;
                    }
                }
            }
        }
        private void EstablecerModoFormView()
        {
            if (_mode == "read")
            {
                FormView1.ChangeMode(FormViewMode.ReadOnly);
            }
            else if (_mode == "edit")
            {
                FormView1.ChangeMode(FormViewMode.Edit);
            }
            else if (_mode == "insert")
            {
                FormView1.ChangeMode(FormViewMode.Insert);
            }
        }
        private bool PermisosOK()
        {
            bool result = false;
            int usuarioId = Utilidades.Autentificacion.GetUsuarioID();
            if (usuarioId > 0)
            {
                if (Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.DocumentosEditarTodo))
                {
                    result = true;
                }
                else if (Utilidades.Autentificacion.comprobarPermiso(usuarioId, TipoPermiso.DocumentosEditarSoloEmpresa))
                {
                    if (_mode == "edit")
                    {
                        result = Utilidades.Autentificacion.EsDocumentoEmpresaUsuario(_documento_Id);
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        private void InicializarAsignaciones()
        {
            if (_mode == "edit")
            {
                RadioButtonList rbPropiedad = (RadioButtonList)FormView1.FindControl("rbPropiedad");
                ListBox lbTrabajadores = (ListBox)FormView1.FindControl("ddAsignacion");
                DropDownList ddlEmpresas = (DropDownList)FormView1.FindControl("ddlEmpresas");
                DropDownList ddTipoDocumento = (DropDownList)FormView1.FindControl("ddTipoDocumento");

                int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                Prestlan.Models.Documento documento = _db.Documento.Find(id);
                bool primeraVez = true;
                foreach (Propietario prop in documento.Propietario)
                {
                    if (prop.Trabajador_Id.HasValue)
                    {
                        if (primeraVez)
                        {
                            rbPropiedad.SelectedValue = "Empleado";
                            rbPropiedad.Items[0].Selected = true;
                            rbPropiedad_SelectedIndexChanged(this, new EventArgs());
                            ddlEmpresas.SelectedValue = prop.Empresa_Id.ToString();
                            lbTrabajadores.DataBind();
                            ddTipoDocumento.DataBind();
                            ddTipoDocumento.SelectedValue = documento.TipoDocumento_Id.ToString();
                            primeraVez = false;
                        }
                        foreach (ListItem li in lbTrabajadores.Items)
                        {
                            if (li.Value == prop.Trabajador_Id.ToString())
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                    else if (prop.Empresa_Id.HasValue)
                    {
                        ddlEmpresas.SelectedValue = prop.Empresa_Id.ToString();
                        rbPropiedad.SelectedValue = "Empresa";
                        rbPropiedad.Items[1].Selected = true;
                        rbPropiedad_SelectedIndexChanged(this, new EventArgs());
                        ddTipoDocumento.DataBind();
                        ddTipoDocumento.SelectedValue = documento.TipoDocumento_Id.ToString();
                    }
                }
            }
        }
        private void AplicarTraducciones()
        {
            // Traducir radiobutton
            RadioButtonList rbl = FormView1.FindControl("rbPropiedad") as RadioButtonList;
            if (rbl != null)
            {
                rbl.Items[0].Text = Traduce.getTG("empleado");
                rbl.Items[1].Text = Traduce.getTG("empresa");
            }
        }
        private void CambiosSegunEstado()
        {
            //Se llama a esta funcion en el FormView_Load, cuando todo se ha cargado

            //Si esta en edicion
            if (_mode == "edit")
            {
                //y el documento esta publicado o caducado
                Documento doc = _db.Documento.Find(_documento_Id);
                if (doc.Estado_Id == (int)TiposDeEstado.PUBLICADO ||
                    doc.Estado_Id == (int)TiposDeEstado.CADUCADO)
                {
                    RadioButtonList rblPropiedad = FormView1.FindControl("rbPropiedad") as RadioButtonList;
                    rblPropiedad.Enabled = false;
                    DropDownList ddlEmpresas = FormView1.FindControl("ddlEmpresas") as DropDownList;
                    ddlEmpresas.Enabled = false;
                    DropDownList ddTipoDocumento = FormView1.FindControl("ddTipoDocumento") as DropDownList;
                    ddTipoDocumento.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Cargo el documento en el formview
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Prestlan.Models.Documento FormView1_GetItem([QueryString]int? Id)
        {
            if (Id.HasValue)
            {
                return _db.Documento.Find(Id.Value);
            }
            return null;
        }

        public void FormView1_InsertItem()
        {
            var documento = new Prestlan.Models.Documento();

            TryUpdateModel(documento);

            if (ModelState.IsValid)
            {
                // Fecha creación y actualización
                documento.FechaCreado = DateTime.Now;
                documento.FechaActualizado = DateTime.Now;

                // Hash del empleado ?????
                documento.HashEmpleado = Tools.GenerarHash();

                // ID del usuario actual                    
                documento.Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();

                // Estado inicial = BORRADOR
                documento.Estado_Id = (int)TiposDeEstado.BORRADOR;

                // Etapa  inicial = Etapa 1, Borrador
                documento.Etapa_Id = (int)TiposDeEtapa.BORRADOR;

                // Tipo documento (No esta con Bind por que al ser un desplegable filtrado por "rPropiedad" daba error)
                documento.TipoDocumento_Id = int.Parse(((DropDownList)FormView1.FindControl("ddTipoDocumento")).SelectedValue);

                // Procesar fichero
                FileUpload fu = ((FileUpload)FormView1.FindControl("fuDocumento"));
                string fuu = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper() + System.IO.Path.GetExtension(fu.FileName);
                string filePath = ProcesarFichero(fu, fuu);

                if (filePath != "")
                {
                    // Añadimos el fichero
                    var fichero = new Prestlan.Models.Fichero();

                    fu.PostedFile.SaveAs(filePath);

                    fichero.nombre = fu.FileName;
                    fichero.nombreFS = fuu;
                    fichero.FechaCreado = DateTime.Now;
                    documento.Fichero = fichero;

                    _db.Documento.Add(documento);
                    _db.SaveChanges();

                    // Actualizamos la versión del documento
                    documento.DocumentoVersion_Id = documento.Id;

                    //Obtengo el tipo de propietario Empresa | Empleado
                    string tipoPropietario = ((RadioButtonList)FormView1.FindControl("rbPropiedad")).SelectedValue;
                    if (tipoPropietario == "Empresa")
                    {
                        var propietario = new Prestlan.Models.Propietario();
                        propietario.Empresa_Id = int.Parse(((DropDownList)FormView1.FindControl("ddlEmpresas")).SelectedValue);
                        documento.Propietario.Add(propietario);
                    }
                    else if (tipoPropietario == "Empleado")
                    {
                        // Obtenemos todos los id de trabajador seleccionados en un array y añadimos los propietarios
                        var selectedItems = from ListItem i in ((ListBox)FormView1.FindControl("ddAsignacion")).Items where i.Selected select i;
                        if (selectedItems.Count() > 0)
                        {
                            documento.SinPropietarios = false;
                        }
                        else
                        {
                            documento.SinPropietarios = true;
                        }
                        foreach (var p in selectedItems)
                        {
                            var trabajador = _db.Trabajador.Find(int.Parse(p.Value));
                            var propietario = new Prestlan.Models.Propietario();
                            propietario.Empresa_Id = trabajador.Empresa_Id;
                            propietario.Trabajador_Id = trabajador.Id;
                            documento.Propietario.Add(propietario);
                        }
                    }
                    else
                    {
                        documento.SinPropietarios = true;
                    }
                    _db.SaveChanges();

                    msgEstado.Text = Traduce.getTG("documentoinsertok");
                    panelEstado.Visible = true;
                    FormView1.Visible = false;
                }
                else
                {
                    msgError.Text = Traduce.getTG("soloficherosextension");
                    panelError.Visible = true;
                }
            }
        }
        public void FormView1_UpdateItem(int id)
        {
            Prestlan.Models.Documento documento = _db.Documento.Find(id);
            if (documento == null)
            {
                ModelState.AddModelError("", String.Format("No se encontró el elemento con id. {0}", id));
                return;
            }
            TryUpdateModel(documento);
            if (ModelState.IsValid)
            {
                FileUpload fu = ((FileUpload)FormView1.FindControl("fuDocumento"));
                if (documento.Estado_Id != (int)TiposDeEstado.BORRADOR && fu != null && fu.HasFile)
                {
                    //CREAR UNA NUEVA VERSION
                    //************************************************************************************************************/
                    Prestlan.Models.Documento nuevo_documento = new Prestlan.Models.Documento();
                    //Fichero
                    string fuu = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper() + System.IO.Path.GetExtension(fu.FileName);
                    string filePath = ProcesarFichero(fu, fuu);
                    if (filePath != "")
                    {
                        var fichero = new Prestlan.Models.Fichero();
                        fu.PostedFile.SaveAs(filePath);
                        fichero.nombre = fu.FileName;
                        fichero.nombreFS = fuu;
                        fichero.FechaCreado = DateTime.Now;
                        _db.Fichero.Add(fichero);
                        _db.SaveChanges();
                        nuevo_documento.Fichero_Id = fichero.Id;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Extension incorrecta");
                        return;
                    }
                    // Título
                    nuevo_documento.Titulo = documento.Titulo;
                    // Descripción
                    nuevo_documento.Descripcion = documento.Descripcion;
                    // Caducidad y Fecha de caducidad
                    if (((CheckBox)FormView1.FindControl("cbCaducidad")).Checked)
                    {
                        nuevo_documento.Caduca = true;
                        nuevo_documento.FechaCaducidad = DateTime.Parse(((TextBox)FormView1.FindControl("tbFechaCaducidad")).Text);
                    }
                    else
                    {
                        nuevo_documento.Caduca = false;
                        nuevo_documento.FechaCaducidad = null;
                    }
                    // actualización de fecha
                    nuevo_documento.FechaActualizado = DateTime.Now;
                    // Reutilizamos el hash de empleado ?????
                    nuevo_documento.HashEmpleado = documento.HashEmpleado;
                    // Fecha creación y actualización
                    nuevo_documento.FechaCreado = DateTime.Now;
                    nuevo_documento.FechaActualizado = DateTime.Now;
                    // ID del usuario actual                    
                    nuevo_documento.Usuario_Id = documento.Usuario_Id;
                    // Tipo documento
                    nuevo_documento.TipoDocumento_Id = int.Parse(((DropDownList)FormView1.FindControl("ddTipoDocumento")).SelectedValue);
                    // Estado inicial = BORRADOR
                    nuevo_documento.Estado_Id = (int)TiposDeEstado.BORRADOR;
                    // Etapa  inicial = Etapa 1, Borrador
                    nuevo_documento.Etapa_Id = (int)TiposDeEtapa.BORRADOR;
                    // Versiones
                     nuevo_documento.DocumentoVersion_Id = documento.Id;
                    _db.Documento.Add(nuevo_documento);
                    _db.SaveChanges();
                   
                    //Obtengo el tipo de propietario Empresa | Empleado
                    string tipoPropietario = ((RadioButtonList)FormView1.FindControl("rbPropiedad")).SelectedValue;
                    if (tipoPropietario == "Empresa")
                    {
                        var propietario = new Prestlan.Models.Propietario();
                        propietario.Empresa_Id = int.Parse(((DropDownList)FormView1.FindControl("ddlEmpresas")).SelectedValue);
                        nuevo_documento.Propietario.Add(propietario);
                    }
                    else if (tipoPropietario == "Empleado")
                    {
                        // Obtenemos todos los id de trabajador seleccionados en un array y añadimos los propietarios
                        var selectedItems = from ListItem i in ((ListBox)FormView1.FindControl("ddAsignacion")).Items where i.Selected select i;
                        if (selectedItems.Count() > 0)
                        {
                            nuevo_documento.SinPropietarios = false;
                        }
                        else
                        {
                            nuevo_documento.SinPropietarios = true;
                        }
                        foreach (var p in selectedItems)
                        {
                            var trabajador = _db.Trabajador.Find(int.Parse(p.Value));
                            var propietario = new Prestlan.Models.Propietario();
                            propietario.Empresa_Id = trabajador.Empresa_Id;
                            propietario.Trabajador_Id = trabajador.Id;
                            nuevo_documento.Propietario.Add(propietario);
                        }
                    }
                    else
                    {
                        nuevo_documento.SinPropietarios = true;
                    }

                    _db.SaveChanges();
                    msgEstado.Text = Traduce.getTG("documentoeditok");
                    panelEstado.Visible = true;
                    FormView1.Visible = false;
                }
                else
                {
                    //Actualizar documento existente
                    //Fichero
                    string fuu = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper() + System.IO.Path.GetExtension(fu.FileName);
                    string filePath = ProcesarFichero(fu, fuu);
                    if (filePath != "")
                    {
                        var fichero = new Prestlan.Models.Fichero();
                        fu.PostedFile.SaveAs(filePath);
                        fichero.nombre = fu.FileName;
                        fichero.nombreFS = fuu;
                        fichero.FechaCreado = DateTime.Now;
                        _db.Fichero.Add(fichero);
                        _db.SaveChanges();
                        documento.Fichero_Id = fichero.Id;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Extension incorrecta");
                        return;
                    }
                    //El resto de campos estan bindeados

                    // Tipo documento
                    documento.TipoDocumento_Id = int.Parse(((DropDownList)FormView1.FindControl("ddTipoDocumento")).SelectedValue);
                    // Caducidad y Fecha de caducidad
                    if (((CheckBox)FormView1.FindControl("cbCaducidad")).Checked)
                    {
                        documento.Caduca = true;
                        documento.FechaCaducidad = DateTime.Parse(((TextBox)FormView1.FindControl("tbFechaCaducidad")).Text);
                    }
                    else
                    {
                        documento.Caduca = false;
                        documento.FechaCaducidad = null;
                    }
                    //Borro los propietarios anteriores
                    foreach (Propietario dp in documento.Propietario.ToList())
                    {
                        _db.Entry<Propietario>(dp).State = EntityState.Deleted;
                    }

                    //Obtengo el tipo de propietario Empresa | Empleado
                    string tipoPropietario = ((RadioButtonList)FormView1.FindControl("rbPropiedad")).SelectedValue;
                    if (tipoPropietario == "Empresa")
                    {
                        var propietario = new Prestlan.Models.Propietario();
                        propietario.Empresa_Id = int.Parse(((DropDownList)FormView1.FindControl("ddlEmpresas")).SelectedValue);
                        documento.Propietario.Add(propietario);
                    }
                    else if (tipoPropietario == "Empleado")
                    {
                        // Obtenemos todos los id de trabajador seleccionados en un array y añadimos los propietarios
                        var selectedItems = from ListItem i in ((ListBox)FormView1.FindControl("ddAsignacion")).Items where i.Selected select i;
                        if (selectedItems.Count() > 0)
                        {
                            documento.SinPropietarios = false;
                        }
                        else
                        {
                            documento.SinPropietarios = true;
                        }
                        foreach (var p in selectedItems)
                        {
                            var trabajador = _db.Trabajador.Find(int.Parse(p.Value));
                            var propietario = new Prestlan.Models.Propietario();
                            propietario.Empresa_Id = trabajador.Empresa_Id;
                            propietario.Trabajador_Id = trabajador.Id;
                            documento.Propietario.Add(propietario);
                        }
                    }
                    else
                    {
                        documento.SinPropietarios = true;
                    }
                    _db.SaveChanges();
                    msgEstado.Text = Traduce.getTG("documentoeditok");
                    panelEstado.Visible = true;
                    FormView1.Visible = false;
                }
            }
        }

        /// <summary>
        /// Comprueba que el archivo a subir está autorizado por extensión
        /// Crea subdirectorio si no existe
        /// </summary>
        /// <param name="f"></param>
        /// <param name="fuu"></param>
        /// <returns></returns>
        protected string ProcesarFichero(FileUpload f, string fuu)
        {
            string filePath = "";
            if (f != null)
            {
                string path = Server.MapPath(ConfigurationManager.AppSettings["RepositorioDocumentos"].ToString());
                if (f.HasFile)
                {
                    String fileExtension = System.IO.Path.GetExtension(f.FileName).ToLower();
                    String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".pdf", ".doc", ".docx", ".txt" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (fileExtension == allowedExtensions[i])
                        {
                            if (!Directory.Exists(fuu.Substring(0, 2)))
                            {
                                Directory.CreateDirectory(path + "\\" + fuu.Substring(0, 2));
                            }
                            filePath = path + "\\" + fuu.Substring(0, 2) + "\\" + fuu;
                        }
                    }
                }
            }
            return filePath;
        }
        public string ProcesaFechaCaducidad(object of)
        {
            if (of != null)
            {
                return Convert.ToDateTime(of).ToShortDateString();
            }
            else return "";
        }

        public IQueryable<Prestlan.Models.TipoDocumento_translation> GetTipoDocumento([Control]string rbPropiedad)
        {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            bool EsDocumentoDeEmpresa = rbPropiedad == "Empresa";
            return _db.TipoDocumento_translation.Where(a => a.Idioma_codigo == idioma && (a.TipoDocumento.EsDocumentoDeEmpresa == EsDocumentoDeEmpresa || EsDocumentoDeEmpresa == null));
        }
        public string GetStrTipodocumento(object oid)
        {
            string idioma = "es";
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            int tdi = 0;
            if (oid != null)
            {
                tdi = Convert.ToInt32(oid);
            }
            else return "";

            return _db.TipoDocumento_translation.Where(a => a.Idioma_codigo == idioma).Where(t => t.Tipodocumento_Id == tdi).FirstOrDefault().Descripcion;
        }
        public IQueryable<Prestlan.Models.Trabajador> GetTrabajadores()
        {
            return _db.Trabajador.Where(x => x.Eliminado != true);
        }
        public IQueryable<Prestlan.Models.Trabajador> GetTrabajadoresPorEmpresa([Control]int? ddlEmpresas)
        {
            if (ddlEmpresas != null)
            {
                return _db.Trabajador.Where(x => x.Empresa_Id == ddlEmpresas.Value && x.Eliminado != true);
            }
            else return null;
        }
        public IQueryable<Prestlan.Models.Empresa> GetEmpresas()
        {
            int Usuario_Id = Utilidades.Autentificacion.GetUsuarioID();
            Usuario user = _db.Usuario.Find(Usuario_Id);
            if (Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.DocumentosEditarTodo))
            {
                return _db.Empresa.Where(x => x.Eliminado != true).OrderBy(x => x.TipoEmpresa_Id);
            }
            else if (Utilidades.Autentificacion.comprobarPermiso(Usuario_Id, TipoPermiso.DocumentosEditarSoloEmpresa))
            {
                return _db.Empresa.Where(x => x.Eliminado != true).OrderBy(x => x.TipoEmpresa_Id).Where(x => x.Id == user.Empresa_Id);
            }
            else
            {
                return null;
            }
        }

        protected void rbPropiedad_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rb = FormView1.FindControl("rbPropiedad") as RadioButtonList;
            if (rb != null)
            {
                DropDownList ddlEmpresas = FormView1.FindControl("ddlEmpresas") as DropDownList;
                Panel pnlTrabajadores = FormView1.FindControl("pnlTrabajadores") as Panel;

                if (ddlEmpresas != null && pnlTrabajadores != null)
                {
                    if (rb.SelectedValue == "Empresa")
                    {
                        ddlEmpresas.Visible = true;
                        pnlTrabajadores.Visible = false;
                    }
                    else if (rb.SelectedValue == "Empleado")
                    {
                        ddlEmpresas.Visible = true;
                        pnlTrabajadores.Visible = true;
                    }
                    else
                    {
                        ddlEmpresas.Visible = false;
                        pnlTrabajadores.Visible = false;
                    }
                }
            }
            DropDownList ddTipoDocumento = FormView1.FindControl("ddTipoDocumento") as DropDownList;
            ddTipoDocumento.DataBind();
        }
        protected void ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/list_documentos.aspx");
            }
        }

        protected void FormView1_Load(object sender, EventArgs e)
        {
            CambiosSegunEstado();
        }
    }
}