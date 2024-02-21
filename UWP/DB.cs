using System.Data.SqlClient;

namespace LL
{
    class Db
    {
        public static void CreateDb()
        {
            using (var sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = "SELECT database_id FROM sys.databases Where Name='LL'";
                var rd = cmd.ExecuteReader();
                if (rd.Read()) return;
                cmd.CommandText = "Create database 'LL'";
                try { cmd.ExecuteNonQuery(); }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
