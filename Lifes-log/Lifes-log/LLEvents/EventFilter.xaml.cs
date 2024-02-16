using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Lifes_log;

namespace Lifes_log.LLEvents
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
            InitializeComponent();
            BTRS.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            var cmd = (App.Current as App).NpDs.CreateCommand(
            $@"Select lt.id,lt.{lang}_name as nm,lt.class_name,lt.hsm
               From ll_event_type lt Where lt.priority>0
               Order by lt.priority,lt.{lang}_name");
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bt = new CheckBox
                {
                    Content = reader["id"],
                    Tag = reader["nm"]
                };
                switch (reader["hsm"])
                {
                    case "S": PS.Children.Add(bt); break;
                    case "H": PH.Children.Add(bt); break;
                    case "M": PM.Children.Add(bt); break;
                    default: PE.Children.Add(bt); break;
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
            BTRS.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            Apply?.Invoke(tps);
        }

        private void BTRS_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox s in PE.Children) s.IsChecked = false;
            foreach (CheckBox s in PS.Children) s.IsChecked = false;
            foreach (CheckBox s in PH.Children) s.IsChecked = false;
            foreach (CheckBox s in PM.Children) s.IsChecked = false;
            BTRS.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            Reset?.Invoke();
        }
    }
}
