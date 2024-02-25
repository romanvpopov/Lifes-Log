using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Npgsql;
using System;

namespace Lifes_log.LLEvents
{
    public partial class UNote 
    {
        private readonly string lang = (App.Current as App).lang;
        private int cd;
        private readonly string cname;
        private EventBody bd;
        private readonly Event et;
        private DateTime dt;

        public UNote(int cds, Event ets)
        {
            InitializeComponent();
            cd = cds;
            et = ets;
            var cmd = ((App)App.Current).NpDs.CreateCommand(
                $"Select l.comment,lt.{lang}_name as nm,lt.class_name,l.event_type_id "+
                "From ll_event l join ll_event_type lt on l.event_type_id = lt.id "+
                "Where l.id = {cd}");
            var rd = cmd.ExecuteReader();
            rd.Read();
            GNote.Text = rd["comment"].ToString();
            TypeNote.Text = rd["nm"].ToString();
            cname = rd["class_name"].ToString();
            var ntp = (short)rd["event_type_id"];
            rd.Close();
            UBody(cmd, this.cd, ntp);
        }

        public UNote(short tp, Event ets)
        {
            var ntp = tp; et = ets;
            InitializeComponent();
            cd = 0; dt = et.Dt;
            DelBt.Visibility = Visibility.Collapsed;
            var cmd = (App.Current as App).NpDs.CreateCommand(
            $"Select {lang}_name as nm,class_name From ll_event_type Where id = {ntp}");
            var rd = cmd.ExecuteReader();
            rd.Read();
            TypeNote.Text = rd["nm"].ToString();
            cname = rd["class_name"].ToString();
            rd.Close();
            UBody(cmd, cd, ntp);
        }

        private void UBody(NpgsqlCommand cmd, int cds, short ntp)
        {
            bd = cname switch
            {
                "Num" => new UNum(cds, ntp),
                "Tono" => new UTono(cmd, cds),
                "Shaving" => new UShaving(cmd, cds),
                "Training" => new UTraining(cmd, cds, ntp),
                "Exercise" => new UExercise(cmd, cds, ntp),
                "List" => new UList(cmd, cds, ntp),
                _ => bd
            };
            if (bd == null) return;
            bd.Sf = SetGNoteFocus;
            MainGrid.Children.Add(bd);
        }

        private void SetGNoteFocus() { GNote.Focus(FocusState.Programmatic); }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (bd == null) SetGNoteFocus(); else bd.GetFocus();
        }

        private void Log_Click(object  _1, RoutedEventArgs _2)
        {
            var cmt = bd == null ? "" : bd.ToString();
            if (cd == 0)
            {
                var cmd = (App.Current as App).NpDs.CreateCommand(
                 "Select Max(id)+1 as Code From ll_event");
                var rd = cmd.ExecuteReader(); rd.Read();
                cd = rd.GetInt32(0);
                et.Code = cd;
                rd.Close();
                cmd.CommandText =
                    $"INSERT INTO ll_event (  id,server_time,    event_time,comment, description, event_type_id)"+
                                 "values   ({cd}, localtime,'{dt:yyyyMMdd}','{cmt}','{GNote.Text}',      {ntp})";
                cmd.ExecuteNonQuery();
            }
            else
            {
                var cmd = (App.Current as App).NpDs.CreateCommand(
                $"Update ll_event Set comment='{cmt}',description='{GNote.Text}' Where id={cd}");
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"Delete From ll_value Where event_id={cd}";
                cmd.ExecuteNonQuery();
            }
            if (bd != null)
            {
                var cmd = (App.Current as App).NpDs.CreateCommand("");
                bd.InsertBody(cmd, cd);
            }
            et.Collapse();
        }

        private void Delete_Click(object _1, RoutedEventArgs _2)
        {
            var cmd = (App.Current as App).NpDs.CreateCommand(
            $"Delete From LLEventValue Where EventCode={cd}");
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"Delete From LLEvent Where Code={cd}";
            cmd.ExecuteNonQuery();
            cd = 0;
            et.Code = 0;
            et.Collapse();
        }

        private void Cancel_Click(object _1, RoutedEventArgs _2)
        {
            et.Collapse();
        }

        private void GNote_KeyUp(object _1, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { LogBt.Focus(FocusState.Programmatic); }
        }

    }
    public class EventBody : UserControl
    {
        public virtual void SelectBody(NpgsqlCommand cmd, int code, short ntp) { }
        public virtual void InsertBody(NpgsqlCommand cmd, int code) { }
        public virtual void GetFocus() { }
        public Action Sf;
    }
}
