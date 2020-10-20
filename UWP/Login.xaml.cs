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

        public Login() {

            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            Msg.Text = "Подключение";
            var ans = CheckDB();
            if (ans == "")
            {
                if (mp != null) mp.CreateNav();
            }
            else { Msg.Text = ans; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) mp = (MainPage) e.Parameter;
        }

        private string CheckDB() {

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
            //(App.Current as App).ConStr = "Data Source = sql.mt-soft.ru,44433; Initial Catalog = LL; User Id=sa; Password=P12455p93; Integrated Security=False";
            sq = new SqlConnection((App.Current as App).ConStr);
            try
            {
            sq.Open();
            if (sq.State == System.Data.ConnectionState.Open) { return ""; }
                else { return "error"; }
            } catch (Exception e) { return e.Message; }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var ans = CheckDB();
            if (ans == "") {
                if (mp != null)  mp.CreateNav(); 
            }
            else { Msg.Text = ans; }
        }
    }
}
