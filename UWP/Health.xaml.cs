using System.Data.SqlClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LL
{
    public sealed partial class Health : Page
    {
        private readonly string lang = (App.Current as App).lang;

        public Health()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dsp = Window.Current.Dispatcher;
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr)) {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText =$@"
                   Select Distinct lt.Code,lt.Turn,lt.{lang}_name From LLEventType lt
                    join LLFieldEvent le on lt.Code = le.EventTypeCode
                    join LLUnit lu on le.UnitCode = lu.Code
                    Where lt.Turn > 0 and lt.HSM = 'H' Order by Turn";
                var rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    EL.Items.Add(new LL.Healths.HQEvent(rd.GetInt16(0), rd.GetString(2)));
                }
                rd.Close();

            }
        }
    }
}
