using Microsoft.Data.SqlClient;
using System.Text;

namespace LL
{
    class DB
    {
        public DB() { }

        public static void CreateDB()
        {
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = "SELECT database_id FROM sys.databases Where Name='LL'";
                var rd = cmd.ExecuteReader();
                if (!rd.Read()) {
                    cmd.CommandText = "Create database 'LL'";
                    try { cmd.ExecuteNonQuery(); }
                    catch { }
                };
            }
        }
    }
}
