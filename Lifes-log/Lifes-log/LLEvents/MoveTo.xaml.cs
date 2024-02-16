using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Lifes_log;

namespace Lifes_log.LLEvents
{
    public sealed partial class MoveTo : UserControl
    {
        public Action<String> Move;
        private readonly Button bt;
        public MoveTo()
        {
            this.InitializeComponent();
            var cmd = (App.Current as App).NpDs.CreateCommand(
             $@"Select Distinct date_part('year', event_time)::varchar as dy From ll_event order by dy");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                bt = new Button() { Content = rd.GetString(0), Name = rd.GetString(0) };
                bt.Click += Year_Click;
                YS.Children.Add(bt);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Move?.Invoke("Today");
        }

        private void Year_Click(object sender, RoutedEventArgs e)
        {
            Move?.Invoke((sender as Button).Name);
        }
    }
}
