<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="adm_gestionusuarios.aspx.cs" Inherits="Prestlan.adm_gestionusuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.adm_gestionusuarios.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="msgError" Text="Error" runat="server" Visible="false" />

    <asp:ListView ID="ListView1" runat="server"
        DataKeyNames="Id"
        ItemType="Prestlan.Models.ListUsuario"
        OnDataBound="ListView1_DataBound"
        OnItemDataBound="ListView1_ItemDataBound"
        InsertMethod="ListView1_InsertItem"
        UpdateMethod="ListView1_UpdateItem"
        SelectMethod="ListView1_GetData"
        OnItemInserted="ListView1_ItemInserted"
        OnItemEditing="ListView1_ItemEditing"
        DeleteMethod="ListView1_DeleteItem"
        OnLayoutCreated="ListView1_LayoutCreated">
        <EmptyDataTemplate>
            No hay usuarios
       
        </EmptyDataTemplate>
        <LayoutTemplate>
            <h2><span class="icon-user2"></span>
                <asp:Label ID="lblGestiondeusuarios" runat="server" Text="Gestión de usuarios" /></h2>
            <table class="table">
                <thead>
                    <tr>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="Nombre" runat="Server"><%= IBD.Web.Traduce.getTG("nombredeusuario") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="Email" runat="Server"><%= IBD.Web.Traduce.getTG("email") %></asp:LinkButton>
                        </th>
                        <th id="thClave" runat="server" visible="false"><%= IBD.Web.Traduce.getTG("clave") %></th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="Rol" runat="Server"><%= IBD.Web.Traduce.getTG("rol") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="Empresa" runat="Server"><%= IBD.Web.Traduce.getTG("empresa") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="Fecha" runat="Server"><%= IBD.Web.Traduce.getTG("fechadealta") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="UltimoLogin" runat="Server"><%= IBD.Web.Traduce.getTG("ultimologin") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton Text="IP" CommandName="Sort" CommandArgument="Ip" runat="Server" />
                        </th>

                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody>
                    <tr runat="server" id="itemPlaceholder" />
                </tbody>
            </table>
            <br />

            <asp:DataPager PageSize="10" runat="server">
                <Fields>
                    <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="False" ButtonType="Button" ButtonCssClass="button_next" />
                    <asp:NumericPagerField ButtonType="Button" NumericButtonCssClass="btn" CurrentPageLabelCssClass="btn disabled" NextPreviousButtonCssClass="btn" />
                    <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button" ButtonCssClass="button_next" />
                </Fields>
            </asp:DataPager>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="lblNombreUsuario" runat="server" Text='<%# Item.Nombre %>' />
                </td>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text='<%# Item.Email %>' />
                </td>
                <td id="tdClave" runat="server" visible="false">
                    <asp:Label ID="lblClave" Text="********" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblRol" runat="server" Text='<%# Item.Rol %>' />
                </td>
                <td>
                    <asp:Label ID="lblEmpresa" runat="server" Text='<%# Item.Empresa %>' />
                </td>
                <td>
                    <asp:Label ID="lblFechaAlta" runat="server" Text='<%# Item.FechaAlta.ToShortDateString() %>' />
                </td>

                <td>
                    <asp:Label ID="lblUltimoLogin" runat="server" Text='<%# Item.UltimoLogin.HasValue? Item.UltimoLogin.Value.ToShortDateString() : "" %>' />
                </td>

                <td>
                    <asp:Label ID="lblIp" runat="server" Text='<%# Item.Ip %>' />
                </td>

                <td>
                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="icon-editar" /> &nbsp;                   
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" CssClass="icon-remove" OnClientClick='<%# Prestlan.Utilidades.Confirmacion.crearConfirmacion("estasegurousuario") %>'><span class="icon-cancelar-small"></span></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="tbNombreEdit" runat="server" Text='<%# Bind("Nombre") %>' />
                </td>
                <td>
                    <asp:TextBox ID="tbEmailEdit" runat="server" Text='<%# Bind("Email") %>' />
                </td>
                <td>
                    <asp:TextBox ID="tbClaveEdit" runat="server" Text='' TextMode="Password" />
                </td>

                <td>
                    <asp:DropDownList ID="ddRolEdit" runat="server" AppendDataBoundItems="true" CssClass="jsRol"
                        SelectedValue='<%# Bind("Rol_Id") %>'
                        SelectMethod="GetRoles"
                        DataTextField="descripcion"
                        DataValueField="Id">
                        <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddEmpresa" CssClass="jsEmpresa" runat="server" AppendDataBoundItems="true" SelectedValue='<%# Bind("Empresa_Id") %>'
                        SelectMethod="GetEmpresas"
                        DataTextField="Nombre"
                        DataValueField="Id">
                        <asp:ListItem Value="-1">Seleccione subcontrata</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblFechaAltaEdit" runat="server" Text='<%# Item.FechaAlta.ToShortDateString() %>' />
                </td>
                <td>
                    <asp:Label ID="lblUltimoLoginEdit" runat="server" Text='<%# Item.UltimoLogin.HasValue? Item.UltimoLogin.Value.ToShortDateString() : "" %>' />
                </td>
                <td>
                    <asp:Label ID="lblIpEdit" runat="server" Text='<%# Item.Ip %>' />
                </td>

                <td>
                    <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" CssClass="icon-guardar-small" />
                    <asp:LinkButton ID="CancelUpdate" runat="server" CommandName="Cancel" CssClass="icon-cancelar-small" />
                </td>
            </tr>
        </EditItemTemplate>
        <InsertItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="tbNombre" runat="server" Text='<%# Bind("Nombre") %>' />
                </td>
                <td>
                    <asp:TextBox ID="tbEmail" runat="server" Text='<%# Bind("Email") %>' />
                </td>
                <td>
                    <asp:TextBox ID="tbClave" runat="server" Text='<%# Bind("Clave") %>' />
                </td>
                <td>
                    <asp:DropDownList ID="ddRol" runat="server" AppendDataBoundItems="true" SelectedValue='<%# Bind("Rol_Id") %>' CssClass="jsRol"
                        SelectMethod="GetRoles"
                        DataTextField="descripcion"
                        DataValueField="Id">
                        <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddEmpresa" CssClass="jsEmpresa" runat="server" AppendDataBoundItems="true" SelectedValue='<%# Bind("Empresa_Id") %>'
                        SelectMethod="GetEmpresas"
                        DataTextField="Nombre"
                        DataValueField="Id">
                        <asp:ListItem Value="-1">Seleccione subcontrata</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <!-- Fecha Alta -->
                </td>
                <td>
                    <!-- Último login -->
                </td>
                <td>
                    <!-- Ip -->
                </td>
                <td>

                    <asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" CssClass="icon-guardar-small"></asp:LinkButton>
                    <asp:LinkButton ID="CancelInsert" runat="server" CommandName="Cancel" OnClick="CancelInsert_Click" CssClass="icon-cancelar-small"></asp:LinkButton>
                </td>
            </tr>
        </InsertItemTemplate>
    </asp:ListView>

    <asp:LinkButton ID="lbNuevo" runat="server" OnClick="lbNuevo_Click" CssClass="button"><span class="icon-plus"></span><%= IBD.Web.Traduce.getTG("insertarnuevo") %></asp:LinkButton>


</asp:Content>
