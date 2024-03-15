using System;
using Lifes_log.LLEvents;

namespace Lifes_log
{
    public sealed partial class LlEvent
    {

        private readonly DayL ds;
        private readonly EventFilter ef;
        private readonly NewEventList ne;
        private readonly MoveTo mt;

        public LlEvent()
        {
            InitializeComponent();
            NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            ds = new (new DayList(DateTime.Today));
            ef = new EventFilter { Apply = ApplyFilter, Reset = ResetFilter };
            mt = new MoveTo { Move = Move };
            ne = new NewEventList
            {
                Add = Add,
                Manage = () => { Frame.Navigate(typeof(DBSettings.SetEventType)); }
            };
            //if (ls.Values.TryGetValue("FixPane", out var value))
            FixPane.IsOn = true; // (bool)value;
            RPane.Content = ne;
        }

        private void EL_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            El.ItemsSource = ds;
        }

        private void Add(DateTime dt, Int16 tp)
        {
            foreach (var dy in El.Items)
            {
                if (dy.GetType().Name != "Day") continue;
                if (((Day)dy).dt != dt) continue;
                (dy as Day).AddEvent(tp);
                El.SelectedItem = dy;
                El.ScrollIntoView(El.SelectedItem);
                HidePane();
                break;
            }
        }

        private void Move(string ss)
        {
            ds.RefreshAsync();
            if (ss == "Today")
            {
                ds.Dt = DateTime.Today;
                ne.DatePic = DateTime.Today;
            }
            else
                ds.Dt = new DateTime(Int32.Parse(ss), 12, 31);
            HidePane();
        }

        private void ApplyFilter(String tpd)
        {
            ds.Etps = tpd;
            foreach (object d in El.Items)
                if (d.GetType().Name == "Day") (d as Day).ApllyFilter(tpd);
        }

        private void ResetFilter()
        {
            ds.Etps = "0";
            foreach (var d in El.Items) if (d.GetType().Name == "Day") (d as Day)?.ResetFilter();
        }

        private void HidePane()
        {
            if (!FixPane.IsOn) RightPane.IsPaneOpen = false;
        }

        private void FixPane_Toggled(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            //ls.Values["FixPane"] = FixPane.IsOn;
        }

        private void BTMove_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (RPane.Content.GetType().Name == "MoveTo")
            {
                RightPane.IsPaneOpen = !RightPane.IsPaneOpen;
            }
            else
            {
                RPane.Content = mt;
                RightPane.IsPaneOpen = true;
            }
        }

        private void BTNewEvent_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (RPane.Content.GetType().Name == "NewEventList")
            {
                RightPane.IsPaneOpen = !RightPane.IsPaneOpen;
            }
            else
            {
                RPane.Content = ne;
                RightPane.IsPaneOpen = true;
            }
        }

        private void BTFilter_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (RPane.Content.GetType().Name == "EventFilter")
            {
                RightPane.IsPaneOpen = !RightPane.IsPaneOpen;
            }
            else
            {
                RPane.Content = ef;
                RightPane.IsPaneOpen = true;
            }
        }

    }
}
