using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using System;
using Npgsql;
using Microsoft.Extensions.Configuration;

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
            var bld = new ConfigurationBuilder();
            var sets = new Settings();
            bld.AddJsonFile("appsettings.json").Build()
                .GetSection("Settings").Bind(sets);
            constr = $"Host={sets.Server}; Database={sets.Database};" +
                        $" Username={sets.User}; Password = {sets.Password}";
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
        private class Settings
        {
            public string Server { get; set; }
            public string Database { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
        }
    }
}
