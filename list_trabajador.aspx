
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="list_trabajador.aspx.cs" Inherits="Prestlan.list_trabajador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h2><span class="icon-user-small"></span><%= IBD.Web.Traduce.getTG("listartrabajador") %></h2>

    <asp:Panel runat="server" ID="pnlFiltroEmpresa">
        Empresa
        <asp:ListBox ID="lbEmpresas" runat="server" SelectionMode="Multiple" SelectMethod="GetEmpresas" DataValueField="Id" DataTextField="Nombre" />

        <asp:LinkButton runat="server" id="btnBuscar" CssClass="button" OnClick="btnBuscar_Click">
            <span class="icon-search"></span>
            Buscar
        </asp:LinkButton>
    </asp:Panel>


    <asp:GridView ID="GridView1" CssClass="table" runat="server" AutoGenerateColumns="False" 
         OnSorting="GridView1_Sorting"
         OnRowDataBound="GridView1_RowDataBound" DataKeyNames="Id" CellPadding="3">
        <Columns>
            <%--<asp:TemplateField>                
                <ItemTemplate>  
                    <input name="rbTrabajador" type="radio" value='<%# Eval("Id") %>' />                                 
                </ItemTemplate>
            </asp:TemplateField> --%>  
            <asp:BoundField DataField="Nombre" HeaderText="Nombre"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Empresa" HeaderText="Empresa" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="ValorIdentificador" HeaderText="Doc. Identidad" ItemStyle-HorizontalAlign="Left" />
<%--            <asp:BoundField DataField="Empleado" HeaderText="Empleado" ItemStyle-HorizontalAlign="Left" />--%>
            <asp:BoundField DataField="Actividad" HeaderText="Actividad" ItemStyle-HorizontalAlign="Left" />
            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="FechaAlta" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"  ItemStyle-HorizontalAlign="Left" />     
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HiddenField ID="HiddenIdTrabajador" runat="server" Value='<%# Eval("Id") %>' />                       
                    <asp:HyperLink runat="server" NavigateUrl='<%# Eval("Id", "trabajador.aspx?mode=Edit&id={0}") %>' CssClass="button-grey"><span class="icon-editar"></span></asp:HyperLink>&nbsp;
                    <asp:LinkButton ID="DeleteButton" runat="server" OnClick="DeleteButton_Click" CssClass="icon-remove" OnClientClick='<%# Prestlan.Utilidades.Confirmacion.crearConfirmacion("estasegurotrabajador") %>'><span class="icon-cancelar-small"></span></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>                   
        </Columns>
    </asp:GridView>
    <br />
    <%--<asp:LinkButton ID="lbEditar" runat="server" OnClick="lbEditar_Click" CssClass="button"><span class="icon-editar-pen"></span><%= IBD.Web.Traduce.getTG("editar") %></asp:LinkButton>--%>
    <!--<span class="icon-plus"></span>Asignar-->
     <!--<span class="icon-anadir"></span>Añadir-->

    

</asp:Content>
