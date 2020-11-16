using LL.LLEvents;
using Microsoft.Data.SqlClient;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LL
{
    public sealed partial class Search : Page
    {
        public Search()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var ss = e.Parameter as string;
            if (ss == "") return;
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText =
                   $"Select DateEvent From llEvent Where Comment like '%{ss}%' or Descr like '%{ss}%' Order by DateEvent";
                var rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read()) EL.Items.Add(new Day(rd.GetDateTime(0), "0"));
                }
                else
                {
                    
                }
            }
        }
    }
}
