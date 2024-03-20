using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using System;
using Npgsql;
using Microsoft.Win32;

namespace Lifes_log
{
    public sealed partial class Login
    {
        private MainWindow mp;
        private string constr;

        public Login()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            constr = $@"
              Host={(string)Registry.GetValue(App.RegRoot + "Settings", "Server", "localhost") ?? "localhost"};
              Database={(string)Registry.GetValue(App.RegRoot + "Settings", "Database", "ll") ?? "ll"};
              Username={(string)Registry.GetValue(App.RegRoot + "Settings", "User", "postgres") ?? "postgres"};
              Password = {(string)Registry.GetValue(App.RegRoot + "Settings", "Password", "") ?? ""}";
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(constr);
            App.NpDs = dataSourceBuilder.Build();
            try
            {
                App.NpDs.OpenConnection();
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
