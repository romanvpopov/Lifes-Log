using Microsoft.UI.Xaml.Controls;
using System;
using Lifes_log;

namespace Lifes_log.LLEvents
{
    public sealed partial class NewEventBody : UserControl
    {
        private readonly string lang = (App.Current as App).lang;
        private NewEvent ne;
        private Day pd;
        private Button bt;

        public NewEventBody(NewEvent Ne, Day Pd)
        {
            this.InitializeComponent();
            ne = Ne; pd = Pd;
            var cmd = (App.Current as App).NpDs.CreateCommand(
            $@"Select lt.id,lt.{lang}_short_name,lt.class_name,lt.hsm,lt.{lang}_Name
               From ll_event_type lt Where lt.class_name<>'' and lt.priority>0
               Order by lt.priority,lt.{lang}_short_name");
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bt = new Button();
                bt.Content = (reader.GetString(1) == "" ? reader.GetString(4) : reader.GetString(1));
                bt.Name = reader.GetString(2);
                bt.Tag = reader.GetInt16(0);
                bt.Click += Bt_Click;
                switch (reader.GetString(3))
                {
                    case "S": PS.Children.Add(bt); break;
                    case "H": PH.Children.Add(bt); break;
                    case "M": PM.Children.Add(bt); break;
                    default: PE.Children.Add(bt); break;
                }
            }
        }

        private void Bt_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            pd.AddEvent(Convert.ToInt16((sender as Button).Tag.ToString()));
            ne.Collapse();
        }

        private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ne.Collapse();
        }
    }
}
