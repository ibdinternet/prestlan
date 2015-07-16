<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Prestlan.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="NoMenu">
    <ul class="grid cs-style-4">
        <li class="verde" runat="server" id="li_documentos">

            <div>
                <a href='<%=ResolveUrl("~/list_documentos.aspx") %>'>
                    <span class="icon-docs white-doc"></span>
                  
                    <span class="txt"><%= IBD.Web.Traduce.getTG("documentos") %></span>
                </a>
            </div>
        </li>
              <li class="verde" runat="server" id="li_trabajadores">
            <div>
                <a href='<%=ResolveUrl("~/list_trabajador.aspx") %>'>
                    <span class="icon-user white"></span>
                       
                        <span class="txt"><%= IBD.Web.Traduce.getTG("trabajadores") %></span></a>
            </div>
        </li>
        <li class="verde" runat="server" id="li_empresas">
            <div>
                <a href='<%=ResolveUrl("~/list_empresa.aspx") %>'>
                    <span class="icon-office white"></span>
                 
                    <span class="txt"><%= IBD.Web.Traduce.getTG("subcontratas") %></span></a>
            </div>
        </li>
        <li class="verde" runat="server" id="li_edicionEmpresa">
            <div>
                <asp:HyperLink runat="server" ID="linkEdicionEmpresaMenu">
                    <span class="icon-office white"></span>                 
                    <span class="txt"><%= IBD.Web.Traduce.getTG("empresa") %></span>
                </asp:HyperLink>
            </div>
        </li>
        <li class="verde" runat="server" id="li_homologaciones">
            <div>
                <a href='<%=ResolveUrl("~/list_homologaciones.aspx") %>'>
                    <span class="icon-homologa white"></span>
                   
                    <span class="txt"><%= IBD.Web.Traduce.getTG("homologaciones") %></span></a>
            </div>
        </li>
        <li class="verde" runat="server" id="li_reclamaciones">

            <div>
                <a href='<%=ResolveUrl("~/reclamaciones.aspx") %>'>
                    <span class="icon-reclama white"></span>
                    <span class="txt"><%= IBD.Web.Traduce.getTG("reclamaciones") %></span></a>
            </div>


        </li>
  
        <li class="verde" runat="server" id="li_administracion">
            <div>
                <a href='<%=ResolveUrl("~/administracion.aspx") %>'>
                    <span class="icon-adm white-doco"></span>
                
                    <span class="txt"><%= IBD.Web.Traduce.getTG("administracion") %></span></a>
            </div>
        </li>
        <li class="verde" runat="server" id="li_validaciones">
            <div>
                <a href='<%=ResolveUrl("~/validaciones.aspx") %>'>
                    <span class="icon-valida white"></span>
                   
                    <span class="txt"><%= IBD.Web.Traduce.getTG("validaciones") %></span></a>
            </div>
        </li>
        <li class="verde" runat="server" id="li_requisitos">
            <div>
                <a href='<%=ResolveUrl("~/requisitos.aspx") %>'>
                 <span class="icon-requisitos white"></span>
                   
                    <span class="txt"><%= IBD.Web.Traduce.getTG("requisitos") %></span></a>
            </div>
        </li>
        <li class="verde" runat="server" id="li_generarzip">
            <div>
                <a href='<%=ResolveUrl("~/generar_zip.aspx") %>'>
                 <span class="icon-download3 white"></span>
                   
                    <span class="txt"><%= IBD.Web.Traduce.getTG("generarzip") %></span></a>
            </div>
        </li>
    </ul>
        </div>
</asp:Content>
