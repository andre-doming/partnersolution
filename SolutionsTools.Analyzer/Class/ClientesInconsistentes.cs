using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionsTools.Analyzer
{
    [Table("TB_Clientes_Inconsistentes")]
    class ClientesInconsistentes
    {
        private int id;
        private string nome;
        private string sobrenome;
        private string cpf;
        private string email;
        private string sexo;
        private DateTime? datanascimento;
        private string departamento;
        private string cargo;
        private string obs;
        private string snpartner;
        private string tipo_busca_partner;
        private int? id_partner;
        private string guid_cliente_partner;
        private string guid_parceiro_partner;
        private string nome_partner;
        private string sobrenome_partner;
        private string snmasterdata;
        private string tipo_busca_masterdata;
        private string guid_cliente_masterdata;
        private string guid_parceiro_masterdata;
        private string nome_masterdata;
        private string sobrenome_masterdata;

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Sobrenome { get => sobrenome; set => sobrenome = value; }
        public string Cpf { get => cpf; set => cpf = value; }
        public string Email { get => email; set => email = value; }
        public string Sexo { get => sexo; set => sexo = value; }
        public DateTime? DataNascimento { get => datanascimento; set => datanascimento = value; }
        public string Departamento { get => departamento; set => departamento = value; }
        public string Cargo { get => cargo; set => cargo = value; }
        public string Obs { get => obs; set => obs = value; }
        public string SnPartner { get => snpartner; set => snpartner = value; }
        public string Tipo_Busca_Partner { get => tipo_busca_partner; set => tipo_busca_partner = value; }
        public int? Id_Partner { get => id_partner; set => id_partner = value; }
        public string Guid_Cliente_Partner { get => guid_cliente_partner; set => guid_cliente_partner = value; }
        public string Guid_Parceiro_Partner { get => guid_parceiro_partner; set => guid_parceiro_partner = value; }
        public string Nome_Partner { get => nome_partner; set => nome_partner = value; }
        public string Sobrenome_Partner { get => sobrenome_partner; set => sobrenome_partner = value; }
        public string SnMasterdata { get => snmasterdata; set => snmasterdata = value; }
        public string Tipo_Busca_Masterdata { get => tipo_busca_masterdata; set => tipo_busca_masterdata = value; }
        public string Guid_Cliente_Masterdata { get => guid_cliente_masterdata; set => guid_cliente_masterdata = value; }
        public string Guid_Parceiro_Masterdata { get => guid_parceiro_masterdata; set => guid_parceiro_masterdata = value; }
        public string Nome_Masterdata { get => nome_masterdata; set => nome_masterdata = value; }
        public string Sobrenome_Masterdata { get => sobrenome_masterdata; set => sobrenome_masterdata = value; }
    }
}
