using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using Npgsql;

namespace Lifes_log.LLEvents
{
    public class UEvent : UserControl
    {
        private int cd;
        private readonly string cname;
        private EventBody bd;
        private readonly Event et;
        private readonly DateTime dt;
        private readonly short ntp;

        private TextBox GNote;
        private TextBlock TypeNote;
        private Button LogBt;
        private Button BtCancel;
        private Button BtDel;
        private Grid MainGrid;

        public UEvent(int cds, Event ets)
        {
            cd = cds;
            et = ets;
            InitControls();
            var cmd = App.NpDs.CreateCommand(
                $@"Select l.comment,lt.{App.lang}_name as nm,lt.class_name,l.event_type_id
                From ll_event l join ll_event_type lt on l.event_type_id = lt.id
                Where l.id = {cd}");
            var rd = cmd.ExecuteReader();
            rd.Read();
            GNote.Text = rd["comment"].ToString();
            TypeNote.Text = rd["nm"].ToString();
            cname = rd["class_name"].ToString();
            ntp = (short)rd["event_type_id"];
            rd.Close();
            UBody(cmd, cd, ntp);
            Content = MainGrid;
        }
        public UEvent(short tp, Event ets)
        {
            ntp = tp; et = ets;
            cd = 0; dt = et.Dt;
            InitControls();
            BtDel.Visibility = Visibility.Collapsed;
            var cmd = App.NpDs.CreateCommand(
            $"Select {App.lang}_name as nm,class_name From ll_event_type Where id = {ntp}");
            var rd = cmd.ExecuteReader();
            rd.Read();
            TypeNote.Text = rd["nm"].ToString();
            cname = rd["class_name"].ToString();
            rd.Close();
            UBody(cmd, cd, ntp);
            Content = MainGrid;
        }

        private void UBody(NpgsqlCommand cmd, int cds, short tp)
        {
            bd = cname switch
            {
                "Num" => new UNum(cds, tp),
                "BPM" => new UBPM(cmd, cds),
                "Shaving" => new UShaving(cmd, cds),
                "Training" => new UTraining(cmd, cds, tp),
                "Exercise" => new UExercise(cmd, cds),
                "List" => new UList(cmd, cds),
                _ => bd
            };
            if (bd == null) return;
            bd.Sf = ()=>{ GNote.Focus(FocusState.Programmatic); };
            MainGrid.Children.Add(bd);
        }

        private void Log_Click(object _1, RoutedEventArgs _2)
        {
            var cmt = bd == null ? "" : bd.ToString();
            if (cd == 0)
            {
                var cmd = App.NpDs.CreateCommand(
                 "Select Max(id)+1 as Code From ll_event");
                var rd = cmd.ExecuteReader(); rd.Read();
                cd = rd.GetInt32(0);
                et.Code = cd;
                rd.Close();
                cmd.CommandText = $@"
                   INSERT INTO ll_event (  id,    server_time,    event_time,        comment,description, event_type_id)
                                Values   ({cd}, localtimestamp,'{dt:yyyyMMdd}','{GNote.Text}',    '{cmt}',  {ntp})";
                cmd.ExecuteNonQuery();
            }
            else
            {
                var cmd = App.NpDs.CreateCommand(
                $"Update ll_event Set description='{cmt}',comment='{GNote.Text}' Where id={cd}");
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"Delete From ll_value Where event_id={cd}";
                cmd.ExecuteNonQuery();
            }
            if (bd != null)
            {
                var cmd = App.NpDs.CreateCommand("");
                bd.InsertBody(cmd, cd);
            }
            et.Collapse();
        }

        private void InitControls() {

            var rl = new ResourceLoader();

            GNote = new() { Width = 400,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Left
            };
            GNote.KeyUp += new KeyEventHandler(GNote_KeyUp);
            GNote.SetValue(Grid.RowProperty, 2);
            GNote.SetValue(Grid.ColumnSpanProperty, 4);

            TypeNote = new() {
                VerticalAlignment = VerticalAlignment.Center };
            TypeNote.SetValue(Grid.ColumnSpanProperty, 2);

            LogBt = new() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = rl.GetString("BTLog") };
            LogBt.SetValue(Grid.RowProperty, 3);
            LogBt.Click += new RoutedEventHandler(Log_Click);

            BtCancel = new() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = rl.GetString("BTCancel") };
            BtCancel.Click += new RoutedEventHandler(Cancel_Click);
            BtCancel.SetValue(Grid.RowProperty, 3);
            BtCancel.SetValue(Grid.ColumnProperty, 1);

            BtDel = new() {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = rl.GetString("BTDel")};
            BtDel.SetValue(Grid.RowProperty, 3);
            BtDel.SetValue(Grid.ColumnProperty, 3);
            var stp = new StackPanel { Orientation = Orientation.Vertical };
            stp.Children.Add(new TextBlock { Text = rl.GetString("DelConfirm") });
            var btd = new Button {
                Content = rl.GetString("BtDel"),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            btd.Click += new RoutedEventHandler(Delete_Click);
            stp.Children.Add(btd);
            BtDel.Flyout = new Flyout { Content = stp };

            MainGrid = new() {
                RowDefinitions = {
                new RowDefinition { Height = GridLength.Auto},
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto } },
                ColumnDefinitions = {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto }},
            };
            MainGrid.Children.Add(TypeNote);
            MainGrid.Children.Add(GNote);
            MainGrid.Children.Add(LogBt);
            MainGrid.Children.Add(BtCancel);
            MainGrid.Children.Add(BtDel);
        }

        private void Delete_Click(object _1, RoutedEventArgs _2)
        {
            var cmd = App.NpDs.CreateCommand(
            $"Delete From ll_value Where event_id={cd}");
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"Delete From ll_event Where id={cd}";
            cmd.ExecuteNonQuery();
            cd = 0;
            et.Code = 0;
            et.Collapse();
        }

        private void Cancel_Click(object _1, RoutedEventArgs _2)
        {
            et.Collapse();
        }

        private void GNote_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { LogBt.Focus(FocusState.Programmatic); }
        }

    }
}
