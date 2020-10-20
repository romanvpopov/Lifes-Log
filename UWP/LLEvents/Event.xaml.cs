using LL.LLEvents;
using Microsoft.Data.SqlClient;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace LL
{
    public sealed partial class Event : UserControl
    {
        private Boolean exp;
        public Int32 Code;
        public DateTime Dt;
        private Int16 tp;
        private readonly string lang = (App.Current as App).lang;

        public Event(String st, Int32 EventCode, Int16 EventType)
        {
            this.InitializeComponent();
            Code = EventCode;
            tp = EventType;
            if (st != "") {
                Body.Content = new TextBlock { Text = st };
                exp = false;
            }
            else {
                Body.Content = new UNote(tp,this);
                exp = true;
            }
        }

        public Event(Int16 EventType, DateTime dt)
        {
            this.InitializeComponent();
            Code = 0;
            tp = EventType;
            Dt = dt;
            Body.Content = new UNote(tp, this);
            exp = true;
        }


        private void UserControl_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (!exp & Code > 0) {
                Body.Content = new UNote(Code, this);
                BD.BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
                exp = true;
            }
        }

        public void Collapse()
        {
            if (Code > 0)
                using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
                {
                    sq.Open();
                    var cmd = sq.CreateCommand();
                    cmd.CommandText = $"Select lt.{lang}_ShortName,l.Comment,l.Descr " +
                        $"From llEvent l join LLEventType lt on l.EventTypeCode = lt.Code Where l.Code={Code}";
                    var rd = cmd.ExecuteReader();
                    rd.Read();
                    Body.Content = new TextBlock { Text = (rd.GetString(0) != "" ? rd.GetString(0) + ": " : "") + rd.GetString(1) + " " + rd.GetString(2) };
                }
            else { Body.Content = null; }
            exp = false;
        }

        private void Border_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            if (!exp) (sender as Border).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void Border_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }
    }
} 
