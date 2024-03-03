using Npgsql;

namespace Lifes_log.LLEvents
{
    public sealed partial class UShaving
    {
        public UShaving(NpgsqlCommand cmd, int code)
        {
            InitializeComponent();
            BTM.IsChecked = true;
            if (code <= 0) return;
            cmd.CommandText = $"Select Comment From LLEvent Where Code = {code}";
            var rd = cmd.ExecuteReader();
            if (!rd.Read()) return;
            var ss = rd.GetString(0);
            BTM.IsChecked = ss.Contains("Morning");
            BTD.IsChecked = ss.Contains("Midday");
            BTE.IsChecked = ss.Contains("Evening");
            BNB.IsChecked = ss.Contains("New blade");
        }
        public override void InsertBody(NpgsqlCommand cmd, int code)
        {
            cmd.CommandText = $"Update LLEvent Set Comment='{ToString()}' Where Code={code}";
            cmd.ExecuteNonQuery();
        }
        public override string ToString()
        {
            var st = BNB.IsChecked == true ? " " + BNB.Content : "";
            if (BTM.IsChecked == true) return BTM.Content + st;
            else if (BTD.IsChecked == true) return BTD.Content + st;
            else return BTE.Content + st;
        }
    }
}

