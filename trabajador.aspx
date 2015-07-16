<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="trabajador.aspx.cs" Inherits="Prestlan.trabajador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.trabajador.js")%>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><span class="icon-user-small"></span><%= IBD.Web.Traduce.getTG("añadirtrabajador") %></h2>

    <asp:Panel runat="server" ID="panelError" Visible="false" CssClass="ErrorBox">
        <asp:Label ID="msgError" runat="server" Text="" />
    </asp:Panel>

    <asp:Panel runat="server" ID="panelEstado" Visible="false" CssClass="OkBox">
        <asp:Label ID="msgEstado" runat="server" Text="" />
    </asp:Panel>

    <asp:FormView runat="server" ID="FormView1"
        DataKeyNames="Id"
        ItemType="Prestlan.Models.Trabajador"
        DefaultMode="Insert"
        SelectMethod="FormView1_GetItem"
        UpdateMethod="FormView1_UpdateItem"
        InsertMethod="FormView1_InsertItem"
        OnInit="FormView1_Init"
        OnItemCommand="FormView1_ItemCommand" RenderOuterTable="False">

        <EditItemTemplate>
            <asp:ValidationSummary runat="server" DisplayMode="BulletList" CssClass="ErrorBox" />
            <br />
            <fieldset>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("empresa") %>:</label>
                    <asp:DropDownList ID="ddEmpresa" runat="server"
                        Enabled="false"
                        AppendDataBoundItems="true"
                        SelectMethod="GetEmpresas"
                        DataTextField="Nombre"
                        DataValueField="Id" SelectedValue='<%# Bind("Empresa_Id") %>'>
                        <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddEmpresa"
                        InitialValue="-1"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_empresa") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("autonomo") %>:</label>
                    <asp:CheckBox ID="cbEmpleado" runat="server" Checked='<%# Bind("Autonomo") %>' />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("activo") %></label>
                    <asp:CheckBox ID="cbActivo" runat="server" Checked='<%# Bind("Activo") %>' />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("identidad") %>:</label>
                    <asp:DropDownList ID="ddTipoidentificador" runat="server"
                        AppendDataBoundItems="true"
                        SelectMethod="GetTipoidentificador"
                        DataTextField="Descripcion"
                        DataValueField="TipoIdentificador_Id" SelectedValue='<%# Bind("TipoIdentificador_Id") %>'>
                        <asp:ListItem Value="-1">Seleccione elemento</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddTipoidentificador"
                        InitialValue="-1"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_tipoidentificador") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label></label>
                    <asp:TextBox ID="tbValorIdentificador" runat="server" Text='<%# Bind("ValorIdentificador") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbValorIdentificador"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_identificador") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("nombrecompleto") %>:</label>
                    <asp:TextBox ID="tbNombre" runat="server" Text='<%# Bind("Nombre") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbNombre"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_nombre") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />

                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("actividades") %>:</label>
                    <asp:ListBox ID="lbActividadesTrabajador" runat="server"
                        AppendDataBoundItems="true"
                        SelectMethod="GetActividades"
                        DataTextField="Descripcion"
                        DataValueField="Id"
                        Rows="5"
                        SelectionMode="Multiple" />
                    <asp:RequiredFieldValidator ControlToValidate="lbActividadesTrabajador"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_actividad") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("observaciones") %>:</label>

                    <asp:TextBox ID="tbObservaciones" runat="server" TextMode="multiline" Columns="50" Rows="5" Text='<%# Bind("Observaciones") %>' />
                </div>

                <div>
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" CssClass="button"><span class="icon-guardar"></span><%= IBD.Web.Traduce.getTG("guardar") %></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="button"><span class="icon-cancelar"></span><%= IBD.Web.Traduce.getTG("cancelar") %></asp:LinkButton>
                </div>
            </fieldset>
        </EditItemTemplate>

        <InsertItemTemplate>
            <asp:ValidationSummary runat="server" CssClass="ErrorBox" />
            <br />
            <fieldset>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("empresa") %>:</label>

                    <asp:DropDownList ID="ddEmpresa" CssClass="" runat="server" SelectedValue='<%# Bind("Empresa_Id") %>'
                        Enabled="true"
                        AppendDataBoundItems="true"
                        SelectMethod="GetEmpresas"
                        DataTextField="Nombre"
                        DataValueField="Id">
                        <asp:ListItem Value="-1" Selected="True">Seleccione elemento</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddEmpresa"
                        InitialValue="-1"
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_empresa") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("autonomo") %>:</label>
                    <asp:CheckBox ID="cbEmpleado" runat="server" Checked='<%# Bind("Autonomo") %>' />

                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("identidad") %>:</label>
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
                </div>
                <div>
                    <label></label>
                    <asp:TextBox ID="tbValorIdentificador" runat="server" Text='<%# Bind("ValorIdentificador") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbValorIdentificador"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_identificador") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>
                <div>
                    <label><%= IBD.Web.Traduce.getTG("nombrecompleto") %>:</label>
                    <asp:TextBox ID="tbNombre" runat="server" Text='<%# Bind("Nombre") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbNombre"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_nombre") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />

                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("actividades") %>:</label>
                    <asp:ListBox ID="lbActividadesTrabajador" runat="server"
                        AppendDataBoundItems="true"
                        SelectMethod="GetActividades"
                        DataTextField="Descripcion"
                        DataValueField="Id"
                        Rows="5"
                        SelectionMode="Multiple" />
                    <asp:RequiredFieldValidator ControlToValidate="lbActividadesTrabajador"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage='<%# IBD.Web.Traduce.getTG("camporequerido_actividad") %>'
                        Text="<span class='errorcampo'></span>"
                        runat="server" />
                </div>

                <div>
                    <label><%= IBD.Web.Traduce.getTG("observaciones") %>:</label>

                    <asp:TextBox ID="tbObservaciones" runat="server" Text='<%# Bind("Observaciones") %>' TextMode="multiline" Columns="50" Rows="5" />
                </div>

                <div>
                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" CssClass="button"><span class="icon-guardar"></span><%= IBD.Web.Traduce.getTG("guardar") %></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="button"><span class="icon-cancelar"></span><%= IBD.Web.Traduce.getTG("cancelar") %></asp:LinkButton>
                </div>
            </fieldset>
        </InsertItemTemplate>

    </asp:FormView>


</asp:Content>
