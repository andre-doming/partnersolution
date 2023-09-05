using SolutionsTools.DAL;
using SolutionsTools.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolutionsTools.Analyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //var cliente = new ClientesInconsistentes();

            var cliPartner = new ClienteDAL();

            var cliMasterdata = new ClientServices();

            var dal = new ClientesInconsistentesDAL();

            var clientes = dal.GetAll();

            foreach (var cliente in clientes)
            {
                // Processo de busca no Partner (SQL)

                var c = cliPartner.GetByEmail(cliente.Email);
                cliente.Tipo_Busca_Partner = "Email";

                if (c == null)
                {
                    var md = new CryptographyMD5();
                    var hash = md.ReturnMD5(cliente.Cpf);
                    c = cliPartner.GetByCpf(hash);
                    cliente.Tipo_Busca_Partner = "Cpf";
                }

                if (c != null)
                {
                    cliente.SnPartner = "S";
                    cliente.Id_Partner = c.Id;
                    cliente.Guid_Cliente_Partner = c.Id_Cliente.ToString();
                    cliente.Guid_Parceiro_Partner = c.Id_Parceiro.ToString();
                    cliente.Nome_Partner = c.Nome;
                    cliente.Sobrenome_Partner = c.Sobrenome;
                }
                else
                {
                    cliente.SnPartner = "N";
                    cliente.Id_Partner = null;
                    cliente.Guid_Cliente_Partner = string.Empty;
                    cliente.Guid_Parceiro_Partner = string.Empty;
                    cliente.Nome_Partner = string.Empty;
                    cliente.Sobrenome_Partner = string.Empty;
                }

                dal.Update(cliente);

                c = null;

                // Processo de busca no Masterdata (VTEX)

                c = cliMasterdata.GetByEmail(cliente.Email);
                cliente.Tipo_Busca_Masterdata = "Email";

                if (c == null)
                {
                    c = cliMasterdata.GetByCpf(cliente.Cpf);
                    cliente.Tipo_Busca_Masterdata = "Cpf";
                }

                if (c != null)
                {
                    cliente.SnMasterdata = "S";
                    cliente.Guid_Cliente_Masterdata = c.Id_Cliente.ToString();
                    cliente.Guid_Parceiro_Masterdata = c.EmpresaId.ToString();
                    cliente.Nome_Masterdata = c.Nome;
                    cliente.Sobrenome_Masterdata = c.Sobrenome;
                }
                else
                {
                    cliente.SnPartner = "N";
                    cliente.Guid_Cliente_Masterdata = string.Empty;
                    cliente.Guid_Parceiro_Masterdata = string.Empty;
                    cliente.Nome_Masterdata = string.Empty;
                    cliente.Sobrenome_Masterdata = string.Empty;
                }

                dal.Update(cliente);

                c = null;
            }

            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var s = new ClientServices();

            Cursor.Current = Cursors.WaitCursor;

            var clientes = new List<Cliente>();

            var isContinue = true;

            //var isReady = true;

            while (isContinue)
            {
                clientes = s.GetByCompanyTabocas();

                while (clientes.Count == 0)
                {
                    clientes = s.GetByCompanyTabocas();
                }

                isContinue = !clientes.Count.Equals(0);

                foreach (var cliente in clientes)
                {
                    if (cliente.Empresa.Equals("Tabocas"))
                    {
                        s.DelClient(cliente.Id_Cliente);
                    }
                }

                //System.Threading.Thread.Sleep(10000);
            }

            Cursor.Current = Cursors.Default;

        }
    }
}
