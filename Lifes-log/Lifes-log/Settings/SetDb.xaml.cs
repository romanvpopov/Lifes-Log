using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Globalization;
using Microsoft.Win32;

namespace Lifes_log.Settings
{
    public sealed partial class SetDb
    {
        private MainWindow mp;

        public SetDb()
        {
            InitializeComponent();
            DataSource.Text = (string) Registry.GetValue(App.RegRoot + "Settings", "Server", "localhost") ?? "localhost";
            InitialCatalog.Text = (string) Registry.GetValue(App.RegRoot + "Settings", "Database", "ll") ?? "ll"; 
            Login.Text = (string) Registry.GetValue(App.RegRoot + "Settings", "User", "postgres") ?? "postgres";
            Password.Password = (string) Registry.GetValue(App.RegRoot + "Settings", "Password", "") ?? "";
            if (int.TryParse((string) Registry.GetValue(App.RegRoot + "Settings", "Lang","")?? "0", out var b))
                LLang.SelectedIndex = b; 
            CurLang.Text = CultureInfo.CurrentCulture.Name+" - "+CultureInfo.CurrentCulture.DisplayName;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mp = e.Parameter as MainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registry.SetValue(App.RegRoot + "Settings", "Server", DataSource.Text);
            Registry.SetValue(App.RegRoot + "Settings", "Database", InitialCatalog.Text);
            Registry.SetValue(App.RegRoot + "Settings", "User", Login.Text);
            Registry.SetValue(App.RegRoot + "Settings", "Password", Password.Password);
            mp.Login();
        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ss = (e.AddedItems[0] as ComboBoxItem)?.Tag.ToString();
            if (ss == null) return;
            switch (ss)
            {
                case "1":
                    CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
                    App.lang = "en";
                    break;
                case "2":
                    CultureInfo.CurrentCulture = new CultureInfo("ru-ru", false);
                    App.lang = "ru";
                    break;
            }
            Registry.SetValue(App.RegRoot + "Settings", "Lang", ss);
        }

    }

}
