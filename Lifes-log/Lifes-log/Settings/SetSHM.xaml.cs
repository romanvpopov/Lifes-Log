using Microsoft.UI.Xaml.Controls;
using System;
using Lifes_log;

namespace WinUI3.Settings
{
    public sealed partial class SetSHM : UserControl
    {
        private readonly string lang = (App.Current as App).lang;

        public SetSHM(String et)
        {
            this.InitializeComponent();
            switch (et)
            {
                case "S": FI.Glyph = "\xE805"; TX.Text = "Sport"; break;
                case "H": FI.Glyph = "\xE95E"; TX.Text = "Health"; break;
            }
            var cmd = (App.Current as App).NpDs.CreateCommand(
              $@"Select lt.Code,lt.{lang}_Name,lt.ClassName,lt.HSM
                 From LLEventType lt Where lt.ClassName<>'' and lt.HSM='{et}'
                Order by lt.Turn,lt.{lang}_Name");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                LV.Items.Add(new ListViewItem { Content = rd.GetString(1), CanDrag = true, AllowDrop = true });
            }
        }
    }
}

