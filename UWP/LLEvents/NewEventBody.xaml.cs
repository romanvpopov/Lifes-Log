using System.Data.SqlClient;
using System;
using Windows.UI.Xaml.Controls;

namespace LL.LLEvents
{
    public sealed partial class NewEventBody
    {
        private readonly string lang = (App.Current as App).lang;
        private readonly NewEvent ne;
        private readonly Day pd;
        private readonly Button bt;

        public NewEventBody(NewEvent nes, Day pds)
        {
            this.InitializeComponent();
            ne = nes; pd = pds;
            using (var sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $@"
                  Select lt.Code,lt.{lang}_ShortName,lt.ClassName,lt.HSM,lt.{lang}_Name
                  From LLEventType lt Where lt.ClassName<>'' and lt.Turn>0
                  Order by lt.Turn,lt.{lang}_ShortName";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bt = new Button
                    {
                        Content = (reader.GetString(1) == "" ? reader.GetString(4) : reader.GetString(1)),
                        Name = reader.GetString(2),
                        Tag = reader.GetInt16(0)
                    };
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
            pd.AddEvent(Convert.ToInt16((sender as Button)?.Tag.ToString()));
            ne.Collapse();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ne.Collapse();
        }
    }
}
