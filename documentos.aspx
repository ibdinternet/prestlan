<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="documentos.aspx.cs" Inherits="Prestlan.documentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.documentos.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <h2><span class="icon-doc-small"></span><%= IBD.Web.Traduce.getTG("añadirdocumentos") %></h2>

    <asp:Panel runat="server" ID="panelError" Visible="false" CssClass="ErrorBox">
        <asp:Label ID="msgError" runat="server" Text="" />
    </asp:Panel>

    <asp:Panel runat="server" ID="panelEstado" Visible="false" CssClass="OkBox">
        <asp:Label ID="msgEstado" runat="server" Text="" />
    </asp:Panel>

    <asp:FormView runat="server" ID="FormView1"
        DataKeyNames="Id"
        ItemType="Prestlan.Models.Documento" DefaultMode="Insert" SelectMethod="FormView1_GetItem" UpdateMethod="FormView1_UpdateItem"
        InsertMethod="FormView1_InsertItem" OnLoad="FormView1_Load"
        OnItemCommand="ItemCommand" RenderOuterTable="False" Visible="true">

        <EditItemTemplate>
            <asp:ValidationSummary runat="server" DisplayMode="BulletList" CssClass="ErrorBox" />
            <br />
            <asp:HiddenField ID="hfid" Value='<%# Bind("Id") %>' runat="server" />
            <fieldset>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("titulo") %></label>
                    <asp:TextBox ID="tbTitulo" runat="server" Text='<%# Bind("Titulo") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbTitulo"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_titulo") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("descripcion") %>:</label>
                    <asp:TextBox ID="tbDescripcion" TextMode="MultiLine" Rows="10" runat="server" Text='<%# Bind("Descripcion") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbDescripcion"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_descripcion") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("propiedad") %>:</label>
                    <asp:RadioButtonList ID="rbPropiedad" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="rbPropiedad_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text=" Empleado" Value="Empleado" />
                        <asp:ListItem Text=" Empresa" Value="Empresa" />
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ControlToValidate="rbPropiedad"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_propiedad") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("asignacion") %>:</label>
                    <asp:DropDownList runat="server" ID="ddlEmpresas" SelectMethod="GetEmpresas" AutoPostBack="true" DataTextField="Nombre" DataValueField="Id" Visible="false" CssClass="jsSelectEmpresa" />
                </div>
                <asp:Panel runat="server" ID="pnlTrabajadores">
                    <label></label>
                    <asp:ListBox ID="ddAsignacion" runat="server" SelectionMode="Multiple"
                        SelectMethod="GetTrabajadoresPorEmpresa"
                        DataTextField="Nombre"
                        DataValueField="Id"></asp:ListBox>
                </asp:Panel>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("tipodedocumento") %></label>
                    <asp:DropDownList ID="ddTipoDocumento" runat="server" CssClass="jsSelectTipoDocumento"
                        SelectMethod="GetTipoDocumento"
                        DataTextField="descripcion"
                        DataValueField="TipoDocumento_Id">
                    </asp:DropDownList>
                    <asp:CompareValidator runat="server"
                        ControlToValidate="ddTipoDocumento" Operator="GreaterThan"
                        Display="Dynamic" Type="Integer" SetFocusOnError="true"
                        ValueToCompare="-1" ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_tipodocumento") %>' Text="<span class='errorcampo'></span>">
                    </asp:CompareValidator>
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("caducidad") %>:</label>
                    <asp:CheckBox ID="cbCaducidad" runat="server" Checked='<%# Bind("Caduca") %>' />
                    <asp:TextBox ID="tbFechaCaducidad" Text='<%# Bind("FechaCaducidad", "{0:dd/MM/yyyy}") %>' runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("documentoactual") %>:</label>
                    <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# Eval("Fichero_Id","/handlers/opendoc.ashx?id={0}") %>' Text='<%# IBD.Web.Traduce.getTG("verdocumento") %>' />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("subirdocumento") %>:</label>
                    <asp:FileUpload ID="fuDocumento" runat="server" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="fuDocumento"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("soloficherosextension") %>' Text="<span class='errorcampo'></span>"
                        ValidationExpression="(.+\.([Gg][Ii][Ff])|.+\.([Pp][Nn][Gg])|.+\.([Jj][Pp][Gg])|.+\.([Jj][Pp][Ee][Gg])|.+\.([Pp][Dd][Ff])|.+\.([Dd][Oo][Cc])|.+\.([Dd][Oo][Cc][Xx])|.+\.([Tt][Xx][Tt]))"></asp:RegularExpressionValidator>
                </div>
                <div>
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" CssClass="button"><span class="icon-guardar"></span>Guardar</asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="button"><span class="icon-cancelar"></span>Cancelar</asp:LinkButton>
                </div>

            </fieldset>
        </EditItemTemplate>



        <InsertItemTemplate>
            <asp:ValidationSummary runat="server" DisplayMode="BulletList" CssClass="ErrorBox" />
            <br />
            <fieldset>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("titulo") %>:</label><asp:TextBox ID="tbTitulo" runat="server" Text='<%# Bind("Titulo") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbTitulo"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_titulo") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("descripcion") %>:</label><asp:TextBox ID="tbDescripcion" TextMode="MultiLine" Rows="10" runat="server" Text='<%# Bind("Descripcion") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbDescripcion"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_descripcion") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("propiedad") %>:</label>
                    <asp:RadioButtonList ID="rbPropiedad" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" OnSelectedIndexChanged="rbPropiedad_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Selected="True" Text=" Empleado" Value="Empleado" />
                        <asp:ListItem Text=" Empresa" Value="Empresa" />
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ControlToValidate="rbPropiedad"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_propiedad") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("asignacion") %>:</label>
                    <asp:DropDownList runat="server" ID="ddlEmpresas" SelectMethod="GetEmpresas" AutoPostBack="true" DataTextField="Nombre" DataValueField="Id" CssClass="jsSelectEmpresa" />
                </div>
                <asp:Panel runat="server" ID="pnlTrabajadores">
                    <label></label>
                    <asp:ListBox ID="ddAsignacion" runat="server" SelectionMode="Multiple"
                        SelectMethod="GetTrabajadoresPorEmpresa"
                        DataTextField="Nombre"
                        DataValueField="Id"></asp:ListBox>
                </asp:Panel>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("tipodedocumento") %></label>
                    <asp:DropDownList ID="ddTipoDocumento" runat="server" CssClass="jsSelectTipoDocumento"
                        SelectMethod="GetTipoDocumento"
                        DataTextField="descripcion"
                        DataValueField="TipoDocumento_Id">
                    </asp:DropDownList>
                    <asp:CompareValidator runat="server"
                        ControlToValidate="ddTipoDocumento" Operator="GreaterThan"
                        Display="Dynamic" Type="Integer" SetFocusOnError="true"
                        ValueToCompare="-1" ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_tipodocumento") %>' Text="<span class='errorcampo'></span>">
                    </asp:CompareValidator>
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("caducidad") %>:</label>
                    <asp:CheckBox ID="cbCaducidad" runat="server" Checked='<%# Bind("Caduca") %>' />
                    <asp:TextBox ID="tbFechaCaducidad" Text='<%# Bind("FechaCaducidad") %>' runat="server" Enabled="false" />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("subirdocumento") %>:</label>
                    <asp:FileUpload ID="fuDocumento" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="fuDocumento"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_fichero") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="fuDocumento"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("soloficherosextension") %>' Text="*"
                        ValidationExpression="(.+\.([Gg][Ii][Ff])|.+\.([Pp][Nn][Gg])|.+\.([Jj][Pp][Gg])|.+\.([Jj][Pp][Ee][Gg])|.+\.([Pp][Dd][Ff])|.+\.([Dd][Oo][Cc])|.+\.([Dd][Oo][Cc][Xx])|.+\.([Tt][Xx][Tt]))"></asp:RegularExpressionValidator>
                </div>


                <div>
                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" CssClass="button"><span class="icon-guardar"></span><%= IBD.Web.Traduce.getTG("guardar") %></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="button"><span class="icon-cancelar"></span><%= IBD.Web.Traduce.getTG("cancelar") %></asp:LinkButton>
                </div>
            </fieldset>
        </InsertItemTemplate>

    </asp:FormView>

</asp:Content>
