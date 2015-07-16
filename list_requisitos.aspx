<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="list_requisitos.aspx.cs" Inherits="Prestlan.list_requisitos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.list_requisitos.js")%>"></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:GridView ID="GridViewRequisitos" CssClass="table" runat="server" AutoGenerateColumns="False" OnPreRender="GridViewRequisitos_PreRender"
        EnableViewState="False" CellPadding="3" DataKeyNames="RequisitoId,TipoDocumentoId" OnSorting="GridViewRequisitos_Sorting" >
        <Columns>
            <asp:BoundField DataField="RequisitoId" Visible="false" />      
            <asp:BoundField DataField="TipoDocumentoId" Visible="false" />                                
            <asp:BoundField DataField="Requisito" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="TipoDocumento" HeaderText="Tipo documento" ItemStyle-HorizontalAlign="Left" />      
        </Columns>
    </asp:GridView>
</asp:Content>
