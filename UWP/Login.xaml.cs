using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Data.SqlClient;
using Windows.Globalization;
using System;

namespace LL
{
    public sealed partial class Login : Page
    {
        private readonly ApplicationDataContainer ls = ApplicationData.Current.LocalSettings;
        private SqlConnection sq = new SqlConnection();
        private MainPage mp;

        public Login()
        {

            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ls.Values.ContainsKey("LocalDB")) {
                if ((bool)ls.Values["LocalDB"]) {
                    (App.Current as App).ConStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LL;Integrated Security=True;Timeout=5";
                } else {
                    (App.Current as App).ConStr = $"Data Source={(string)ls.Values["DataSource"] ?? ""};" +
                        $" Initial Catalog={(string)ls.Values["InitialCatalog"] ?? ""};" +
                        $" User Id={(string)ls.Values["Login"] ?? ""};" +
                        $" Password = {(string)ls.Values["Password"] ?? ""}";
                }
            }
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
                try {
                    await sq.OpenAsync();
                    while (sq.State == System.Data.ConnectionState.Connecting) { }
                    if (sq.State == System.Data.ConnectionState.Open)
                        mp.CreateNav();
                    else {
                        PB.IsActive = false;
                        Msg.Text = "Connection error";
                    }
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
