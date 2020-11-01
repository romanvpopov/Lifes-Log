using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using Windows.UI.Xaml.Controls;
using LL.LLEvents;
using Windows.Storage;
using Windows.UI.Xaml;

namespace LL { 
    /// 
    /// Страница событий
    /// 
    public sealed partial class LLEvent : Page {

        private readonly string lang = (App.Current as App).lang;
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        public string etps, prds, srchs;
        private EventFilter ef;
        private NewEventList ne;
        private MoveTo mt;

        public LLEvent() {
            InitializeComponent();
        }

        private void EventList_Loaded(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
        {
            etps = ""; srchs = "";
            EL.Items.Add(new More());
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))  {
                sq.Open();
                for (int i = -31; i <= 0; i++)
                    EL.Items.Add(new Day(sq, DateTime.Today.Add(TimeSpan.FromDays(i)),""));
            }
            EL.Items.Add(new More());
            ef = new EventFilter();  ef.Apply = ApplyFilter;
            mt = new MoveTo();       mt.Move = Move;
            ne = new NewEventList(); ne.Add=Add;
            RPane.Content = ne;
            if (ls.Values.ContainsKey("FixPane")) FixPane.IsOn = (bool) ls.Values["FixPane"];
            Move("Today");
        }

        private void Add(DateTime dt, Int16 tp)
        { 
            foreach (object dy in EL.Items) {
                if (dy.GetType().Name == "Day") {
                    if ((dy as Day).dt == dt) {
                        (dy as Day).AddEvent(tp);
                        EL.SelectedItem = dy;
                        EL.ScrollIntoView(EL.SelectedItem);
                        HidePane();
                        break;
                    }
}
            }
        }

        private void Move(String ss) 
        {
            if (ss == "Today") {
                foreach (object d in EL.Items) {
                    if (d.GetType().Name == "Day") {
                        if ((d as Day).dt == DateTime.Today) {
                            EL.SelectedItem = d;
                            EL.ScrollIntoView(EL.SelectedItem);
                            HidePane();
                            break;
                        }
                    }
                }
            }
        }

        private void ApplyFilter(String tpd)
        { 
        }

        public void HidePane()
        {
            if (!FixPane.IsOn) RightPane.IsPaneOpen = false;
        }

        private void FixPane_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ls.Values["FixPane"] = FixPane.IsOn;
        }

        private void BTMove_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (RPane.Content.GetType().Name == "MoveTo") {
                RightPane.IsPaneOpen = !RightPane.IsPaneOpen;
            } else {
                RPane.Content = mt;
                RightPane.IsPaneOpen = true;
            }
        }


        private void BTNewEvent_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (RPane.Content.GetType().Name == "NewEventList")  {
                RightPane.IsPaneOpen = !RightPane.IsPaneOpen;
            } else {
                RPane.Content = ne;
                RightPane.IsPaneOpen = true;
            }
        }

        private void BTFilter_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (RPane.Content.GetType().Name == "EventFilter") {
                RightPane.IsPaneOpen = !RightPane.IsPaneOpen;
            } else {
                RPane.Content = ef;
                RightPane.IsPaneOpen = true;
            }
        }
    }
}



