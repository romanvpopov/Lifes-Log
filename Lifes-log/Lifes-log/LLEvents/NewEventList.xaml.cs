using Microsoft.UI.Xaml.Controls;
using System;

namespace Lifes_log.LLEvents
{
    public sealed partial class NewEventList
    {
        public Action<DateTime, short> Add;
        public Action Manage;
        public DateTime DatePic { set => DateEvent.Date = value; }
        private readonly Button bt;

        public NewEventList()
        {
            this.InitializeComponent();
            DateEvent.Date = DateTime.Today;
            var cmd = App.NpDs.CreateCommand(
              $@"Select lt.id,lt.{App.lang}_name as nm,lt.class_name,lt.hsm
               From ll_event_type lt Where lt.priority>0
               Order by lt.priority,lt.{App.lang}_Name");
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bt = new Button
                {
                    Content = reader["nm"],
                    Name = reader["class_name"].ToString(),
                    Tag = reader["id"]
                };
                bt.Click += Bt_Click;
                switch (reader["hsm"])
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
            Add(DateEvent.Date?.Date ?? DateTime.Today, Convert.ToInt16((sender as Button)?.Tag.ToString()));
        }

        private void MN_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Manage();
        }
    }
}
