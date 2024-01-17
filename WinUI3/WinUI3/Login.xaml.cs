using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.Storage;
using Npgsql;

namespace WinUI3
{
    public sealed partial class Login : Page
    {
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        private MainWindow mp;

        public Login()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ls.Values.ContainsKey("LocalDB"))
            {
                if ((bool)ls.Values["LocalDB"])
                {
                    (App.Current as App).ConStr = "Host=localhost;Username=postgres;Password='';Database=LL";
                }
                else
                {
                    (App.Current as App).ConStr = $"Host={(string)ls.Values["DataSource"] ?? ""};" +
                        $" Database={(string)ls.Values["InitialCatalog"] ?? ""};" +
                        $" Username={(string)ls.Values["Login"] ?? ""};" +
                        $" Password = {(string)ls.Values["Password"] ?? ""}";
                }
            }

            (App.Current as App).ConStr = "Host=localhost;Username=postgres;Password=;Database=ll";
            var dataSourceBuilder = new NpgsqlDataSourceBuilder((App.Current as App).ConStr);
            (App.Current as App).npds = dataSourceBuilder.Build();
            try
            {
                (App.Current as App).npds.OpenConnection();
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
