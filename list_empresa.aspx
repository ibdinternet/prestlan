<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="list_empresa.aspx.cs" Inherits="Prestlan.list_empresa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ListView ID="ListView1" runat="server"
        DataKeyNames="Id"
        ItemType="Prestlan.Models.ListEmpresa"
        SelectMethod="GetData"
        DeleteMethod="ListView1_DeleteItem">
        <EmptyDataTemplate>
            There are no entries found for Empresa
       
        </EmptyDataTemplate>
        <LayoutTemplate>
            <h2><span class="icon-office-small"></span>
                <asp:Label ID="lblListarSubcontrata" runat="server" Text="Listar subcontrata" /></h2>
            <table class="table">
                <thead>
                    <tr>
                        <th>
                            <asp:LinkButton ID="lnkNombre" Text="Nombre" CommandName="Sort" CommandArgument="nombre" runat="Server" />
                        </th>
                        <th>
                            <asp:LinkButton ID="lnkActiva" Text="¿Activa?" CommandName="Sort" CommandArgument="activa" runat="Server" />
                        </th>
                        <th>
                            <asp:LinkButton ID="lnkFechaAlta" Text="Fecha de Alta" CommandName="Sort" CommandArgument="fechaAlta" runat="Server" />
                        </th>
                        <th>
                            <asp:LinkButton ID="lnkFechaBaja" Text="Fecha de Baja" CommandName="Sort" CommandArgument="fechaBaja" runat="Server" />
                        </th>

                        <th>
                            <asp:LinkButton ID="lnkTipoEmpresa" Text="Tipo de empresa" CommandName="Sort" CommandArgument="TipoEmpresa_Id" runat="Server" />
                        </th>
                        <th>
                            <asp:LinkButton ID="lnkActividad" Text="Actividad" CommandName="Sort" CommandArgument="Actividad_Id" runat="Server" />
                        </th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody>
                    <tr runat="server" id="itemPlaceholder" />
                </tbody>
            </table>
            <br />

            <asp:DataPager PageSize="10" runat="server" ID="datapagerLV1">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="button_next"
                        ShowPreviousPageButton="true"
                        ShowFirstPageButton="false"
                        ShowLastPageButton="false"
                        ShowNextPageButton="false" />
                    <asp:NumericPagerField ButtonType="Button" NumericButtonCssClass="btn" CurrentPageLabelCssClass="btn disabled" NextPreviousButtonCssClass="btn" />
                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="button_next"
                        ShowPreviousPageButton="false"
                        ShowFirstPageButton="false"
                        ShowLastPageButton="false"
                        ShowNextPageButton="true" />
                </Fields>
            </asp:DataPager>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>

                <td>
                    <asp:Label ID="lblNombre" runat="server" Text='<%# Item.Nombre %>' />
                </td>

                <td>
                    <asp:CheckBox ID="cbActiva" runat="server" Enabled="false" Checked='<%# Item.Activa %>' />
                </td>
                <td>
                    <asp:Label ID="lblFechaAlta" runat="server" Text='<%# Item.FechaAlta.ToShortDateString() %>' />
                </td>
                <td>
                    <asp:Label ID="lblFechaBaja" runat="server" Text='<%# Item.FechaBaja.ToShortDateString() %>' />
                </td>

                <td>
                    <asp:Label ID="lblTipoEmpresa" runat="server" Text='<%# Item.TipoEmpresa %>' />
                </td>
                <td>
                    <asp:Label ID="lblActividad" runat="server" Text='<%# Item.Actividad %>' />
                </td>
                <td>
                    <asp:HyperLink Visible='<%#PermitirEdicion%>' runat="server" NavigateUrl='<%# "empresa.aspx?mode=Edit&Id=" + Item.Id %>' CssClass="button-grey"><span class="icon-editar"></span></asp:HyperLink>&nbsp;                   
                    <asp:LinkButton Visible='<%#PermitirEdicion%>' ID="DeleteButton" runat="server" CommandName="Delete" CssClass="icon-remove" OnClientClick='<%# Prestlan.Utilidades.Confirmacion.crearConfirmacion("estaseguroempresa") %>'><span class="icon-cancelar-small"></span></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>

</asp:Content>
