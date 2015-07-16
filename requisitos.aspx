<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="requisitos.aspx.cs" Inherits="Prestlan.requisitos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/prestlan.requisitos.js")%>"></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><span class="icon-doc-small"></span>Añadir requisitos</h2>

    <asp:Label ID="msgError" runat="server" Text="" Visible="false" BackColor="Red" />
    <asp:Label ID="msgEstado" runat="server" Text="" Visible="false" />

    <asp:FormView runat="server" ID="FormView1"
        DataKeyNames="Id"
        ItemType="Prestlan.Models.Requisitos" 
        DefaultMode="Insert" 
        SelectMethod="FormView1_GetItem"
        UpdateMethod="FormView1_UpdateItem"
        InsertMethod="FormView1_InsertItem"
        OnInit="FormView1_Init"
        OnItemCommand="FormView1_ItemCommand"
        RenderOuterTable="False" Visible="true">

        <EditItemTemplate>
            <asp:ValidationSummary runat="server" />            

            <fieldset>               
                <div>
                    <label>Descripción:</label>
                    <asp:TextBox ID="tbDescripcion" runat="server" Text='<%# Bind("Descripcion") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbDescripcion"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage="Debe introducir un valor"
                        Text="*"
                        runat="server" />
                </div>

                <div>
                    <label>Tipos de documentos requeridos:</label>
                    <asp:ListBox ID="lbTipoDocumentoRequisito" runat="server" SelectionMode="Multiple" 
                        SelectMethod="GetTipoDocumento"
                        DataTextField="descripcion"
                        DataValueField="TipoDocumento_Id"></asp:ListBox>

                </div>
               
                <div>
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" CssClass="button"><span class="icon-guardar"></span>Guardar</asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="button"><span class="icon-cancelar"></span>Cancelar</asp:LinkButton>
                </div>

            </fieldset>
        </EditItemTemplate>



        <InsertItemTemplate>
            <asp:ValidationSummary runat="server" />
            <fieldset>

                 <div>
                    <label>Descripción:</label>
                    <asp:TextBox ID="tbDescripcion" runat="server" Text='<%# Bind("Descripcion") %>' />
                    <asp:RequiredFieldValidator ControlToValidate="tbDescripcion"
                        InitialValue=""
                        EnableClientScript="true"
                        SetFocusOnError="true"
                        ErrorMessage="Debe introducir un valor"
                        Text="*"
                        runat="server" />
                </div>

                <div>
                    <label>Tipos de documentos requeridos:</label>
                    <asp:ListBox ID="lbTipoDocumentoRequisito" runat="server" SelectionMode="Multiple" 
                        SelectMethod="GetTipoDocumento"
                        DataTextField="descripcion"
                        DataValueField="TipoDocumento_Id"></asp:ListBox>

                </div>


                <div>
                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" CssClass="button"><span class="icon-guardar"></span>Guardar</asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="button"><span class="icon-cancelar"></span>Cancelar</asp:LinkButton>
                </div>
            </fieldset>
        </InsertItemTemplate>

    </asp:FormView>

</asp:Content>
