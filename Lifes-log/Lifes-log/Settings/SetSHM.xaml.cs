using Microsoft.UI.Xaml.Controls;

namespace Lifes_log.DBSettings
{
    public sealed partial class SetShm
    {
        public SetShm(string et)
        {
            this.InitializeComponent();
            switch (et)
            {
                case "S": Fi.Glyph = "\xE805"; Tx.Text = "Sport"; break;
                case "H": Fi.Glyph = "\xE95E"; Tx.Text = "Health"; break;
            }
            var cmd = App.NpDs.CreateCommand($@"
                Select lt.id,lt.{App.lang}_Name,lt.class_name,lt.HSM
                From ll_event_type lt Where lt.class_name<>'' and lt.HSM='{et}'
                Order by lt.priority,lt.{App.lang}_name");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Lv.Items.Add(new ListViewItem { Content = rd.GetString(1), CanDrag = true, AllowDrop = true });
            }
            rd.Close();
        }
    }
}

