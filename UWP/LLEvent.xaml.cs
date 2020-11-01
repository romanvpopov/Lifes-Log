using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using Windows.UI.Xaml.Controls;
using LL.LLEvents;
using Windows.Storage;

namespace LL { 
    /// 
    /// Страница событий
    /// 
    public sealed partial class LLEvent : Page {

        private readonly string lang = (App.Current as App).lang;
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        public string etps, prds;
        private EventFilter ef;
        private NewEventList ne;
        private MoveTo mt;
        private int lp;

        public LLEvent() {
            InitializeComponent();
        }

        private void EventList_Loaded(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
        {
            (App.Current as App).lenpage = 31;
            lp = (App.Current as App).lenpage;
            etps = "0"; 
            var dt = DateTime.Today.Add(TimeSpan.FromDays(lp));
            var mr = new More(dt) { Up = More };
            EL.Items.Add(mr);
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))  {
                sq.Open();
                for (int i = -lp; i <= 0; i++) {
                    dt = DateTime.Today.Add(TimeSpan.FromDays(i));
                    EL.Items.Add(new Day(dt, etps));
                }
            }
            ef = new EventFilter { Apply = ApplyFilter, Reset=ResetFilter };
            mt = new MoveTo { Move = Move };
            ne = new NewEventList { Add = Add };
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
            etps = tpd;
            foreach (object d in EL.Items) if (d.GetType().Name == "Day") (d as Day).ApllyFilter(tpd);
        }

        private void ResetFilter()
        {
            etps = "0";
            foreach (object d in EL.Items) if (d.GetType().Name == "Day") (d as Day).ResetFilter();
        }

        private void More(DateTime dt)
        {
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                for (int i = -lp; i <= 0; i++) {
                    dt = dt.Add(TimeSpan.FromDays(-1));
                    EL.Items.Insert(1,new Day(dt, etps));
                }
            }
            EL.SelectedItem = EL.Items.ElementAt(lp);
            EL.ScrollIntoView(EL.SelectedItem);
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

        private void TS_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
                {
                    sq.Open();
                    var ss = (sender as TextBox).Text;
                    var cmd = sq.CreateCommand();
                    cmd.CommandText =
                       $"Select DateEvent From llEvent Where Comment like '%{ss}%' or Descr like '%{ss}%' Order by DateEvent";
                    var rd = cmd.ExecuteReader();
                    if (rd.HasRows) {
                        EL.Items.Clear();
                        while (rd.Read()) EL.Items.Add(new Day(rd.GetDateTime(0), etps));
                        EL.SelectedItem = EL.Items.Last();
                        EL.ScrollIntoView(EL.SelectedItem);
                    }
                    else {
                        new Msg("NF").ShowAt(sender as TextBox);
                    }
                }
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



