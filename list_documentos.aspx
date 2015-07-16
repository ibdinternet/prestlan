<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="list_documentos.aspx.cs" Inherits="Prestlan.list_documentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.list_documentos.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        <span class="icon-doc-small"></span><%= IBD.Web.Traduce.getTG("listardocumentos") %></h2>
    <br />

    <div class="buscadorListado">
        <label for="lbEmpleados"><%= IBD.Web.Traduce.getTG("empleados") %></label>
        <asp:ListBox ID="lbEmpleados" runat="server" SelectionMode="Multiple"
            SelectMethod="GetTrabajadores"
            DataTextField="Nombre"
            DataValueField="Id"></asp:ListBox>
        <asp:Label runat="server" ID="lEmpresas" AssociatedControlID="lbEmpresas"><%= IBD.Web.Traduce.getTG("empresa") %></asp:Label>
        <asp:ListBox ID="lbEmpresas" AutoPostBack="true" runat="server" SelectionMode="Multiple"
            SelectMethod="GetEmpresas"
            DataTextField="Nombre"
            DataValueField="Id"></asp:ListBox>
        <label for="lbTipoDocumentos"><%= IBD.Web.Traduce.getTG("tipodocumento") %></label>
        <asp:ListBox ID="lbTipoDocumentos" runat="server" SelectionMode="Multiple"
            SelectMethod="GetTipoDocumentos"
            DataTextField="Descripcion"
            DataValueField="Id"></asp:ListBox>
        <label for="lbEstados"><%= IBD.Web.Traduce.getTG("estado") %></label>
        <asp:ListBox ID="lbEstados" runat="server" SelectionMode="Multiple"
            SelectMethod="GetEstados"
            DataTextField="Descripcion"
            DataValueField="Id"></asp:ListBox>
        <asp:LinkButton runat="server" CssClass="button" ID="btnBuscar" OnClick="btnBuscar_Click">
            <span class="icon-search"></span>
                Buscar
        </asp:LinkButton>
    </div>
    <br />

    <div class="btn-group">
        <asp:LinkButton ID="lnkPropietario" OnClick="lnkPropietario_Click" class="btn btn-default" runat="server"><%= IBD.Web.Traduce.getTG("propietario") %></asp:LinkButton>
        <asp:LinkButton ID="lnkTipoDocumento" OnClick="lnkTipoDocumento_Click" class="btn btn-default linkactive" runat="server"><%= IBD.Web.Traduce.getTG("tipodocumento") %></asp:LinkButton>
    </div>
    <br />

    <asp:DropDownList ID="ddOrdenacion" runat="server" CssClass="float_right" AutoPostBack="true" OnSelectedIndexChanged="ddOrdenacion_SelectedIndexChanged">
    </asp:DropDownList>

    <div id="divDialog" class="ui-helper-hidden">
        <asp:Label ID="lblTitulo" runat="server" />
        <br />
        <asp:Label ID="lblDescripcion" runat="server" />
        <br />
        <span class="tit"><%= IBD.Web.Traduce.getTG("subidopor") %>: </span>
        <asp:Label ID="lblAutor" runat="server" />
        <br />
        <span class="tit"><%= IBD.Web.Traduce.getTG("version") %>: </span>
        <asp:Label ID="lblVersion" runat="server" />
        <br />
        <span class="tit"><%= IBD.Web.Traduce.getTG("estado") %>: </span>
        <asp:Label ID="lblEstado" runat="server" />&nbsp;<asp:Label ID="lblEtapa" runat="server" />
        <br />
        <br />
        <div class="<%= PermitirEdicion == true ? "icon-dialog" : "icon-dialog hidden" %>">
            <a href="#" id="bVerFichero" class="button-icon" title='<%= IBD.Web.Traduce.getTG("ver") %>'><span class="icon-search-white"></span></a>
            <a href="#" id="bPasarARevision" class="button-icon " title='<%= IBD.Web.Traduce.getTG("pasararevision") %>'><span class="icon-checkmark"></span></a>

            <asp:HyperLink CssClass="button-icon bValidar" ID="lnkValidar" runat="server"><span class="icon-checkmark"></span></asp:HyperLink>

            <%--<a href="#" id="bValidar" class="button-icon " title='<%= IBD.Web.Traduce.getTG("validar") %>'><span class="icon-checkmark"></span></a>--%>

            <a href="#" id="bPasarABorrador" class="button-icon" title='<%= IBD.Web.Traduce.getTG("pasaraborrador") %>'><span class="icon-stack"></span></a>
            <a href="#" id="bEditar" class="button-icon" title='<%= IBD.Web.Traduce.getTG("editar") %>'><span class="icon-editar2"></span></a>
            <a href="#" id="bEliminarBorrador" class="button-icon" title='<%= IBD.Web.Traduce.getTG("eliminarborrador") %>'><span class="icon-eliminar"></span></a>            
        </div>
    </div>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSorting="GridView1_Sorting" OnRowDataBound="GridView1_RowDataBound"
        EnableViewState="False" DataKeyNames="Documento_Id" CellPadding="3" CssClass="table">
        <EmptyDataTemplate>
            <%= IBD.Web.Traduce.getTG("nosehanencontradodocs") %>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:CheckBox runat="server" ID="chkAll" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="chkDocumento" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Documento_Id" Visible="false" />
            <asp:TemplateField HeaderText="Tipo de documento" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HyperLink ID="hlTipoDocumento" Target="_blank" runat="server" NavigateUrl='<%# "handlers/opendoc.ashx?id=" + DataBinder.Eval(Container.DataItem, "Fichero_id") + "" %>' Text='<%# Bind("TipoDocumento") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    <%= IBD.Web.Traduce.getTG("versiones") %>
                </HeaderTemplate>
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FechaCaducidad" HeaderText="Caducidad" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200" />
            <asp:BoundField DataField="FechaCreado" HeaderText="FechaCreado" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200" />
        </Columns>
    </asp:GridView>

</asp:Content>
