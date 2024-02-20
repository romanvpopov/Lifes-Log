using System;
using System.Data;
using System.Data.SqlClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LL.LLEvents
{
    public partial class UNote : UserControl
    {
        protected string lang = (App.Current as App).lang;
        protected Int16 ntp;
        protected Int32 cd;
        protected string cname;
        protected EventBody Bd;
        protected Event et;
        protected DateTime dt;

        public UNote(Int32 Cd, Event Et)
        {
            this.InitializeComponent();
            cd = Cd;  et = Et;
            using (var sq = new SqlConnection((App.Current as App).ConStr)) {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText =
                    $"Select L.Descr,LT.{lang}_Name,LT.ClassName,L.EventTypeCode " +
                    $"From LLEvent L join LLEventType LT on L.EventTypeCode = LT.Code Where L.Code = {cd}";
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

        public UNote(Int16 tp, Event Et)
        {
            ntp = tp; et = Et;
            this.InitializeComponent();
            cd = 0; dt = et.Dt;
            DelBt.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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

        private void UBody(SqlCommand cmd,Int32 cd,Int16 ntp) {
            switch (cname) {
                case "Num": Bd = new UNum(cmd, cd, ntp); break;
                case "Tono": Bd = new UTono(cmd, cd, ntp); break;
                case "Shaving": Bd = new UShaving(cmd, cd, ntp); break;
                case "Training": Bd = new UTraining(cmd, cd, ntp); break;
                case "Exercise": Bd = new UExercise(cmd, cd, ntp); break;
                case "List": Bd = new UList(cmd, cd, ntp); break;
            }
            if (Bd != null) { Bd.Sf = SetGNoteFocus; MainGrid.Children.Add(Bd); }
        }

        public void SetGNoteFocus() { GNote.Focus(FocusState.Programmatic); }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Bd == null) SetGNoteFocus(); else Bd.GetFocus(); 
        }

        private void Log_Click(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
        {
            var cmt = Bd == null ? "" : Bd.ToString();
            using (var sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var tr = sq.BeginTransaction(IsolationLevel.ReadCommitted);
                var cmd = sq.CreateCommand();
                cmd.Transaction = tr;
                if (cd == 0) {
                    cmd.CommandText = "Select Max(Code)+1 as Code From LLEvent";
                    var rd = cmd.ExecuteReader(); rd.Read();
                    cd = rd.GetInt32(0);
                    et.Code = cd;
                    rd.Close();
                    cmd.CommandText =
                        "Insert into LLEvent (Code,    DateT,                  DateEvent,Comment,        Descr, EventTypeCode,UserCode) " +
                                    $"Values ({cd},GETDATE(),'{dt.ToString("yyyyMMdd")}','{cmt}','{GNote.Text}',        {ntp},1)";
                    cmd.ExecuteNonQuery();
                } else { 
                    cmd.CommandText = $"Update LLEvent Set Comment='{cmt}',Descr='{GNote.Text}' Where Code={cd}";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"Delete From LLEventValue Where EventCode={cd}";
                    cmd.ExecuteNonQuery();
                }
                if (Bd != null) {
                   Bd.InsertBody(cmd, cd);
                }
                tr.Commit();
            }
            et.Collapse();
        }

        private void Delete_Click(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
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

        private void Cancel_Click(object _1, Windows.UI.Xaml.RoutedEventArgs _2)
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
        public virtual void SelectBody(SqlCommand cmd, Int32 Code, Int16 ntp) { }
        public virtual void InsertBody(SqlCommand cmd, Int32 Code) { }
        public virtual void GetFocus() { }
        public Action Sf;
    }
}

 