<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="reclamaciones.aspx.cs" Inherits="Prestlan.reclamaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.reclamaciones.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfuid" runat="server" Value='<%= Session["Usuario_Id"].ToString() %>' />
    <h2>
        <span class="icon-reclama-small"></span><%= IBD.Web.Traduce.getTG("reclamaciones") %></h2>
    <br />
    <div class="buscadorListado">
        <label for="Empleados"><%= IBD.Web.Traduce.getTG("empleados") %></label>
        <asp:ListBox ID="lbEmpleados" runat="server" SelectionMode="Multiple"
            SelectMethod="GetTrabajadores"
            DataTextField="Nombre"
            DataValueField="Id"></asp:ListBox>
        <label for="Empresa"><%= IBD.Web.Traduce.getTG("empresa") %></label>
        <asp:ListBox ID="lbEmpresas" runat="server" SelectionMode="Multiple"
            SelectMethod="GetEmpresas"
            DataTextField="Nombre"
            DataValueField="Id"></asp:ListBox>
        <label for="tipoDocumento"><%= IBD.Web.Traduce.getTG("tipodocumento") %></label>
        <asp:ListBox ID="lbTipoDocumentos" runat="server" SelectionMode="Multiple"
            SelectMethod="GetTipoDocumentos"
            DataTextField="Descripcion"
            DataValueField="Id"></asp:ListBox>
        <asp:LinkButton runat="server" CssClass="button" ID="btnBuscar" OnClick="btnBuscar_Click">
            <span class="icon-search"></span>
                Buscar
        </asp:LinkButton>
    </div>
    <br />
    <div class="btn-group">
        <asp:LinkButton ID="lbPropietario" OnClick="lbPropietario_Click" CssClass="btn btn-default" runat="server"><%= IBD.Web.Traduce.getTG("propietario") %></asp:LinkButton>
        <asp:LinkButton ID="lbTipoDocumento" OnClick="lbTipoDocumento_Click" CssClass="btn btn-default linkactive" runat="server"><%= IBD.Web.Traduce.getTG("tipodocumento") %></asp:LinkButton>

    </div>
    <br />
    <asp:DropDownList ID="ddOrdenacion" CssClass="float_right" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddOrdenacion_SelectedIndexChanged">
    </asp:DropDownList>

    <%--<div id="divDialog" runat="server">--%>
    <div id="divDialog" class="ui-helper-hidden">
        <div id="formularioReclamacion">
            <%= IBD.Web.Traduce.getTG("motivoreclamacion") %>:        
       
            <br />
            <br />
            &nbsp;<asp:TextBox ID="tbComentario" runat="server" TextMode="MultiLine" Columns="40" Rows="10" />&nbsp;
       
            <br />
            <br />
            <div class="<%= PermitirEdicion == true ? "icon-dialog" : "icon-dialog hidden" %>">
                <a href="#" id="bEnviar" class="button-icon"><span class="icon-checkmark"></span>
                    <!--Enviar-->
                </a>
            </div>
        </div>
    </div>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
        OnSorting="GridView1_Sorting"
        OnRowDataBound="GridView1_RowDataBound"
        EnableViewState="False" DataKeyNames="Propietario_Id" CellPadding="3" CssClass="table">
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
            <asp:BoundField DataField="TipoDocumento" HeaderText="Tipo de documento" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left" />

            <asp:TemplateField ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    Versiones
               
                </HeaderTemplate>
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="FechaCaducidad" HeaderText="Caducidad" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200" />
            <asp:BoundField DataField="FechaCreado" HeaderText="FechaCreado" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200" />
        </Columns>
    </asp:GridView>

</asp:Content>
