<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="list_notificaciones.aspx.cs" Inherits="Prestlan.list_notificaciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            ObtenerNotificaciones("#noti_list_pagina");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><span class="icon-user-small"></span><%= IBD.Web.Traduce.getTG("notificaciones") %></h2>
    <div id="noti_list_pagina">

    </div>
</asp:Content>
