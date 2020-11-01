using Microsoft.Data.SqlClient;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LL.LLEvents
{
    public sealed partial class MoveTo : UserControl
    {
        public Action<String> Move;
        private readonly Button bt;
        public MoveTo()
        {
            this.InitializeComponent();
            using (var sq = new SqlConnection((App.Current as App).ConStr)) {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = "Select S.DY,Count(S.Code) From " +
                    "(Select Convert(varchar, Datepart(year, DateEvent)) as DY, Code From LLEvent) as S " +
                    "Group by S.DY Order by S.DY";
                var rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    bt = new Button() { Content = rd.GetString(0), Name = rd.GetString(0) };
                    bt.Click += Year_Click;
                    YS.Children.Add(bt);
                }
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
