using Microsoft.Data.SqlClient;
using System;
using Windows.UI.Xaml.Controls;

namespace LL.LLEvents
{
    public sealed partial class NewEventList : UserControl
    {
        private readonly string lang = (App.Current as App).lang;
        private ListView lv;
        private LLEvent le;
        private Button bt;

        public NewEventList(ListView EL,LLEvent LE)
        {
            lv = EL; le = LE;
            this.InitializeComponent();
            DateEvent.Date = DateTime.Today;
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $"Select lt.Code,lt.{lang}_Name,lt.ClassName,lt.HSM " +
                "From LLEventType lt Where lt.ClassName<>'' and lt.Turn>0" +
                $"Order by lt.Turn,lt.{lang}_Name";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bt = new Button();
                    bt.Content = reader.GetString(1);
                    bt.Name = reader.GetString(2);
                    bt.Tag = reader.GetInt16(0);
                    bt.Click += Bt_Click;
                    switch (reader.GetString(3)) {
                        case "S": PS.Children.Add(bt); break;
                        case "H": PH.Children.Add(bt); break;
                        case "M": PM.Children.Add(bt); break;
                        default : PE.Children.Add(bt); break;
                    }
                }
            }
        }

        private void Bt_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            foreach (Day dy in lv.Items)
            {
                if (dy.dt == DateEvent.Date?.Date) {
                    dy.AddEvent(Convert.ToInt16((sender as Button).Tag.ToString()));
                    lv.SelectedItem = dy;
                    lv.ScrollIntoView(lv.SelectedItem);
                    le.HidePane();
                    break;
                }
            }
            
        }
    }
}
