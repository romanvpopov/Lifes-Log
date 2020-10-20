using System;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Linq;
using Windows.UI.Xaml.Controls;
using LL.LLEvents;

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

            RPane.Content = new NewEventList(EL);
        }

        private void BTNewEvent_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
           RPane.Content = new NewEventList(EL); 
        }

        private void BTSearch_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //RPane.Content = new LLEvents.EventFilter(this);
        }
    }
}



