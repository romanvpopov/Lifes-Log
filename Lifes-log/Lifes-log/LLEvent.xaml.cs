using Microsoft.UI.Xaml.Controls;
using System;
using WinUI3.LLEvents;
using Windows.Storage;


namespace WinUI3
{
    public sealed partial class LLEvent : Page
    {

        private readonly string lang = (App.Current as App).lang;
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        private readonly DayList ds;
        private readonly EventFilter ef;
        private readonly NewEventList ne;
        private readonly MoveTo mt;

        public LLEvent()
        {
            InitializeComponent();
            //NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            ds = new DayList(DateTime.Today);
            ef = new EventFilter { Apply = ApplyFilter, Reset = ResetFilter };
            mt = new MoveTo { Move = Move };
            ne = new NewEventList
            {
                Add = Add,
                Manage = () => { Frame.Navigate(typeof(Settings.SetEventType)); }
            };
            if (ls.Values.ContainsKey("FixPane")) FixPane.IsOn = (bool)ls.Values["FixPane"];
            RPane.Content = ne;
        }

        private void EL_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            EL.ItemsSource = ds;
        }

        private void Add(DateTime dt, Int16 tp)
        {
            foreach (object dy in EL.Items)
            {
                if (dy.GetType().Name == "Day")
                {
                    if ((dy as Day).dt == dt)
                    {
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
            ds.Clear();
            ds.HasMoreItems = true;
            if (ss == "Today")
            {
                ds.dt = DateTime.Today;
                ne.DatePic = DateTime.Today;
            }
            else
                ds.dt = new DateTime(Int32.Parse(ss), 12, 31);
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

        private void FixPane_Toggled(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ls.Values["FixPane"] = FixPane.IsOn;
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
