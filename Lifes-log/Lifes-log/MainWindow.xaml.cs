using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUI3
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void NV_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked) MainFrame.Navigate(typeof(Settings.SetDB), this);
            else
            {
                //switch (args.InvokedItemContainer.Tag)
                //{
                    //case "Events": MainFrame.Navigate(typeof(LLEvent), this); break;
                    //case "Sport": MainFrame.Navigate(typeof(Sport), this); break;
                    //case "Health": MainFrame.Navigate(typeof(Health), this); break;
                    //case "Money": MainFrame.Navigate(typeof(Money), this); break;
                //}
            }
        }

        public void Login()
        {
            NV.MenuItems.Clear();
            NV.AutoSuggestBox.Visibility = Visibility.Collapsed;
            int cacheSize = MainFrame.CacheSize;
            MainFrame.CacheSize = 0;
            MainFrame.CacheSize = cacheSize;
            MainFrame.Navigate(typeof(Login), this);
        }

        public void CreateNav()
        {
            NV.MenuItems.Clear();
            NV.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Events",
                Icon = new FontIcon { Glyph = "\xEADF" },
                Content = "Events"
            });
            NV.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Sport",
                Icon = new FontIcon { Glyph = "\xE805" },
                Content = "Sport"
            });
            NV.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Health",
                Icon = new FontIcon { Glyph = "\xE95E" },
                Content = "Health"
            });
            NV.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Money",
                Icon = new FontIcon { Glyph = "\xEF40" },
                Content = "Money"
            });
            NV.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Calendar",
                Icon = new FontIcon { Glyph = "\xE787" },
                Content = "Calendar"
            });
            MainFrame.Navigate(typeof(LLEvent), this);
            NV.AutoSuggestBox.Visibility = Visibility.Visible;
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
