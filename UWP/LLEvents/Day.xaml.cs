using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LL.LLEvents
{
    public sealed partial class Day : UserControl
    {
        private readonly string lang = (App.Current as App).lang;
        private readonly String ss;
        public DateTime dt;

        public Day(DateTime dts, String etps)
        {
            InitializeComponent();
            dt = dts;
            TX.Text = dts.ToString("dddd")+" "+dts.ToString("D");
            if ((dts.DayOfWeek == System.DayOfWeek.Sunday | dts.DayOfWeek == System.DayOfWeek.Saturday)) {
                BL.Background = new SolidColorBrush(Colors.Red);
                BL.Width = 4;
            } else {
                BL.Background = new SolidColorBrush(Colors.Blue); 
            }
            if (dts.Date == DateTime.Today) {
                BL.Background = new SolidColorBrush(Colors.Green);
                BL.Width = 4;
            }
            EDL.Children.Add(new NewEvent(this));
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                var tps = (etps == "0") ? "" : $" and l.EventTypeCode in ({etps})";
                cmd.CommandText =
                   $"Select L.Code,lt.ClassName,lt.{lang}_ShortName,l.Comment,l.Descr,l.EventTypeCode " +
                   $"From llEvent l join LLEventType lt on l.EventTypeCode = lt.Code " +
                   $"Where l.DateEvent='{dts.ToString("yyyyMMdd")}' Order by l.Code";
                var rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    ss = (rd.GetString(2) != "" ? rd.GetString(2) + ": " : "") + rd.GetString(3) + " " + rd.GetString(4);
                    var ev = new Event(ss, rd.GetInt32(0), rd.GetInt16(5));
                    if (etps != "0") {
                        String[] ar = etps.Split(",");
                        if (ar.Contains(ev.tp.ToString())) {
                            ev.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        } else {
                            ev.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }
                    EDL.Children.Add(ev);
                }
                rd.Close();
            }
        }

        public void ApllyFilter(String etps) 
        {
            String[] ar = etps.Split(",");
            foreach (object et in EDL.Children) {
                if (et.GetType().Name == "Event") {
                    if (ar.Contains((et as Event).tp.ToString())) {
                        (et as Event).Visibility = Windows.UI.Xaml.Visibility.Visible;
                    } else {
                        (et as Event).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                }
            }
        }

        public void ResetFilter()
        {
            foreach (object et in EDL.Children) {
                if (et.GetType().Name == "Event") (et as Event).Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }

        public void AddEvent(Int16 ntp) 
        {
            EDL.Children.Add(new Event(ntp,dt));
        }
    }
}
