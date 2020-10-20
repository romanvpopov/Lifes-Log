using System;
using Microsoft.Data.SqlClient;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;



namespace LL.LLEvents
{
    public sealed partial class EventFilter : UserControl
    {
        private readonly string lang = (App.Current as App).lang;
        private readonly Int16 usr = (App.Current as App).User;
        private readonly LLEvent le;

        public EventFilter(LLEvent lle)
        {
            le = lle;
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $"Select lt.Code,lt.{lang}_Name,lt.ClassName " +
                "From LLUserEventType lu join LLEventType lt on lu.EventTypeCode=lt.Code " +
                "Where lt.ClassName<>'' " +
                $"Order by lu.Turn,lt.{lang}_Name";
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var ni = new ListViewItem { Tag = reader.GetInt16(0), Content = reader.GetString(1) };
                    EventTypeList.Items.Add(ni);
                }
            }

        }
        private void Year_Click(object sender, Windows.UI.Xaml.RoutedEventArgs _)
        {
            le.prds = $"DatePart(year,c.Dat)= '{(sender as MenuFlyoutItem).Text}'";
            Period.Content = (sender as MenuFlyoutItem).Text;
            //EventList.ItemsSource = GetListEvents();
        }

        private void Alldays_Click(object sender, Windows.UI.Xaml.RoutedEventArgs _)
        {
            le.prds = "c.EventCode>0";
            Period.Content = (sender as MenuFlyoutItem).Text;
            //EventList.ItemsSource = GetListEvents();
        }

        private void Period_Click(object sender, Windows.UI.Xaml.RoutedEventArgs _)
        {
            var mf = new MenuFlyout();
            var fi = new MenuFlyoutItem() { Text = ResourceLoader.GetForCurrentView().GetString("Last365") };
            fi.Click += Last365_Click;
            mf.Items.Add(fi);
            fi = new MenuFlyoutItem() { Text = ResourceLoader.GetForCurrentView().GetString("Alldays") };
            fi.Click += Alldays_Click;
            mf.Items.Add(fi);
            mf.Items.Add(new MenuFlyoutSeparator());
            mf.Items.Add(new ToggleMenuFlyoutItem() { Text = ResourceLoader.GetForCurrentView().GetString("WCalendar"), IsChecked = true });
            using (var sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $"Select Distinct Convert(varchar,Datepart(year,DateEvent)) as DY From LLEvent Where UserCode = {usr} Order by DY ";
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    fi = new MenuFlyoutItem() { Text = rd.GetString(0) };
                    fi.Click += Year_Click;
                    mf.Items.Add(fi);
                }
            }
            Period.Flyout = mf;
        }

        private void Last365_Click(object sender, Windows.UI.Xaml.RoutedEventArgs _)
        {
            le.prds = "DateDiff(day, c.Dat, GETDATE()) between 0 and 365 ";
            Period.Content = (sender as MenuFlyoutItem).Text;
            //EventList.ItemsSource = GetListEvents();
        }

        private void Refresh_Click(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
        {
            le.etps = "0";
            foreach (ListViewItem s in EventTypeList.Items) { if (s.IsSelected) { le.etps = le.etps + "," + s.Tag.ToString(); }; }
            if (le.etps == "0") { le.etps = ""; } else { le.etps = " and l.EventTypeCode in (" + le.etps + ")"; };
            //EventList.ItemsSource = GetListEvents();
            //EventList.SelectedItem = EventList.Items.Last();
            //EventList.ScrollIntoView(EventList.SelectedItem);
            BTReset.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }
        private void Reset_Click(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
        {
            Period.Content = ResourceLoader.GetForCurrentView().GetString("Last365");
            le.prds = "DateDiff(day, c.Dat, GETDATE()) between 0 and 365 ";
            le.etps = ""; le.srchs = "";
            //EventFilter.ItemsSource = GetFilterEvents();
            //EventList.ItemsSource = GetListEvents();
            BTReset.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Search_Submitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var ss = sender.Text;
            if (ss != "")
            {
                le.srchs = $" and (l.Comment like '%{ss}%' or l.Descr like '%{ss}%')";
                // EventList.ItemsSource = GetListEvents();
                BTReset.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }
    }
}
