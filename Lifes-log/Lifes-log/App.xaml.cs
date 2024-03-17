using Microsoft.UI.Xaml;
using Npgsql;
using System.Globalization;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;

namespace Lifes_log
{
    public partial class App
    {
        public static string lang;
        public static NpgsqlDataSource NpDs;
        //private readonly HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            /*builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });*/
            var bld = new ConfigurationBuilder();
            var sets = new Settings();
            bld.AddJsonFile("appsettings.json").Build()
                .GetSection("Settings").Bind(sets);
            switch (sets.Lang)
            {
                case 1: CultureInfo.CurrentCulture = new CultureInfo("en-US", false); lang = "en"; break;
                case 2: CultureInfo.CurrentCulture = new CultureInfo("ru-ru", false); lang = "ru"; break;
                default: lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName[..2]; break;
            }
            if (lang != "ru" & lang != "en") lang = "en";
            mWindow = new MainWindow();
            mWindow.Activate();
        }

        private Window mWindow;
        private class Settings
        {
            public int Lang { get; }
        }
    }
}
