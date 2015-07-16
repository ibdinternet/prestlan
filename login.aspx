<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Prestlan.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
      <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <title>PRESTLAN-Iniciar sesión</title>
   
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Content/login.css")%>" />
</head>
<body class="bgimage">
    <form id="form1" runat="server">
        <div class="login_bloq">
            <figure>
            <img src="Images/logoMov.png" alt="Aplicación Prestlan, aplicación para la gestión  documental de actividades empresariales gestinado por prestadoras de servicios">            </figure>
            <div class="formu">
                <asp:Login ID="Login1" runat="server" LoginButtonText="Entrar" OnAuthenticate="Login1_Authenticate" OnLoggedIn="Login1_LoggedIn"
                    TitleText="Bienvenido a la aplicación PRESTLAN. Software creado por IBD Internet para la gestión de actividades empresariales de subcontratas para &quot;Empresa&quot;. Introduzca su usuario y contraseña.Si tiene problemas para acceder, no dude en contactar con nosotros. Tel: 943376085" UserNameLabelText="Usuario:">
                    <LayoutTemplate>
                        <table cellspacing="0" cellpadding="1" style="border-collapse: collapse;">
                            <tr>
                                <td>
                                    <table cellpadding="0">
                                        <tr>
                                            <td align="center" colspan="2"><span>Bienvenido a la aplicación PRESTLAN. Software creado por IBD Internet para la gestión de actividades empresariales de subcontratas para "Empresa". Introduzca su usuario y contraseña.Si tiene problemas para acceder, no dude en contactar con nosotros. Tel: 943376085</span></td>
                                        </tr>
                                        <tr>
                                          
                                            
                                            <td>
                                                <asp:TextBox runat="server" ID="UserName"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" ErrorMessage="El nombre de usuario es obligatorio." ValidationGroup="Login1" ToolTip="El nombre de usuario es obligatorio." ID="UserNameRequired">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            
                                               
                                            <td>
                                                <asp:TextBox runat="server" TextMode="Password" ID="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" ErrorMessage="La contrase&#241;a es obligatoria." ValidationGroup="Login1" ToolTip="La contrase&#241;a es obligatoria." ID="PasswordRequired">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox runat="server" Text="Record&#225;rmelo la pr&#243;xima vez." ID="RememberMe"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red;">
                                                <asp:Literal runat="server" ID="FailureText" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                <asp:Button runat="server" CommandName="Login" Text="Entrar" ValidationGroup="Login1" ID="LoginButton"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:Login>
            </div>
        </div>
    </form>
</body>
</html>
