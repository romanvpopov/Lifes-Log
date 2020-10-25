using System;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Linq;
using Windows.UI.Xaml.Controls;
using LL.LLEvents;
using Windows.ApplicationModel.Resources;

namespace LL { 
    /// 
    /// Страница событий
    /// 
    public sealed partial class LLEvent : Page {

        private readonly string lang = (App.Current as App).lang;
        private readonly Int16 usr = (App.Current as App).User;
        public string etps, prds, srchs;

        public LLEvent() {
            InitializeComponent();
        }

        private void EventList_Loaded(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
        {
            prds = "DateDiff(day, c.Dat, GETDATE()) between 0 and 365";
            etps = ""; srchs = "";
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                for (int i = -365; i <= 0; i++)
                    EL.Items.Add(new Day(sq, DateTime.Today.Add(TimeSpan.FromDays(i))));
            }
            EL.SelectedItem = EL.Items.Last();
            EL.ScrollIntoView(EL.SelectedItem);

            RPane.Content = new NewEventList(EL,this);
        }

        public void HidePane()
        {
            if (!FixPane.IsOn) RightPane.IsPaneOpen = false;
        }

        private void BTMove_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var mf = new MenuFlyout();
            var fi = new MenuFlyoutItem() { Text = ResourceLoader.GetForCurrentView().GetString("Last365") };
            //; fi.Click += Last365_Click;
            mf.Items.Add(fi);
            mf.Items.Add(new MenuFlyoutSeparator());
            mf.Items.Add(new ToggleMenuFlyoutItem() { Text = ResourceLoader.GetForCurrentView().GetString("WCalendar"), IsChecked = true });
            using (var sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $"Select Distinct Convert(varchar,Datepart(year,DateEvent)) as DY From LLEvent Order by DY ";
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    fi = new MenuFlyoutItem() { Text = rd.GetString(0) };
                    //                    fi.Click += Year_Click;
                    mf.Items.Add(fi);
                }
            }
            BTMove.Flyout = mf;

        }

        private void BTNewEvent_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (RPane.Content.GetType().Name == "NewEventList")
            {
                RightPane.IsPaneOpen = !RightPane.IsPaneOpen;
            }
            else
            {
                RPane.Content = new NewEventList(EL, this);
                RightPane.IsPaneOpen = true;
            }
        }

        private void BTSearch_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (RPane.Content.GetType().Name == "EventFilter")
            {
                RightPane.IsPaneOpen = !RightPane.IsPaneOpen;
            }
            else
            {
                RPane.Content = new EventFilter(this);
            }
        }
    }
}



