<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="Prestlan.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= IBD.Web.Traduce.getTG("versionaplicacion") %>: <asp:Label ID="lblVersion" runat="server" />
    <asp:FormView runat="server" ID="FormView1" RenderOuterTable="False"
        DataKeyNames="Id"
        ItemType="Prestlan.Models.Usuario"
        DefaultMode="Edit"
        SelectMethod="FormView1_GetItem"
        UpdateMethod="FormView1_UpdateItem">

        <EditItemTemplate>
            <asp:ValidationSummary runat="server" />
            <fieldset>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("nombreyapellido") %>:</label>
                    <asp:TextBox ID="tbNombre" runat="server" Text='<%# Bind("nombre") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbNombre"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage="Debe introducir un valor"
                        Text="*"
                        runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("contraseña") %>:</label>
                    <asp:TextBox ID="tbClave" runat="server" TextMode="Password" />                    
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("repetircontraseña") %>:</label>
                    <asp:TextBox ID="tbClave2" runat="server" TextMode="Password" />                    
                    <asp:CompareValidator ID="comparePasswords"
                        runat="server"
                        ControlToCompare="tbClave"
                        ControlToValidate="tbClave2"
                        ErrorMessage="Las claves no coinciden"/>
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("email") %>:</label>
                    <asp:Label ID="tbEmail" runat="server" Text='<%# Bind("email") %>' />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("idiomaaplicacion") %>:</label>
                    <asp:RadioButtonList ID="rbIdioma" runat="server" SelectedValue='<%# Bind("idioma") %>'>
                        <asp:ListItem Text="Castellano" Value="es"></asp:ListItem>
                        <asp:ListItem Text="Euskera" Value="eu"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>

                <div>
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" CssClass="button"><span class="icon-guardar"></span>Guardar</asp:LinkButton>
                </div>


            </fieldset>
        </EditItemTemplate>

    </asp:FormView>


</asp:Content>
