using System;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LL.LLEvents
{
    public sealed partial class Day : UserControl
    {
        private readonly string lang = (App.Current as App).lang;
        private String ss;
        public DateTime dt;

        public Day(SqlConnection sq, DateTime dts)
        {
            this.InitializeComponent();
            dt = dts;
            TX.Text = dts.ToString("dddd")+" "+dts.ToString("d");
            if ((dts.DayOfWeek == System.DayOfWeek.Sunday | dts.DayOfWeek == System.DayOfWeek.Saturday))
            {
                BL.Background = new SolidColorBrush(Colors.Red);
            } else {
                BL.Background = new SolidColorBrush(Colors.Blue); 
            }
            EDL.Children.Add(new NewEvent(this));
            var cmd = sq.CreateCommand();
            cmd.CommandText =
               $"Select L.Code,lt.ClassName,lt.{lang}_ShortName,l.Comment,l.Descr,l.EventTypeCode " +
               $"From llEvent l join LLEventType lt on l.EventTypeCode = lt.Code " +
               $"Where l.DateEvent='{dts.ToString("yyyyMMdd")}' Order by l.Code";
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ss = (rd.GetString(2) != "" ? rd.GetString(2) + ": " : "") + rd.GetString(3) + " " + rd.GetString(4);
                EDL.Children.Add(new Event(ss, rd.GetInt32(0), 0));
            }
            rd.Close();
        }

        public void AddEvent(Int16 ntp) 
        {
            EDL.Children.Add(new Event(ntp,dt));
        }
    }
}
