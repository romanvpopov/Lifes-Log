using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Lifes_log.Settings
{
    public sealed partial class SetDb
    {
        private MainWindow mp;
        private readonly Settings sets = new();
        private readonly ConfigurationBuilder bld = new();

        public SetDb()
        {
            InitializeComponent();
            bld.AddJsonFile("appsettings.json").Build()
                .GetSection("Settings").Bind(sets);
            DataSource.Text = sets?.Server;
            InitialCatalog.Text = sets?.Database;
            Login.Text = sets?.User;
            Password.Password = sets?.Password;
            LLang.SelectedIndex = (int)(sets?.Lang);
            CurLang.Text = CultureInfo.CurrentCulture.Name+" - "+CultureInfo.CurrentCulture.DisplayName;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mp = e.Parameter as MainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using StreamWriter writer = new("appsettings.json", false);
            writer.WriteLine(
              $"{{\"Settings\": {{ \"Lang\": \"{(LLang.SelectedItem as ComboBoxItem).Tag}\",\"Server\": \"{DataSource.Text}\","+
              $"\"Database\": \"{InitialCatalog.Text}\",\"User\": \"{Login.Text}\",\"Password\": \"{Password.Password}\"}}}}");
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
                    ((App)App.Current).lang = "en";
                    break;
                case "2":
                    CultureInfo.CurrentCulture = new CultureInfo("ru-ru", false);
                    ((App)App.Current).lang = "ru";
                    break;
            }
        }

        private class Settings
        {
            public int Lang { get; set; }
            public string Server { get; set; }
            public string Database { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
        }

    }

}
