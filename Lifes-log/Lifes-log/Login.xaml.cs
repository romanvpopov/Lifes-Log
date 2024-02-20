using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.Storage;
using Npgsql;
using Windows.Globalization;
using System.Linq;

namespace Lifes_log
{
    public sealed partial class Login
    {
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        private MainWindow mp;
        private string constr;

        public Login()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            switch (ls.Values["LLang"])
            {
                case 1: ApplicationLanguages.PrimaryLanguageOverride = "en"; (App.Current as App).lang = "en"; break;
                case 2: ApplicationLanguages.PrimaryLanguageOverride = "ru"; (App.Current as App).lang = "ru"; break;
                default: (App.Current as App).lang = ApplicationLanguages.Languages.First()[..2]; break;
            }
            if (ls.Values.TryGetValue("LocalDB", out var value))
            {
                if ((bool)value)
                {
                    constr = "Host=localhost;Username=postgres;Password='';Database=ll";
                }
                else
                {
                    constr = $"Host={(string)ls.Values["DataSource"] ?? ""};" +
                        $" Database={(string)ls.Values["InitialCatalog"] ?? ""};" +
                        $" Username={(string)ls.Values["Login"] ?? ""};" +
                        $" Password = {(string)ls.Values["Password"] ?? ""}";
                }
            }

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(constr);
            (App.Current as App).NpDs = dataSourceBuilder.Build();
            try
            {
                (App.Current as App).NpDs.OpenConnection();
                mp.CreateNav();
            }
            catch (Exception ex)
            {
                PB.IsActive = false;
                Msg.Text = ex.Message;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) mp = (MainWindow)e.Parameter;
        }
    }
}
