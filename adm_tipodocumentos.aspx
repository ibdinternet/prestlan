<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="adm_tipodocumentos.aspx.cs" Inherits="Prestlan.adm_tipodocumentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Label ID="msgError" Text="Error" runat="server" Visible="false" />

    <asp:ListView ID="ListView1" runat="server"
        DataKeyNames="Id"
        ItemType="Prestlan.Models.ListTipoDocumento"
        OnItemInserted="ListView1_ItemInserted"
        OnItemEditing="ListView1_ItemEditing"
        DeleteMethod="ListView1_DeleteItem"
        InsertMethod="ListView1_InsertItem"
        UpdateMethod="ListView1_UpdateItem"
        SelectMethod="GetData">
        <EmptyDataTemplate>
            No hay tipos de documentos
       
        </EmptyDataTemplate>
        <LayoutTemplate>
            <h2><span class="icon-doc-small"></span>
                <asp:Label ID="lblTiposdedocumentos" runat="server" Text="Tipos de documentos" /></h2>
            <table class="table">
                <thead>
                    <tr>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="Descripcion" runat="Server"><%= IBD.Web.Traduce.getTG("descripcion") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="TextoAyuda" runat="Server"><%= IBD.Web.Traduce.getTG("textodeayuda") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="Plantilla" runat="Server"><%= IBD.Web.Traduce.getTG("plantilla") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="MultiplesPropietarios" runat="Server"><%= IBD.Web.Traduce.getTG("multiplespropietarios") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="NivelCriticidad" runat="Server"><%= IBD.Web.Traduce.getTG("nivelcriticidad") %></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton CommandName="Sort" CommandArgument="EsDocumentoDeEmpresa" runat="Server">Doc. empresa</asp:LinkButton>
                        </th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody>
                    <tr runat="server" id="itemPlaceholder" />
                </tbody>
            </table>
            <br />

            <asp:DataPager PageSize="30" runat="server">
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
                    <asp:Label ID="lblTextoAyuda" runat="server" Text='<%# Item.TextoAyuda %>' />
                </td>
                <td>
                    <asp:Label ID="lblPlantilla" runat="server" Text='<%# Item.Plantilla %>' />
                </td>

                <td>
                    <asp:CheckBox ID="cbMultiplesPropietarios" runat="server" Enabled="false" Checked='<%# Item.MultiplesPropietarios %>' />
                </td>
                <td>
                    <asp:Label ID="lblNivelCriticidad" runat="server" Text='<%# Item.NivelCriticidad %>' />
                </td>
                <td>
                    <asp:CheckBox ID="chkEsDocumentoDeEmpresa" runat="server" Enabled="false" Checked='<%# Item.EsDocumentoDeEmpresa %>' />
                </td>
                <td>
                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="icon-editar"></asp:LinkButton> &nbsp;  
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" CssClass="icon-remove" OnClientClick='<%# Prestlan.Utilidades.Confirmacion.crearConfirmacion("estasegurodocumento") %>'><span class="icon-cancelar-small"></span></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="tbDescripcion" runat="server" Text='<%# Bind("Descripcion") %>' />
                </td>

                <td>
                    <asp:TextBox ID="tbTextoAyuda" runat="server" Text='<%# Bind("TextoAyuda") %>' />
                </td>

                <td>
                    <asp:Label ID="lblPlantilla" runat="server" Text='<%# Bind("Plantilla") %>' /><br />
                    <asp:FileUpload ID="fuPlantilla" runat="server" />
                </td>

                <td>
                    <asp:CheckBox ID="cbMultiplesPropietarios" runat="server" Enabled="true" Checked='<%# Bind("MultiplesPropietarios") %>' />
                </td>
                <td>
                    <asp:TextBox ID="tbNivelCriticidad" runat="server" Text='<%# Bind("NivelCriticidad") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="chkEsDocumentoDeEmpresa" runat="server" Enabled="true" Checked='<%# Bind("EsDocumentoDeEmpresa") %>' />
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
                    <asp:TextBox ID="tbTextoAyuda" runat="server" Text='<%# Bind("TextoAyuda") %>' />
                </td>

                <td>
                    <asp:FileUpload ID="fuPlantilla" runat="server" />
                </td>

                <td>
                    <asp:CheckBox ID="cbMultiplesPropietarios" runat="server" Enabled="true" Checked='<%# Bind("MultiplesPropietarios") %>' />
                </td>
                <td>
                    <asp:TextBox ID="tblNivelCriticidad" runat="server" Text='<%# Bind("NivelCriticidad") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="chkEsDocumentoDeEmpresa" runat="server" Enabled="true" Checked='<%# Bind("EsDocumentoDeEmpresa") %>' />
                </td>
                <td>

                    <asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" CssClass="icon-guardar-small" />
                    <asp:LinkButton ID="CancelInsert" runat="server" CommandName="Cancel" OnClick="CancelInsert_Click" CssClass="icon-cancelar-small" />

                </td>
            </tr>
        </InsertItemTemplate>
    </asp:ListView>

    <asp:LinkButton ID="lbNuevo" runat="server" OnClick="lbNuevo_Click" CssClass="button"><span class="icon-plus"></span><%= IBD.Web.Traduce.getTG("insertarnuevo") %></asp:LinkButton>
</asp:Content>
