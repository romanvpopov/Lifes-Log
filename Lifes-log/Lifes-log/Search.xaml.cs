using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Lifes_log
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
            var cmd = (App.Current as App).NpDs.CreateCommand($@"
                   Select event_time From ll_event Where comment like '%{ss}%' or ll_event.description like '%{ss}%' Order by event_time");
                try
                {
                    var rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read()) EL.Items.Add(new LLEvents.Day(rd.GetDateTime(0), "0"));
                    }
                }
                catch
                {
                    // ignored
                }
            }
    }
}
