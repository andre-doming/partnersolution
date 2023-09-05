using SolutionsTools.DAL;
using SolutionsTools.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace SolutionsTools.UI
{
    public partial class ImportarNew : System.Web.UI.Page
    {
        [WebMethod]
        public static string ProcessData(string arquivo, string empresa, string empresaId, string idusuario)
        {
            var file = System.Web.HttpContext.Current.Request.MapPath(string.Format("~\\uploads\\{0}", arquivo));

            var reader = new System.IO.StreamReader(file, Encoding.GetEncoding("iso-8859-1"));

            var content = reader.ReadToEnd();

            var log = new List<String>();

            int i = 0;

            var firstRow = true;

            foreach (string row in content.Split('\n'))
            {
                if (string.IsNullOrEmpty(row))
                {
                    i++;
                    continue;
                }

                if (firstRow)
                {
                    firstRow = false;
                    i++;
                    continue;
                }
                else
                {
                    var columns = row.Split(';');

                    var cliente = CreateClient(columns);

                    cliente.Empresa = empresa;
                    cliente.EmpresaId = Guid.Parse(empresaId);
                    cliente.SeqId = Int32.Parse(idusuario);

                    if (string.IsNullOrEmpty(cliente.Cpf) && string.IsNullOrEmpty(cliente.Email) && string.IsNullOrEmpty(cliente.Registro_Colaborador))
                    {
                        var msg = "O CPF, Email ou Registro Colaborador não foram informados.";
                        PutRowLog(i, cliente.Nome, cliente.Sobrenome, msg, ref log, "E");
                        i++;
                        continue;
                    }

                    var isValid = ValidateClient(cliente, i, ref log);

                    if (!isValid)
                    {
                        i++;
                        continue;
                    }

                    ProcessClient(cliente, i, ref log);
                }
                i++;
            }

            var logFile = CreateLogFile(log);

            return logFile;
        }
        private static void ProcessClient(Cliente csv, int line, ref List<String> log)
        {
            var md = new CryptographyMD5();

            string cpf_hash = null;

            if (csv.Cpf.Length > 0)
                cpf_hash = md.ReturnMD5(csv.Cpf);

            var dal = new DAL.ClienteDAL();

            Cliente cliRet = null;

            if (csv.Cpf.Length > 0)
                cliRet = dal.GetByCpf(cpf_hash);

            if (cliRet == null && csv.Email.Length > 0)
                cliRet = dal.GetByEmail(csv.Email);

            if (cliRet == null && csv.Registro_Colaborador.Length > 0)
                cliRet = dal.GetByRegistroColaborador(csv.Registro_Colaborador);

            try
            {
                if (cliRet == null)
                {
                    if (csv.Acao.Equals("inserir"))
                    {
                        InsertClient(csv, cpf_hash);
                    }
                    else
                    {
                        var msg = string.Format("Não foi possível {0}, pois o cliente não existe na base.", csv.Acao);
                        PutRowLog(line, csv.Nome, csv.Sobrenome, msg, ref log, "E");
                    }
                }
                else
                {
                    if (csv.Acao.Equals("atualizar"))
                    {
                        UpdateClient(csv, cliRet);
                    }
                    else if (csv.Acao.Equals("excluir"))
                    {
                        UpdateClientCompany(csv, cliRet);
                    }
                    else
                    {
                        var msg = "Não foi possível inserir, pois o cliente já existe na base.";
                        PutRowLog(line, csv.Nome, csv.Sobrenome, msg, ref log, "E");
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "Error: " + ex.Message;
                PutRowLog(line, csv.Nome, csv.Sobrenome, msg, ref log, "E");
            }
            
        }
        private static Cliente CreateClient(string[] columns)
        {
            var cliente = new DAL.Cliente();
            cliente.Nome = columns[0];
            cliente.Sobrenome = columns[1];
            cliente.Cpf = columns[2];
            cliente.Email = string.IsNullOrEmpty(columns[3]) ? string.Empty : columns[3].Replace(" ", "");
            cliente.Sexo = columns[4];
            cliente.Dt_Nascimento = columns[5];
            cliente.Departamento = columns[6];
            cliente.Cargo = columns[7];
            cliente.Registro_Colaborador = columns[8];
            cliente.Acao = columns[9];
            cliente.Aprovado = false;

            return cliente;
        }
        private static bool ValidateClient(Cliente csv, int line, ref List<String> log)
        {
            var isValid = true;

            if (csv.Sobrenome.Length == 0)
            {
                var nomes = csv.Nome.Split(' ');

                if (nomes.Length > 1)
                {
                    csv.Sobrenome = csv.Nome.Replace(nomes[0], "").Trim();
                    csv.Nome = nomes[0];
                }
            }

            if (!Utils.IsCpf(csv.Cpf) && csv.Cpf.Length > 0)
            {
                var msg = string.Format("O CPF {0} é inválido!", csv.Cpf);
                PutRowLog(line, csv.Nome, csv.Sobrenome, msg, ref log, "E");
                isValid = false;
            }
            else
            {
                csv.Cpf = Utils.FormatCPF(csv.Cpf);
            }

            if (!Utils.IsEmail(csv.Email) && csv.Email.Length > 0)
            {
                var msg = string.Format("O Email {0} é inválido!", csv.Email, "E");
                PutRowLog(line, csv.Nome, csv.Sobrenome, msg, ref log);
                isValid = false;
            }
            else
            {
                csv.Email = csv.Email.Replace(" ", "");
            }

            if (csv.Acao.Length > 0)
                csv.Acao = Regex.Replace(csv.Acao.ToLower(), "[^0-9a-zA-Z_]+", "");

            return isValid;
        }
        private static void InsertClient(Cliente csv, string cpf_hash)
        {
            var movto = "I";

            var cs = new ClientServices();

            var ret = cs.AddClient(csv);

            if (ret.DocumentId == new Guid())
            {
                throw new Exception(ret.Message);
            }

            var cli = new ClienteDAL();
            csv.Id_Cliente = ret.DocumentId;
            csv.Id_Parceiro = csv.EmpresaId;
            csv.Cpf = cpf_hash;
            csv.Ativo = "S";
            cli.Insert(csv);

            Utils.Log(EntityType.CN, ret.DocumentId, csv.SeqId, movto);
        }
        private static void UpdateClient(Cliente csv, Cliente cliRet)
        {
            var movto = "U";

            var cs = new ClientServices();

            var client = cs.GetById(cliRet.Id_Cliente);
            client.Cargo = csv.Cargo;
            client.Departamento = csv.Departamento;

            cs.UpdClient(client);

            Utils.Log(EntityType.CN, cliRet.Id_Cliente, csv.SeqId, movto);
        }
        private static void UpdateClientCompany(Cliente csv, Cliente cliRet)
        {
            var movto = "U";

            var cs = new ClientServices();

            var cliente = cs.GetById(cliRet.Id_Cliente);

            var dal = new EmpresaDAL();
            var empresa = dal.GetByCnpj("00000000000000");

            cliente.EmpresaAnterior = cliente.Empresa;
            cliente.Empresa = empresa.Nome_Fantasia;
            cliente.EmpresaId = empresa.Id_Parceiro;
            var ret = cs.UpdClient(cliente);

            var cli = new ClienteDAL();
            cliente = cli.GetByIdCliente(cliRet.Id_Cliente);
            cliente.Id_Cliente = ret.DocumentId;
            cliente.Id_Parceiro = empresa.Id_Parceiro;
            cli.Update(cliente);

            Utils.Log(EntityType.CN, cliRet.Id_Cliente, csv.SeqId, movto);
        }
        private static void PutRowLog(int line, string name, string sobrenome, string msg, ref List<String> log, string tipo = "M")
        {
            if (log.Count == 0)
            {
                log = new List<String>();
                log.Add("linha;nome;sobrenome;erro;data|tipo");
            }

            var erro = string.Format("{0};{1};{2};{3};{4}|{5}", line, name, sobrenome, msg, DateTime.Now.ToString(), tipo);

            log.Add(erro);
        }
        private static string CreateLogFile(List<String> log)
        {
            if (log.Count == 0)
                return string.Empty;

            var fileName = DateTime.Now.Ticks.ToString() + ".csv";

            var filePath = HttpContext.Current.Server.MapPath(string.Format("~\\logs\\{0}", fileName));

            var sw = new StreamWriter(filePath);

            foreach (var item in log)
            {
                var infos = item.Split('|');

                sw.WriteLine(infos[0]);
            }

            sw.Close();

            return fileName;
        }
        private static string ConvertGender(string gender)
        {
            var convertGender = string.IsNullOrEmpty(gender) ? string.Empty : gender;

            convertGender = convertGender.Length == 0 ? string.Empty : convertGender.Substring(0, 1).ToUpper();

            switch (convertGender)
            {
                case "M":
                    return "male";
                case "F":
                    return "female";
                default:
                    return "not-to-say";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Utils.LoadCompanies(mselEmpresas, false, true);
            }
        }
        protected void btnProcessar_Click(object sender, EventArgs e)
        {
            
        }
    }
}