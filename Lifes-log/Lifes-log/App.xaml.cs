using Microsoft.UI.Xaml;
using Npgsql;

namespace Lifes_log
{
    public partial class App
    {
        public string lang;
        public string ConStr;
        public NpgsqlDataSource NpDs;

        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            mWindow = new MainWindow();
            mWindow.Activate();
        }

        private Window mWindow;
    }
}
