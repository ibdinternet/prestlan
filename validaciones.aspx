<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="validaciones.aspx.cs" Inherits="Prestlan.validaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.validaciones.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        <span class="icon-valida-small"></span><%= IBD.Web.Traduce.getTG("validaciones") %></h2>
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
        <asp:LinkButton ID="lbPropietario" OnClick="lbPropietario_Click" CssClass="btn btn-default" runat="server">Propietario</asp:LinkButton>
        <asp:LinkButton ID="lbTipoDocumento" OnClick="lbTipoDocumento_Click" CssClass="btn btn-default linkactive" runat="server">Tipo Documento</asp:LinkButton>
    </div>
    <br />
    <asp:DropDownList ID="ddOrdenacion" CssClass="float_right" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddOrdenacion_SelectedIndexChanged">
    </asp:DropDownList>

    <div id="divDialog" runat="server">
        <asp:Label ID="lblTitulo" runat="server" />
        <br />
        <asp:Label ID="lblDescripcion" runat="server" />
        <br />
        <span class="tit"><%= IBD.Web.Traduce.getTG("subidopor") %>: </span>
        <asp:Label ID="lblAutor" runat="server" />
        <br />
        <span class="tit"><%= IBD.Web.Traduce.getTG("version") %>:</span>
        <asp:Label ID="lblVersion" runat="server" />
        <br />
        <span class="tit"><%= IBD.Web.Traduce.getTG("estado") %>:</span>
        <asp:Label ID="lblEstado" runat="server" />
        <br />
        <br />
        <div class="icon-dialog">
            <a href="#" id="bValidar" class="button-icon"><span class="icon-checkmark"></span>
                <!--Validar-->
            </a>
            <a href="#" id="bEditar" class="button-icon"><span class="icon-editar2"></span>
                <!--Editar-->
            </a>
            <a href="#" id="bSubir" class="button-icon"><span class="icon-upload"></span>
                <!--Subir-->
            </a>
            <a href="#" id="bDescargar" class="button-icon"><span class="icon-download"></span>
                <!--Descargar-->
            </a>
        </div>
    </div>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
        OnSorting="GridView1_Sorting" OnDataBound="GridView1_DataBound"
        OnRowDataBound="GridView1_RowDataBound"
        EnableViewState="False" DataKeyNames="Id" CellPadding="3" CssClass="table">
        <EmptyDataTemplate>
            <%= IBD.Web.Traduce.getTG("nosehanencontradodocs") %>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:RadioButton runat="server" ID="rbDocumento" OnClick="javascript: selectRB(this);" />
                    <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("id") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Id" Visible="false" />

            <asp:TemplateField Visible="false">
                <HeaderTemplate>
                    Tipo de documento
               
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink ID="hlTipoDocumento" Target="_blank" runat="server" NavigateUrl='<%# "handlers/opendoc.ashx?id=" + DataBinder.Eval(Container.DataItem, "Fichero_id") + "" %>' Text='<%# Bind("Nombre") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField Visible="true">
                <HeaderTemplate>
                    Nombre
               
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink ID="hlTipoDocumento" Target="_blank" runat="server" NavigateUrl='<%# "handlers/opendoc.ashx?id=" + DataBinder.Eval(Container.DataItem, "Fichero_id") + "" %>' Text='<%# Bind("TipoDocumento") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    Versiones
               
                </HeaderTemplate>
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Creador" HeaderText="Creador" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200" />
            <%--<asp:BoundField DataField="Revisor" HeaderText="Revisor" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200" />--%>
        </Columns>
    </asp:GridView>
    <br />
    <asp:Panel runat="server" ID="pnlAcciones">
        <a href="#" id="bRechazarDocumento" class="button" title="Rechazar"><span class="icon-cancelar"></span><%= IBD.Web.Traduce.getTG("rechazar") %></a>&nbsp;   
        <a href="#" id="bValidarDocumento" class="button" title="Validar"><span class="icon-validar"></span><%= IBD.Web.Traduce.getTG("validar") %></a>
    </asp:Panel>  

</asp:Content>
