<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="list_historico.aspx.cs" Inherits="Prestlan.list_historico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Solo pruebas -->
    <style type="text/css">
        .DefaultPager span
        {
            font-weight:bold;
            color:black;
        }
        .DefaultPager a
        {
            font-weight:normal;
        }
    </style>
    <script>
        $(document).ready(function () {
            $(".jsDropDownEmpresas").chosen();
            $(".jsDropDownTipoDocumento").chosen();
            $(".jsDropDownEstado").chosen();
        });        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="buscadorListado">
        <label for="ddlEmpresas"><%= IBD.Web.Traduce.getTG("empresa") %></label>
        <asp:DropDownList ID="ddlEmpresas" runat="server" CssClass="jsDropDownEmpresas"
            SelectMethod="GetEmpresas"
            DataTextField="Descripcion"
            DataValueField="Id"></asp:DropDownList>
        <label for="ddlTipoDocumentos"><%= IBD.Web.Traduce.getTG("tipodocumento") %></label>
        <asp:DropDownList ID="ddlTipoDocumentos" runat="server" CssClass="jsDropDownTipoDocumento"
            SelectMethod="GetTipoDocumentos"
            DataTextField="Descripcion"
            DataValueField="Id"></asp:DropDownList>
        <label for="ddlEstados"><%= IBD.Web.Traduce.getTG("estado") %></label>
        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="jsDropDownEstado"
            SelectMethod="GetEstados"
            DataTextField="Descripcion"
            DataValueField="Id"></asp:DropDownList>
        <br />
        <asp:CheckBox  runat="server" ID="chkElimiandos"  /> Ver solo eliminados
        <asp:LinkButton runat="server" CssClass="button" ID="btnBuscar" OnClick="btnBuscar_Click">
                Buscar
        </asp:LinkButton>
    </div>
    <asp:GridView runat="server" ID="GridViewDocumentosVersiones" AutoGenerateColumns="false" 
        SelectMethod="GridViewDocumentosVersiones_GetData" 
        AllowPaging="true" AllowSorting="true" AllowCustomPaging="true"
        DataKeyNames="Id" PagerStyle-CssClass="DefaultPager" PageSize="15" OnRowDataBound="GridViewDocumentosVersiones_RowDataBound"
        CssClass="table">
        <Columns>
            <asp:BoundField DataField="Titulo" HeaderText="Titulo" />
            <asp:TemplateField HeaderText="Version">
                <ItemTemplate>
                    <asp:Literal runat="server" ID="lVersion" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Tipo doc." DataField="TipoDocumento" />
            <asp:TemplateField HeaderText="Caduca" ItemStyle-HorizontalAlign="Center" >
                <ItemTemplate>
                    <asp:CheckBox Enabled="false" runat="server" ID="chkCaduca" Checked='<%# Eval("Caduca") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FechaCaducidad" HeaderText="Fecha caducidad" />
            <asp:TemplateField HeaderText="Eliminado" ItemStyle-HorizontalAlign="Center" >
                <ItemTemplate>
                    <asp:CheckBox Enabled="false" runat="server" ID="chkEliminado" Checked='<%# Eval("Eliminado") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fichero">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Target="_blank" ID="lnkFichero" Text="Fichero" NavigateUrl='<%# Eval("Fichero_Id","~/handlers/opendoc.ashx?id={0}") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Estado" DataField="Estado" />
            <asp:BoundField HeaderText="Etapa" DataField="Etapa" />
        </Columns>
        <PagerSettings Mode="Numeric" PageButtonCount="10" Position="Bottom" />
    </asp:GridView>
</asp:Content>
