using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Npgsql;
using System;

namespace LL
{
    public sealed partial class Login : Page
    {
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        private MainPage mp;

        public Login()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ls.Values.ContainsKey("LocalDB")) {
                if ((bool)ls.Values["LocalDB"]) {
                    (App.Current as App).ConStr = "Host=localhost;Username=postgres;Password='';Database=LL";
                } else {
                    (App.Current as App).ConStr = $"Host={(string)ls.Values["DataSource"] ?? ""};" +
                        $" Database={(string)ls.Values["InitialCatalog"] ?? ""};" +
                        $" Username={(string)ls.Values["Login"] ?? ""};" +
                        $" Password = {(string)ls.Values["Password"] ?? ""}";
                }
            }

            (App.Current as App).ConStr = "Host=localhost;Username=postgres;Password=;Database=ll";
            var dataSourceBuilder = new NpgsqlDataSourceBuilder((App.Current as App).ConStr);
            var dataSource = dataSourceBuilder.Build();
                try {
                await dataSource.OpenConnectionAsync();
                mp.CreateNav();
                }
                catch (Exception ex) {
                    PB.IsActive = false;
                    Msg.Text = ex.Message;
                }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) mp = (MainPage)e.Parameter;
        }
    }
}
