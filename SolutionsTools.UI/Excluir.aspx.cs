using SolutionsTools.DAL;
using SolutionsTools.Services;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SolutionsTools.UI
{
    public partial class Excluir : BaseWebUi
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Utils.LoadCompanies(mselEmpresas, false);
            }
        }

        protected void btnListar_Click(object sender, EventArgs e)
        {
            var dal = new ClienteDAL();

            var dalEmpresa = new EmpresaDAL();

            var empresa = dalEmpresa.GetById(Convert.ToInt32(mselEmpresas.Value));

            var id = empresa.Id_Parceiro;

            var procurarPor = txtProcurarPor.Text;

            var campo = "Nome_Completo";

            var clientes = dal.GetByCompanyAndCustom(id, campo, procurarPor);

            grvClientes.DataSource = clientes;
            grvClientes.DataBind();
        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grvClientes.Rows)
            {
                var chk = (HtmlInputCheckBox)row.FindControl("chkSelecionado");

                var id = Guid.Parse(grvClientes.DataKeys[row.RowIndex].Value.ToString());

                if (chk.Checked)
                {
                    DeleteClient(id);
                }
            }

            Tools.ShowMessage(this.Page, Page.GetType(),"Cliente(s) excluído(s)!", Tools.MessageType.Info);
        }
        public void DeleteClient(Guid id)
        {
            var cs = new ClientServices();
            cs.DelClient(id);

            var cli = new ClienteDAL();
            var cliente = cli.GetByIdCliente(id);
            cliente.Ativo = "N";
            cli.Update(cliente);

            Utils.Log(EntityType.CL, id, "D");
        }

        protected void grvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Style.Add("display","none");
        }
    }
}