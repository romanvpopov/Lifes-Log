using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI3;

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
            Nv.MenuItems.Clear();
            Nv.AutoSuggestBox.Visibility = Visibility.Collapsed;
            int cacheSize = MainFrame.CacheSize;
            MainFrame.CacheSize = 0;
            MainFrame.CacheSize = cacheSize;
            MainFrame.Navigate(typeof(Login), this);
        }

        public void CreateNav()
        {
            Nv.MenuItems.Clear();
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Events",
                Icon = new FontIcon { Glyph = "\xEADF" },
                Content = "Events"
            });
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Sport",
                Icon = new FontIcon { Glyph = "\xE805" },
                Content = "Sport"
            });
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Health",
                Icon = new FontIcon { Glyph = "\xE95E" },
                Content = "Health"
            });
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Money",
                Icon = new FontIcon { Glyph = "\xEF40" },
                Content = "Money"
            });
            Nv.MenuItems.Add(new NavigationViewItem
            {
                Tag = "Calendar",
                Icon = new FontIcon { Glyph = "\xE787" },
                Content = "Calendar"
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
