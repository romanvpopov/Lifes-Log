using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
//using System;
//using Windows.ApplicationModel;
using Windows.Globalization;
//using Windows.Management.Core;
//using Windows.Storage;

namespace Lifes_log.Settings
{
    public sealed partial class SetDb
    {
        //readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        //readonly ApplicationDataContainer ls = ApplicationDataManager.CreateForPackageFamily(Package.Current.Id.FamilyName).LocalSettings;
        private MainWindow mp;

        public SetDb()
        {
            InitializeComponent();
            /*if (ls.Values.TryGetValue("LocalDB", out var value))
                LocalDB.IsOn = (bool)value;
            else LocalDB.IsOn = true;
            DataSource.Text = (string)ls.Values["DataSource"] ?? "";
            InitialCatalog.Text = (string)ls.Values["InitialCatalog"] ?? "";
            Login.Text = (string)ls.Values["Login"] ?? "";
            Password.Password = (string)ls.Values["Password"] ?? "";
            if (ls.Values.TryGetValue("LLang", out var lsValue))
            {
                LLang.SelectedIndex = (Int32)lsValue;
            }
            else LLang.SelectedIndex = 0;*/
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mp = e.Parameter as MainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*ls.Values["LocalDB"] = LocalDB.IsOn;
            ls.Values["DataSource"] = DataSource.Text;
            ls.Values["InitialCatalog"] = InitialCatalog.Text;
            ls.Values["Login"] = Login.Text;
            ls.Values["Password"] = Password.Password;*/
            mp.Login();
        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ss = (e.AddedItems[0] as ComboBoxItem)?.Tag.ToString();
            if (ss == null) return;
            //ls.Values["LLang"] = int.Parse(ss);
            switch (ss)
            {
                case "1":
                    ApplicationLanguages.PrimaryLanguageOverride = "en-US";
                    ((App)App.Current).lang = "en";
                    break;
                case "2":
                    ApplicationLanguages.PrimaryLanguageOverride = "ru";
                    ((App)App.Current).lang = "ru";
                    break;
            }
        }

        private void LocalDB_Toggled(object _1, RoutedEventArgs _2)
        {
            //ls.Values["LocalDB"] = LocalDB.IsOn;
        }
    }
}
