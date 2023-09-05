<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableViewStateMac="true" AutoEventWireup="true" CodeBehind="Excluir.aspx.cs" Inherits="SolutionsTools.UI.Excluir" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <script src="Import/utils.js" type="text/javascript"></script>
    <script src="Import/consulting.js" type="text/javascript"></script>

    <h1>Excluir Clientes</h1>

    <div class="form-group">
        <div class="input-group">
            <select class="custom-select" id="mselEmpresas" runat="server"> </select>
            <div class="input-group-append">
                <asp:Button ID="btnListar" runat="server" Text="Listar" OnClick="btnListar_Click" class="btn btn-outline-secondary" Visible="false" />
            </div>
        </div>
    </div>

    <div class="form-group">
        <div class="input-group">
            <select class="custom-select" id="mselProcurarPor" runat="server" >
                    <option value="Nome_Completo">Nome</option>
            </select>
            <asp:TextBox ID="txtProcurarPor" runat="server" Width="80%" class="custom-input"></asp:TextBox>
            <div class="input-group-append">
                <asp:Button ID="btnProcurarPor" runat="server" Text="Listar" OnClick="btnListar_Click" class="btn btn-outline-secondary" />
            </div>
        </div>
    </div>

   <div class="form-group">
       <asp:Button ID="btnExcluir" runat="server" class="btn btn-primary" Text="Excluir" OnClick="btnExcluir_Click" OnClientClick="return confirm('Confirmar a exclusão?');" />
   </div>

   <div class="form-row">
        <div class="col-md-12 mb-3" style="min-height: 100px; max-height: 550px; overflow-x: hidden; overflow-y: auto">
            <asp:gridview id="grvClientes" runat="server" cssclass="table table-hover table-striped table-bordered table-condensed table-sm clientes-table" 
                headerstyle-cssclass="thead-light" Style="width: 99.8%; height: 99%;" gridlines="None" AutoGenerateColumns="false" DataKeyNames="Id_Cliente" OnRowDataBound="grvClientes_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Id_Cliente" HeaderText="Guid"></asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div style="text-align: center">
                            <input id="chkSelecionarTodos" class="chkSelecionarTodos" type="checkbox" checked />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: center">
                            <input id="chkSelecionado" runat="server" class="chkSelecionado" type="checkbox" checked />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id" HeaderText="Id"></asp:BoundField>
                    <asp:BoundField DataField="Nome" HeaderText="Name"></asp:BoundField>
                    <asp:BoundField DataField="Sobrenome" HeaderText="Sobrenome"></asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            CPF
                        </HeaderTemplate>
                        <ItemTemplate>
                            000.000.000-00
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
                    <asp:BoundField DataField="Ativo" HeaderText="Ativo"></asp:BoundField>
                </Columns>
            </asp:gridview>
        </div>
    </div>
    
    <button type="button" class="btn btn-primary" id="btnConsultar">Consultar informações</button>

    <div id="divProcessando" class="divProcessando" style="display: none"></div>

    <asp:HiddenField ID="Funcao" runat="server" Value="funcImpNew" />

</asp:Content>
