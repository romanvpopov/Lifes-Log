using Microsoft.UI.Xaml;
using Npgsql;

namespace WinUI3
{
    public partial class App : Application
    {
        public string lang;
        public string ConStr;
        public NpgsqlDataSource npds;

        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
    }
}
