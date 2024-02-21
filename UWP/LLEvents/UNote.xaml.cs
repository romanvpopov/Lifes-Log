using System;
using System.Data;
using System.Data.SqlClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LL.LLEvents
{
    public partial class UNote
    {
        private readonly string lang = (App.Current as App).lang;
        private readonly short ntp;
        private int cd;
        private readonly string cname;
        private EventBody bd;
        private readonly Event et;
        private readonly DateTime dt;

        public UNote(int cds, Event ets)
        {
            this.InitializeComponent();
            cd = cds;  et = ets;
            using (var sq = new SqlConnection((App.Current as App).ConStr)) {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText =$@"
                    Select L.Descr,LT.{lang}_Name,LT.ClassName,L.EventTypeCode
                    From LLEvent L join LLEventType LT on L.EventTypeCode = LT.Code Where L.Code = {cd}";
                var rd = cmd.ExecuteReader();
                rd.Read();
                GNote.Text = rd.GetString(0);
                TypeNote.Text = rd.GetString(1);
                cname = rd.GetString(2);
                ntp = rd.GetInt16(3);
                rd.Close();
                UBody(cmd, cd, ntp);
            }
        }

        public UNote(Int16 tp, Event ets)
        {
            ntp = tp; et = ets;
            this.InitializeComponent();
            cd = 0; dt = et.Dt;
            DelBt.Visibility = Visibility.Collapsed;
            using (var sq = new SqlConnection((App.Current as App).ConStr)) {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $"Select {lang}_Name,ClassName From LLEventType Where Code = {ntp}";
                var rd = cmd.ExecuteReader();
                rd.Read();
                TypeNote.Text = rd.GetString(0);
                cname = rd.GetString(1);
                rd.Close();
                UBody(cmd, cd, ntp);
            }
        }

        private void UBody(SqlCommand cmd,int cds,short ntps) {
            switch (cname) {
                case "Num": bd = new UNum(cmd, cds, ntps); break;
                case "Tono": bd = new UTono(cmd, cds, ntps); break;
                case "Shaving": bd = new UShaving(cmd, cds, ntps); break;
                case "Training": bd = new UTraining(cmd, cds, ntps); break;
                case "Exercise": bd = new UExercise(cmd, cds, ntps); break;
                case "List": bd = new UList(cmd, cds, ntps); break;
            }
            if (bd != null) { bd.Sf = SetGNoteFocus; MainGrid.Children.Add(bd); }
        }

        private void SetGNoteFocus() { GNote.Focus(FocusState.Programmatic); }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (bd == null) SetGNoteFocus(); else bd.GetFocus(); 
        }

        private void Log_Click(object _1, RoutedEventArgs _2)
        {
            var cmt = bd == null ? "" : bd.ToString();
            using (var sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var tr = sq.BeginTransaction(IsolationLevel.ReadCommitted);
                var cmd = sq.CreateCommand();
                cmd.Transaction = tr;
                if (cd == 0) {
                    cmd.CommandText = $@"Select Max(Code)+1 as Code From LLEvent";
                    var rd = cmd.ExecuteReader(); rd.Read();
                    cd = rd.GetInt32(0);
                    et.Code = cd;
                    rd.Close();
                    cmd.CommandText =$@"
                       Insert into LLEvent (Code,    DateT,                  DateEvent,Comment,        Descr, EventTypeCode,UserCode)
                                    Values ({cd},GETDATE(),'{dt:yyyyMMdd}','{cmt}','{GNote.Text}',        {ntp},1)";
                    cmd.ExecuteNonQuery();
                } else { 
                    cmd.CommandText = $"Update LLEvent Set Comment='{cmt}',Descr='{GNote.Text}' Where Code={cd}";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"Delete From LLEventValue Where EventCode={cd}";
                    cmd.ExecuteNonQuery();
                }
                bd?.InsertBody(cmd, cd);
                tr.Commit();
            }
            et.Collapse();
        }

        private void Delete_Click(object _1, RoutedEventArgs _2)
        {
            using (var sq = new SqlConnection((App.Current as App).ConStr)) {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $"Delete From LLEventValue Where EventCode={cd}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"Delete From LLEvent Where Code={cd}";
                cmd.ExecuteNonQuery();
            }
            cd = 0;
            et.Code = 0;
            et.Collapse();
        }

        private void Cancel_Click(object _1, RoutedEventArgs _2)
        {
            et.Collapse();
        }

        private void GNote_KeyUp(object _1, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { LogBt.Focus(FocusState.Programmatic); }
        }

    }
    public class EventBody : UserControl
    {
        public virtual void SelectBody(SqlCommand cmd, int code, short ntp) { }
        public virtual void InsertBody(SqlCommand cmd, int code) { }
        public virtual void GetFocus() { }
        public Action Sf;
    }
}

 