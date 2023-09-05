using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionsTools.Analyzer
{
    class ClientesInconsistentesDAL : DBConnAnalyzer
    {
        public long Insert(ClientesInconsistentes cliente)
        {
            using (var conn = Connection())
            {
                return conn.Insert<ClientesInconsistentes>(cliente);
            }
        }
        public void Update(ClientesInconsistentes cliente)
        {
            using (var conn = Connection())
            {
                conn.Update<ClientesInconsistentes>(cliente);
            }
        }
        public void Delete(ClientesInconsistentes cliente)
        {
            using (var conn = Connection())
            {
                conn.Delete<ClientesInconsistentes>(cliente);
            }
        }
        public ClientesInconsistentes GetById(int id)
        {
            using (var conn = Connection())
            {
                var clientes = conn.Query<ClientesInconsistentes>(string.Format("select * from TB_Clientes_Inconsistentes where Id = '{0}'", id));

                if (clientes.Count() > 0)
                {
                    return clientes.First();
                }

                return null;
            }
        }
        public List<ClientesInconsistentes> GetAll()
        {
            var clientes = new List<ClientesInconsistentes>();

            using (var conn = Connection())
            {
                clientes = conn.Query<ClientesInconsistentes>(string.Format("select * from TB_Clientes_Inconsistentes")).ToList();

                return clientes;
            }
        }
    }
}
