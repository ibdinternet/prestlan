<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="adm_tipotrabajadores.aspx.cs" Inherits="Prestlan.adm_tipotrabajador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="msgError" Text="Error" runat="server" Visible="false" />

    <asp:ListView ID="ListView1" runat="server" 
        DataKeyNames="Id"         
        ItemType="Prestlan.Models.ListTipoTrabajador"
        OnItemInserted="ListView1_ItemInserted"
        OnItemEditing="ListView1_ItemEditing"
        InsertMethod="ListView1_InsertItem"
        UpdateMethod="ListView1_UpdateItem" 
        DeleteMethod="ListView1_DeleteItem"
        SelectMethod="ListView1_GetData"
        OnLayoutCreated="ListView1_LayoutCreated">
        <EmptyDataTemplate>
            No hay tipos de trabajador disponibles
        </EmptyDataTemplate>
        <LayoutTemplate>
              <h2><span class="icon-user-small"></span>
                  <asp:Label ID="lblTiposdetrabajadores" runat="server" Text="" /></h2>
            <table class="table">
                <thead>
                    <tr>                        
                        <th>
                            <asp:LinkButton Text="Descripción" CommandName="Sort" CommandArgument="Descripcion" runat="Server" />
                        </th>                                                                                                                                               
                        <th>
                            <asp:LinkButton Text="¿Activo?" CommandName="Sort" CommandArgument="Activo" runat="Server" />
                        </th>                                                                                                                                               
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody>
                    <tr runat="server" id="itemPlaceholder" />
                </tbody>
            </table>
            <br />

            <asp:DataPager PageSize="5" runat="server">
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
                    <asp:Label ID="lblDescripcion" runat="server" Text='<%# Item.Descripcion %>' />
                </td>                                               
                <td>                    
                    <asp:CheckBox ID="cbActivo" runat="server" Checked='<%# Item.Activo %>' Enabled="false" />
                </td>
                <td>
                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="icon-editar"  />  |     
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" CssClass="icon-remove" OnClientClick='<%# Prestlan.Utilidades.Confirmacion.crearConfirmacion("estasegurotrabajador") %>' />                                      
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <td>  
                    <asp:TextBox ID="tbDescripcion" runat="server" Text='<%# Item.Descripcion %>' />
                </td>                
                    <td>                    
                    <asp:CheckBox ID="cbActivo" runat="server" Checked='<%# Item.Activo %>' />
                </td>                       
                <td>                    
                     <asp:LinkButton ID="InsertButton" runat="server" CommandName="Update" CssClass="icon-guardar-small" />
                     <asp:LinkButton ID="CancelUpdate" runat="server" CommandName="Cancel" CssClass="icon-cancelar-small" />
                </td>
            </tr>
        </EditItemTemplate>
        <InsertItemTemplate>
            <tr>
                <td>                     
                    <asp:TextBox ID="tbDescripcion" runat="server" Text='<%# Bind("Descripcion") %>' />
                </td>

                 <td>                     
                    <asp:CheckBox ID="cbActivo" runat="server" Checked='<%# Bind("Activo") %>' />
                </td>
               
                <td>
                    
                     <asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert"  CssClass="icon-guardar-small"  />
                     <asp:LinkButton ID="CancelInsert" runat="server" CommandName="Cancel" OnClick="CancelInsert_Click" CssClass="icon-cancelar-small" />
                </td>
            </tr>
        </InsertItemTemplate>
    </asp:ListView>

    <asp:LinkButton ID="lbNuevo" runat="server" OnClick="lbNuevo_Click" CssClass="button"><span class="icon-plus"></span>Insertar nuevo</asp:LinkButton>
</asp:Content>
