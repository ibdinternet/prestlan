<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="generar_zip.aspx.cs" Inherits="Prestlan.generar_zip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.generar_zip.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><span class="icon-download3"></span>&nbsp;<%= IBD.Web.Traduce.getTG("generarzip") %></h2>

    <span style="color:red;">
        <asp:Literal runat="server" ID="msgValidacion" />
    </span>
    <br />
    <span>Nombre del fichero (sin extension)</span>
    <br />
    <asp:TextBox runat="server" ID="txtNombreFichero" Width="338" />
    <br />
    <span><%= IBD.Web.Traduce.getTG("requisitos") %></span>
    <br />
    <asp:ListBox runat="server" CssClass="lbRequisitos" ID="lbRequisitos" SelectMethod="lbRequisitos_GetData" DataTextField="descripcion" DataValueField="Id" SelectionMode="Multiple" />
    <asp:Repeater runat="server" ID="Repeater1" OnItemDataBound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table class="table table-generarzip">
                <thead>
                    <tr>
                        <th><%= IBD.Web.Traduce.getTG("empresa") %></th>
                        <th><%= IBD.Web.Traduce.getTG("trabajadores") %></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="col-empresa">
                    <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="true" DataTextField="Nombre" DataValueField="Id" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" />
                </td>
                <td>
                    <asp:ListBox ID="lbTrabajadores" CssClass="lbTrabajadores" runat="server" SelectionMode="Multiple" DataTextField="Nombre" DataValueField="Id" />
                </td>
                <td class="col-eliminar-fila">
                    <asp:LinkButton runat="server" ID="lnkEliminarFila" OnClick="lnkEliminarFila_Click">
                                <span class="icon-cancelar-small"></span>
                    </asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
                    </table>
        </FooterTemplate>
    </asp:Repeater>
    <br />
    <asp:LinkButton ID="AddNewRow" CssClass="button" runat="server" OnClick="AddNewRow_Click">
        <span class="icon-plus"></span>
        <%= IBD.Web.Traduce.getTG("insertarnuevo") %>
    </asp:LinkButton>
    <asp:LinkButton ID="ComprobarDoc" CssClass="button" runat="server" OnClick="ComprobarDoc_Click">
        <span class="icon-checkmark"></span>&nbsp;
        Comprobar documentación
    </asp:LinkButton>
    <asp:LinkButton ID="GenerarZIP" CssClass="button" runat="server" OnClick="GenerarZIP_Click">
        <span class="icon-download"></span>&nbsp;
        <%= IBD.Web.Traduce.getTG("generarzip") %>
    </asp:LinkButton>

    <br />
    <br />
    <asp:GridView runat="server" ID="GridViewDocEmpresa" AutoGenerateColumns="false" DataKeyNames="Empresa_Id">
        <Columns>
            <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
            <asp:BoundField DataField="Requisito" HeaderText="Requisito" />
            <asp:BoundField DataField="TipoDocumento" HeaderText="Tipo de documento" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Panel Width="20" Height="20" runat="server" BackColor='<%# Eval("Documento_Id") == null ? System.Drawing.Color.Red : System.Drawing.Color.LightGreen %>'>
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <asp:GridView runat="server" ID="GridViewDocTrabajadores" AutoGenerateColumns="false" DataKeyNames="Trabajador_Id">
        <Columns>
            <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
            <asp:BoundField DataField="Trabajador" HeaderText="Trabajador" />
            <asp:BoundField DataField="Requisito" HeaderText="Requisito" />
            <asp:BoundField DataField="TipoDocumento" HeaderText="Tipo de documento" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Panel Width="20" Height="20" runat="server" BackColor='<%# Eval("Documento_Id") == null ? System.Drawing.Color.Red : System.Drawing.Color.LightGreen %>'>
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
