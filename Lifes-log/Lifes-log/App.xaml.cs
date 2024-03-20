using Microsoft.UI.Xaml;
using Npgsql;
using System.Globalization;
using Microsoft.Win32;

namespace Lifes_log
{
    public partial class App
    {
        public static string lang;
        public static NpgsqlDataSource NpDs;
        public static readonly string RegRoot = "HKEY_CURRENT_USER\\Software\\mt-soft.ru\\lifes-log\\";

        public App()
        {
            InitializeComponent();
        }
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (int.TryParse((string)Registry.GetValue(App.RegRoot + "Settings", "Lang", "") ?? "0", out var b))
            {
                switch (b)
                {
                    case 1: CultureInfo.CurrentCulture = new CultureInfo("en-US", false); lang = "en"; break;
                    case 2: CultureInfo.CurrentCulture = new CultureInfo("ru-ru", false); lang = "ru"; break;
                    default: lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName[..2]; break;
                }
            }
            if (lang != "ru" & lang != "en") lang = "en";
            mWindow = new MainWindow();
            mWindow.Activate();
        }

        private Window mWindow;
    }
}
