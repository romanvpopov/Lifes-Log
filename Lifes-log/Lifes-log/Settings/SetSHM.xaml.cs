using Microsoft.UI.Xaml.Controls;

namespace Lifes_log.DBSettings
{
    public sealed partial class SetShm
    {
        private readonly string lang = (App.Current as App).lang;

        public SetShm(string et)
        {
            this.InitializeComponent();
            switch (et)
            {
                case "S": Fi.Glyph = "\xE805"; Tx.Text = "Sport"; break;
                case "H": Fi.Glyph = "\xE95E"; Tx.Text = "Health"; break;
            }
            var cmd = ((App)App.Current).NpDs.CreateCommand(
              $@"Select lt.Code,lt.{lang}_Name,lt.ClassName,lt.HSM
                 From LLEventType lt Where lt.ClassName<>'' and lt.HSM='{et}'
                Order by lt.Turn,lt.{lang}_Name");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Lv.Items.Add(new ListViewItem { Content = rd.GetString(1), CanDrag = true, AllowDrop = true });
            }
        }
    }
}

