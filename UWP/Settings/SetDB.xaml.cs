using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System;
using Windows.Globalization;
using Windows.UI.Xaml.Navigation;

namespace LL.Settings
{
    public sealed partial class SetDB : Page
    {
        readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;

        public SetDB() {
            this.InitializeComponent();
            if (ls.Values.ContainsKey("LocalDB")) 
                LocalDB.IsOn = (bool)ls.Values["LocalDB"];
            else LocalDB.IsOn = true;
            DataSource.Text = (string)ls.Values["DataSource"] ?? "";
            InitialCatalog.Text = (string)ls.Values["InitialCatalog"] ?? "";
            Login.Text = (string)ls.Values["Login"] ?? "";
            Password.Password = (string)ls.Values["Password"] ?? "";
            if (ls.Values.ContainsKey("LLang")) {
                LLang.SelectedIndex = (Int32)ls.Values["LLang"];
            } else LLang.SelectedIndex = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) Args.Text = e.Parameter.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            ls.Values["LocalDB"] = LocalDB.IsOn;
            ls.Values["DataSource"] = DataSource.Text;
            ls.Values["InitialCatalog"] = InitialCatalog.Text;
            ls.Values["Login"] = Login.Text;
            ls.Values["Password"] = Password.Password;
            new Msg("Done").ShowAt(BTApply);
            
        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var ss = (e.AddedItems[0] as ComboBoxItem).Tag.ToString();
            ls.Values["LLang"] = Int32.Parse(ss);
            switch (ss) {
                case "1": ApplicationLanguages.PrimaryLanguageOverride = "en-US";
                          (App.Current as App).lang = "en";  break;
                case "2": ApplicationLanguages.PrimaryLanguageOverride = "ru";
                          (App.Current as App).lang = "ru"; break;
            }
        }

        private void LocalDB_Toggled(object _1, RoutedEventArgs _2)
        {
            ls.Values["LocalDB"] = LocalDB.IsOn;
        }
    }
}
