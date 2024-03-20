using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Lifes_log.LLEvents
{
    public sealed partial class MoveTo
    {
        public Action<string> Move;
        private readonly Button bt;
        public MoveTo()
        {
            this.InitializeComponent();
            var cmd = App.NpDs.CreateCommand($@"
                Select date_part('year', event_time)::varchar as dy,count(id) as ids
                From ll_event
                Group by date_part('year', event_time)
                Order by dy");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                bt = new Button() { Content = rd.GetString(0)+" ("+rd.GetInt32(1).ToString()+")", Name = rd.GetString(0) };
                bt.Click += Year_Click;
                YS.Children.Add(bt);
            }
            rd.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Move?.Invoke("Today");
        }

        private void Year_Click(object sender, RoutedEventArgs e)
        {
            Move?.Invoke((sender as Button)?.Name);
        }
    }
}
