using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using Windows.UI.Xaml.Controls;
using LL.LLEvents;
using Windows.Storage;
using System.Threading.Tasks;

namespace LL { 
    /// 
    /// Страница событий
    /// 
    public sealed partial class LLEvent : Page {

        private readonly string lang = (App.Current as App).lang;
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        private readonly DayList ds;
        private readonly EventFilter ef;
        private readonly NewEventList ne;
        private readonly MoveTo mt;

        public LLEvent() {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            ds = new DayList();
            ef = new EventFilter { Apply = ApplyFilter, Reset = ResetFilter };
            mt = new MoveTo { Move = Move };
            ne = new NewEventList { Add = Add };
            if (ls.Values.ContainsKey("FixPane")) FixPane.IsOn = (bool)ls.Values["FixPane"];
            RPane.Content = ne;
        }

        private void EventList_Loaded(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
        {
            EL.ItemsSource=ds;
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
                if (!EL.Items.Where(n => (n as Day)?.dt == DateTime.Today).Select(n => n).Any()) { }
            } else {
                var d = new DateTime(Int32.Parse(ss), 12, 31);
                if (!EL.Items.Where(n => (n as Day)?.dt == d).Select(n => n).Any()) { }
            }
            HidePane();
        }

        private void ApplyFilter(String tpd)
        {
            ds.etps = tpd;
            foreach (object d in EL.Items) 
            if (d.GetType().Name == "Day") (d as Day).ApllyFilter(tpd);
        }

        private void ResetFilter()
        {
            ds.etps = "0";
            foreach (object d in EL.Items) if (d.GetType().Name == "Day") (d as Day).ResetFilter();
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
                        while (rd.Read()) EL.Items.Add(new Day(rd.GetDateTime(0), "0"));
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



