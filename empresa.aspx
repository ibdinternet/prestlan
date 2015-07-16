<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="empresa.aspx.cs" Inherits="Prestlan.empresa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.empresa.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><span class="icon-office-small"></span><%= IBD.Web.Traduce.getTG("empresa") %></h2>

    <asp:Panel runat="server" ID="panelError" Visible="false" CssClass="ErrorBox">
        <asp:Label ID="msgError" runat="server" Text="" />
    </asp:Panel>

    <asp:Panel runat="server" ID="panelEstado" Visible="false" CssClass="OkBox">
        <asp:Label ID="msgEstado" runat="server" Text="" />
    </asp:Panel>

    <asp:FormView runat="server" ID="FormView1"
        DataKeyNames="Id"
        ItemType="Prestlan.Models.Empresa"
        DefaultMode="Insert"
        SelectMethod="FormView1_GetItem"
        UpdateMethod="FormView1_UpdateItem"
        InsertMethod="FormView1_InsertItem"
        OnInit="FormView1_Init"
        OnItemCommand="ItemCommand"
        RenderOuterTable="False">

        <EditItemTemplate>
            <asp:ValidationSummary runat="server" DisplayMode="BulletList" CssClass="ErrorBox" />
            <br />
            <fieldset>
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label><%= IBD.Web.Traduce.getTG("identificador") %>:</label>
                    <asp:DropDownList ID="ddTipoidentificador" runat="server" SelectedValue='<%# Bind("TipoIdentificador_Id") %>'
                        AppendDataBoundItems="true"
                        SelectMethod="GetTipoidentificador"
                        DataTextField="Descripcion"
                        DataValueField="TipoIdentificador_Id">
                        <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddTipoidentificador"
                        InitialValue="-1"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_tipoidentificador") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </asp:Panel>
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label></label>
                    <asp:TextBox ID="valorIdentificadorTextBox" runat="server" Text='<%# Bind("valorIdentificador") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="valorIdentificadorTextBox"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_identificador") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </asp:Panel>
                <div>
                    <label>Nombre:</label>
                    <asp:TextBox ID="nombreTextBox" runat="server" Text='<%# Bind("nombre") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="nombreTextBox"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_nombre") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label>Dirección:</label>
                    <asp:TextBox ID="direccionTextBox" runat="server" Text='<%# Bind("direccion") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="direccionTextBox"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_direccion") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label><%= IBD.Web.Traduce.getTG("observaciones") %>:</label>
                    <asp:TextBox ID="observacionesTextBox" runat="server" Text='<%# Bind("Observaciones") %>' Rows="5" />
                </asp:Panel>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("correosnotificaciones") %>:</label>
                    <asp:TextBox ID="correosNotificacionesTextBox" runat="server" Text='<%# Bind("CorreosNotificaciones") %>' />
                </div>
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label><%= IBD.Web.Traduce.getTG("enviarnotificaciones") %></label>
                    <asp:CheckBox ID="sinNotificacionesCheckBox" runat="server" Checked='<%# Bind("sinNotificaciones") %>' />
                </asp:Panel>
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label><%= IBD.Web.Traduce.getTG("activa") %></label>
                    <asp:CheckBox ID="activaCheckBox" runat="server" Checked='<%# Bind("activa") %>' />
                </asp:Panel>
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label><%= IBD.Web.Traduce.getTG("fechadealta") %>:</label>
                    <asp:Label ID="lblFechaDeAlta" runat="server" Text='<%# Bind("fechaAlta", "{0:dd/MM/yyyy}") %>' />
                </asp:Panel>
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label><%= IBD.Web.Traduce.getTG("fechadebaja") %>:</label>
                    <asp:Label ID="lblFechaBaja" runat="server" Text='<%# Bind("fechaBaja", "{0:dd/MM/yyyy}") %>' />
                </asp:Panel>

                <br />
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label><%= IBD.Web.Traduce.getTG("tipodeempresa") %>:</label>
                    <asp:DropDownList ID="ddTipoempresa" runat="server" SelectedValue='<%# Bind("TipoEmpresa_Id") %>'
                        AppendDataBoundItems="true"
                        SelectMethod="GetTipoEmpresa"
                        DataTextField="descripcion"
                        DataValueField="Tipoempresa_Id">
                        <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddTipoempresa"
                        InitialValue="-1"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage="Debe introducir un valor"
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </asp:Panel>
                <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                    <label><%= IBD.Web.Traduce.getTG("actividad") %>:</label>
                    <asp:DropDownList ID="ddActividad" runat="server" SelectedValue='<%# Bind("Actividad_Id") %>'
                        AppendDataBoundItems="true"
                        SelectMethod="GetActividades"
                        DataTextField="descripcion"
                        DataValueField="Actividad_Id">
                        <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddActividad"
                        InitialValue="-1"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_tipoempresa") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                    <br />
                </asp:Panel>

                <div>
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" CssClass="button"><span class="icon-guardar"></span>Guardar</asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="button"><span class="icon-cancelar"></span>Cancelar</asp:LinkButton>
                </div>


            </fieldset>
        </EditItemTemplate>

        <InsertItemTemplate>
            <asp:ValidationSummary runat="server" DisplayMode="BulletList" CssClass="ErrorBox"/>
            <br />
            <fieldset>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label><%= IBD.Web.Traduce.getTG("identificador") %>:</label>
                        <asp:DropDownList ID="ddTipoidentificador" runat="server" SelectedValue='<%# Bind("TipoIdentificador_Id") %>'
                            AppendDataBoundItems="true"
                            SelectMethod="GetTipoidentificador"
                            DataTextField="Descripcion"
                            DataValueField="TipoIdentificador_Id">
                            <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddTipoidentificador"
                            InitialValue="-1"
                            EnableClientScript="true"
                            SetFocusOnError="true"
                            ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_tipoidentificador") %>'
                            Text="<span class='errorcampo'></span>"
                            runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label></label>
                        <asp:TextBox ID="tbvalorIdentificador" Text='<%# Bind("ValorIdentificador") %>' runat="server" />
                        <asp:RequiredFieldValidator ControlToValidate="tbvalorIdentificador"
                            InitialValue=""
                            EnableClientScript="true"
                            SetFocusOnError="true"
                            ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_identificador") %>'
                            Text="<span class='errorcampo'></span>"
                            runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label><%= IBD.Web.Traduce.getTG("nombre") %>:</label>
                        <asp:TextBox ID="tbnombre" Text='<%# Bind("Nombre") %>' runat="server" />
                        <asp:RequiredFieldValidator ControlToValidate="tbnombre"
                            InitialValue=""
                            EnableClientScript="true"
                            SetFocusOnError="true"
                            ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_nombre") %>'
                            Text="<span class='errorcampo'></span>"
                            runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label><%= IBD.Web.Traduce.getTG("direccion") %>:</label>
                        <asp:TextBox ID="tbdireccion" Text='<%# Bind("Direccion") %>' runat="server" />
                        <asp:RequiredFieldValidator ControlToValidate="tbdireccion"
                            InitialValue=""
                            EnableClientScript="true"
                            SetFocusOnError="true"
                            ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_direccion") %>'
                            Text="<span class='errorcampo'></span>"
                            runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label><%= IBD.Web.Traduce.getTG("observaciones") %>:</label>
                        <asp:TextBox ID="tbobservaciones" Text='<%# Bind("Observaciones") %>' runat="server" Rows="5" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label><%= IBD.Web.Traduce.getTG("correosnotificaciones") %>:</label>
                        <asp:TextBox ID="tbcorreosNotificaciones" Text='<%# Bind("CorreosNotificaciones") %>' runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label><%= IBD.Web.Traduce.getTG("enviarnotificaciones") %></label>
                        <asp:CheckBox ID="cbsinNotificaciones" runat="server" Checked='<%# Bind("sinNotificaciones") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label><%= IBD.Web.Traduce.getTG("tiupodeempresa") %>:</label>
                        <asp:DropDownList ID="ddTipoempresa" runat="server" SelectedValue='<%# Bind("TipoEmpresa_Id") %>'
                            AppendDataBoundItems="true"
                            SelectMethod="GetTipoEmpresa"
                            DataTextField="descripcion"
                            DataValueField="Tipoempresa_Id">
                            <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddTipoempresa"
                            InitialValue="-1"
                            EnableClientScript="true"
                            SetFocusOnError="true"
                            ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_tipoempresa") %>'
                            Text="<span class='errorcampo'></span>"
                            runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>Actividad:</label>
                        <asp:DropDownList ID="ddActividad" runat="server" SelectedValue='<%# Bind("Actividad_Id") %>'
                            AppendDataBoundItems="true"
                            SelectMethod="GetActividades"
                            DataTextField="descripcion"
                            DataValueField="Actividad_Id">
                            <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ControlToValidate="ddActividad"
                            InitialValue="-1"
                            EnableClientScript="true"
                            SetFocusOnError="true"
                            ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_actividad") %>'
                            Text="<span class='errorcampo'></span>"
                            runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" CssClass="button"><span class="icon-guardar"></span>Guardar</asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="button"><span class="icon-cancelar"></span>Cancelar</asp:LinkButton>
                    </asp:Panel>
            </fieldset>
        </InsertItemTemplate>

        <ItemTemplate>
            <fieldset>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>Id:</label>
                        <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>valorIdentificador:</label>
                        <asp:Label ID="valorIdentificadorLabel" runat="server" Text='<%# Bind("valorIdentificador") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>nombre:</label>
                        <asp:Label ID="nombreLabel" runat="server" Text='<%# Bind("nombre") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>direccion:</label>
                        <asp:Label ID="direccionLabel" runat="server" Text='<%# Bind("direccion") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>observaciones:</label>
                        <asp:Label ID="observacionesLabel" runat="server" Text='<%# Bind("observaciones") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>correosNotificaciones:</label>
                        <asp:Label ID="correosNotificacionesLabel" runat="server" Text='<%# Bind("correosNotificaciones") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>sinNotificaciones:</label>
                        <asp:CheckBox ID="sinNotificacionesCheckBox" runat="server" Checked='<%# Bind("sinNotificaciones") %>' Enabled="false" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>activa:</label>
                        <asp:CheckBox ID="activaCheckBox" runat="server" Checked='<%# Bind("activa") %>' Enabled="false" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>fechaAlta:</label>
                        <asp:Label ID="fechaAltaLabel" runat="server" Text='<%# Bind("fechaAlta") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>fechaBaja:</label>
                        <asp:Label ID="fechaBajaLabel" runat="server" Text='<%# Bind("fechaBaja") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>eliminado:</label>
                        <asp:CheckBox ID="eliminadoCheckBox" runat="server" Checked='<%# Bind("eliminado") %>' Enabled="false" />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>TipoIdentificador_Id:</label>
                        <asp:Label ID="TipoIdentificador_IdLabel" runat="server" Text='<%# Bind("TipoIdentificador_Id") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>TipoEmpresa_Id:</label>
                        <asp:Label ID="TipoEmpresa_IdLabel" runat="server" Text='<%# Bind("TipoEmpresa_Id") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>Actividad_Id:</label>
                        <asp:Label ID="Actividad_IdLabel" runat="server" Text='<%# Bind("Actividad_Id") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>Propietario:</label>
                        <asp:Label ID="PropietarioLabel" runat="server" Text='<%# Bind("Propietario") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>Trabajador:</label>
                        <asp:Label ID="TrabajadorLabel" runat="server" Text='<%# Bind("Trabajador") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>TipoEmpresa:</label>
                        <asp:Label ID="TipoEmpresaLabel" runat="server" Text='<%# Bind("TipoEmpresa") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <label>TipoIdentificador:</label>
                        <asp:Label ID="TipoIdentificadorLabel" runat="server" Text='<%# Bind("TipoIdentificador") %>' />
                    </asp:Panel>
                    <asp:Panel runat="server" Visible="<%#!ModoEdicionSubcontrata%>">
                        <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit" Text="Editar" />
                        &nbsp;<asp:LinkButton ID="DeleteButton" CssClass="button" runat="server" CausesValidation="False" CommandName="Delete" Text="Eliminar" />
                        &nbsp;<asp:LinkButton ID="NewButton" CssClass="button" runat="server" CausesValidation="False" CommandName="New" Text="Nuevo" />
                    </asp:Panel>
            </fieldset>
        </ItemTemplate>
    </asp:FormView>

</asp:Content>
