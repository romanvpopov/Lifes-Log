using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.Windows.ApplicationModel.Resources;
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
            cmd.CommandText = $"Select description From ll_event Where id = {code}";
            var rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                var ss = rd.GetString(0);
                var rl = new ResourceLoader();
                BTM.IsChecked = ss.Contains(rl.GetString("BTMorning/Content"));
                BTD.IsChecked = ss.Contains(rl.GetString("BTDay/Content"));
                BTE.IsChecked = ss.Contains(rl.GetString("BTEvening/Content"));
                BNB.IsChecked = ss.Contains(rl.GetString("BTNewBlade/Content"));
            }
            rd.Close();
        }
        public override void InsertBody(NpgsqlCommand cmd, int code)
        {
            cmd.CommandText = $"Update ll_event Set description='{ToString()}' Where id={code}";
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

