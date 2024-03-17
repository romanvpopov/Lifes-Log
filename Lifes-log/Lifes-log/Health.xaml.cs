using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Lifes_log
{
    public sealed partial class Health : Page
    {
        public Health()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var cmd = App.NpDs.CreateCommand($@"
                   Select Distinct lt.id,lt.priority,lt.{App.lang}_name From ll_event_type lt
                   Where lt.priority > 0 and lt.HSM = 'H' Order by priority");
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    EL.Items.Add(new Healths.HQEvent(rd.GetInt16(0), rd.GetString(2)));
                }
                rd.Close();

        }
    }
}
