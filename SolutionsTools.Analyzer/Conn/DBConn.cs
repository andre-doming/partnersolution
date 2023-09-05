using System.Configuration;
using System.Data.SqlClient;

namespace SolutionsTools.Analyzer
{
    public class DBConnAnalyzer
    {
        private string _conn = ConfigurationManager.ConnectionStrings["DB_Conn_Analyzer"].ConnectionString;

        public SqlConnection Connection() {
            var conn = new SqlConnection(_conn);
            return conn;
        }

    }
}
