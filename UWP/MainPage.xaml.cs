﻿using LL.Settings;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media;

namespace LL
{
    public sealed partial class MainPage : Page {

        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        
        public MainPage() {

            InitializeComponent();
            var nvi = new NavigationViewItem {Tag = "Login",
            Content = ResourceLoader.GetForCurrentView().GetString("Log in")};
            NV.MenuItems.Add(nvi);
            MainFrame.Navigate(typeof(Login),this);
        }

        private void NV_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked) MainFrame.Navigate(typeof(SetDB));
            else {
                switch (args.InvokedItemContainer.Tag) {
                    case "Login" : MainFrame.Navigate(typeof(Login), this); break;
                    case "Logout": LogoutNav(); break;
                    case "Events": MainFrame.Navigate(typeof(LLEvent)); break;
                    case "Sport" : MainFrame.Navigate(typeof(Sport)); break;
                    case "Health": MainFrame.Navigate(typeof(Health)); break;
                    case "Money" : MainFrame.Navigate(typeof(Money)); break;
                }
            }
        }

        public void LogoutNav() {
            NV.MenuItems.Clear();
            MainFrame.Navigate(typeof(Login), this);
        }

        public void CreateNav() {
            NV.MenuItems.Clear();
            
            var icn = new FontIcon { Glyph = "\xEADF" };
            var nvi = new NavigationViewItem {Tag = "Events", Icon=icn,  
                Content = ResourceLoader.GetForCurrentView().GetString("Events")};
            NV.MenuItems.Add(nvi);
            
            icn = new FontIcon { Glyph = "\xE805" };
            nvi = new NavigationViewItem { Tag = "Sport", Icon = icn,
                Content = ResourceLoader.GetForCurrentView().GetString("Sport")};
            NV.MenuItems.Add(nvi);
            
            icn = new FontIcon { Glyph = "\xE95E" };
            nvi = new NavigationViewItem { Tag = "Health", Icon = icn,
                Content = ResourceLoader.GetForCurrentView().GetString("Health")};
            NV.MenuItems.Add(nvi);
            
            icn = new FontIcon { Glyph = "\xEF40" };
            nvi = new NavigationViewItem { Tag = "Money", Icon = icn,
                Content = ResourceLoader.GetForCurrentView().GetString("Money")};
            NV.MenuItems.Add(nvi);

            icn = new FontIcon { Glyph = "\xE787" };
            nvi = new NavigationViewItem { Tag = "Calendar", Icon = icn,
                Content = ResourceLoader.GetForCurrentView().GetString("Calendar")};
            NV.MenuItems.Add(nvi);

            icn = new FontIcon { Glyph = "\xF3B1" };
            nvi = new NavigationViewItem {Tag = "Logout", Icon = icn,
                Content = ResourceLoader.GetForCurrentView().GetString("Log out")};
            NV.MenuItems.Add(nvi);

            MainFrame.Navigate(typeof(LLEvent));
        }

        private void NV_BackRequested(NavigationView _1, NavigationViewBackRequestedEventArgs _2)
        {
            if (MainFrame.CanGoBack)
            MainFrame.GoBack();
        }

    }
}

