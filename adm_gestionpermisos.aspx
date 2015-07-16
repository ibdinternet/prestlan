<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="adm_gestionpermisos.aspx.cs" Inherits="Prestlan.adm_gestionpermisos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divError" style="visibility: hidden;" runat="server">
    <asp:Label ID="msgError" Text="Error" runat="server" />
    </div>

    <h2><span class="icon-user2"></span><%= IBD.Web.Traduce.getTG("gestiondepermisos") %></h2>
    <asp:ListView ID="ListView1" runat="server"  
        DataKeyNames="Id"
        ItemType="Prestlan.Models.ListPermisos"
        OnItemDataBound="ListView1_ItemDataBound"
        UpdateMethod="ListView1_UpdateItem"
        SelectMethod="ListView1_GetData"
        OnItemEditing="ListView1_ItemEditing">
        <EmptyDataTemplate>
            No hay permisos
        </EmptyDataTemplate>
        <LayoutTemplate>
                
            <table class="table">
                <thead>
                    <tr>
                        <th><%--<%= IBD.Web.Traduce.getTG("permisos") %>--%></th>                       
                        <th>Admin</th>                        
                        <th>SPA/Validador</th>                        
                        <th>Subcontrata</th>
                        <th>Free</th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody>
                    <tr runat="server" id="itemPlaceholder" />
                </tbody>
            </table>
            <br />

           
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="lblDescripcion" runat="server" Text='<%# Item.Descripcion %>'/>
                </td>
                           
                <td>
                    <asp:CheckBox ID="cbAdmin" runat="server" Enabled="false" />
                </td>               

                <td>
                    <asp:CheckBox ID="cbValidador" runat="server" Enabled="false" />
                </td>

                <td>
                    <asp:CheckBox ID="cbSubcontrata" runat="server" Enabled="false" />
                </td>

                <td>
                    <asp:CheckBox ID="cbFree" runat="server" Enabled="false" />
                </td>

                <td>
                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="icon-editar" />
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="tbNombre" runat="server" Text='<%# Item.Descripcion %>' />
                </td>                

               <td>
                    <asp:CheckBox ID="cbAdmin" runat="server" />
                </td>               

                <td>
                    <asp:CheckBox ID="cbValidador" runat="server" />
                </td>

                <td>
                    <asp:CheckBox ID="cbSubcontrata" runat="server" />
                </td>

                <td>
                    <asp:CheckBox ID="cbFree" runat="server" />
                </td>             

                <td>
                    <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" CssClass="icon-guardar-small"></asp:LinkButton>
                    <asp:LinkButton ID="CancelUpdate" runat="server" CommandName="Cancel" CssClass="icon-cancelar-small"></asp:LinkButton>
                </td>
            </tr>
        </EditItemTemplate>
        
    </asp:ListView>    

</asp:Content>
