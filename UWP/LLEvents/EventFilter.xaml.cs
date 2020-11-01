using System;
using Microsoft.Data.SqlClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LL.LLEvents
{
    public sealed partial class EventFilter : UserControl
    {
        public Action<String> Apply;
        public Action Reset;
        private readonly string lang = (App.Current as App).lang;
        private String tps;
        private readonly CheckBox bt;

        public EventFilter()
        {
            this.InitializeComponent();
            BTRS.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr)) {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $"Select lt.Code,lt.{lang}_Name,lt.ClassName,lt.HSM " +
                "From LLEventType lt Where lt.ClassName<>'' and lt.Turn>0" +
                $"Order by lt.Turn,lt.{lang}_Name";
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    bt = new CheckBox {
                        Content = reader.GetString(1),
                        Tag = reader.GetInt16(0)
                    };
                    switch (reader.GetString(3))
                    {
                        case "S": PS.Children.Add(bt); break;
                        case "H": PH.Children.Add(bt); break;
                        case "M": PM.Children.Add(bt); break;
                        default: PE.Children.Add(bt); break;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tps = "0";
            foreach (CheckBox s in PE.Children) if (s.IsChecked == true) tps = tps + "," + s.Tag;
            foreach (CheckBox s in PS.Children) if (s.IsChecked == true) tps = tps + "," + s.Tag;
            foreach (CheckBox s in PH.Children) if (s.IsChecked == true) tps = tps + "," + s.Tag;
            foreach (CheckBox s in PM.Children) if (s.IsChecked == true) tps = tps + "," + s.Tag;
            BTRS.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Apply?.Invoke(tps);
        }

        private void BTRS_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox s in PE.Children) s.IsChecked = false;
            foreach (CheckBox s in PS.Children) s.IsChecked = false;
            foreach (CheckBox s in PH.Children) s.IsChecked = false;
            foreach (CheckBox s in PM.Children) s.IsChecked = false;
            BTRS.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Reset?.Invoke();
        }
    }
}
