using System.Data.SqlClient;
using Windows.ApplicationModel.Resources;

namespace LL.LLEvents
{
    public sealed partial class UShaving
    {
        public UShaving(SqlCommand cmd, int code, short ntp)
        {
            InitializeComponent();
            BTM.IsChecked = true;
            if (code <= 0) return;
            cmd.CommandText = $"Select Comment From LLEvent Where Code = {code}";
            var rd = cmd.ExecuteReader();
            if (!rd.Read()) return;
            var ss = rd.GetString(0);
            BTM.IsChecked = ss.IndexOf(ResourceLoader.GetForCurrentView().GetString("BTMorning/Content")) >= 0;
            BTD.IsChecked = ss.IndexOf(ResourceLoader.GetForCurrentView().GetString("BTDay/Content")) >= 0;
            BTE.IsChecked = ss.IndexOf(ResourceLoader.GetForCurrentView().GetString("BTEvening/Content")) >= 0;
            BNB.IsChecked = ss.IndexOf(ResourceLoader.GetForCurrentView().GetString("BTNewBlade/Content")) >= 0;
        }
        public override void InsertBody(SqlCommand cmd, int code)
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

