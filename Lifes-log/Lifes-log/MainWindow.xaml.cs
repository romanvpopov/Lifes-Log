using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;

namespace Lifes_log
{
    public sealed partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void NV_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked) MainFrame.Navigate(typeof(Settings.SetDb), this);
            else
            {
                switch (args.InvokedItemContainer.Tag)
                {
                    case "Events": MainFrame.Navigate(typeof(LlEvent), this); break;
                    //case "Sport": MainFrame.Navigate(typeof(Sport), this); break;
                    //case "Health": MainFrame.Navigate(typeof(Health), this); break;
                    //case "Money": MainFrame.Navigate(typeof(Money), this); break;
                }
            }
        }

        public void Login()
        {
            Nv.MenuItems.Clear();
            Nv.AutoSuggestBox.Visibility = Visibility.Collapsed;
            int cacheSize = MainFrame.CacheSize;
            MainFrame.CacheSize = 0;
            MainFrame.CacheSize = cacheSize;
            MainFrame.Navigate(typeof(Login), this);
        }

        public void CreateNav()
        {
            var rl = new ResourceLoader();
            Nv.MenuItems.Clear();
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Events",
                Icon = new FontIcon { Glyph = "\xEADF" },
                Content = rl.GetString("Events")
            });
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Sport",
                Icon = new FontIcon { Glyph = "\xE805" },
                Content = rl.GetString("Sport")
            });
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Health",
                Icon = new FontIcon { Glyph = "\xE95E" },
                Content = rl.GetString("Health")
            });
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Money",
                Icon = new FontIcon { Glyph = "\xEF40" },
                Content = rl.GetString("Money")
            });
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Calendar",
                Icon = new FontIcon { Glyph = "\xE787" },
                Content = rl.GetString("Calendar")
            });
            MainFrame.Navigate(typeof(LlEvent), this);
            Nv.AutoSuggestBox.Visibility = Visibility.Visible;
        }

        private void NV_BackRequested(NavigationView _1, NavigationViewBackRequestedEventArgs _2)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }

        
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //MainFrame.Navigate(typeof(Search), args.QueryText);
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            Login();
        }
    }
}
