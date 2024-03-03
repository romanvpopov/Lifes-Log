using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;

namespace Lifes_log.LLEvents
{
    public sealed partial class Day
    {
        private readonly string lang = (App.Current as App).lang;
        public DateTime dt;

        public Day(DateTime dts, string etps)
        {
            InitializeComponent();
            dt = dts;
            TX.Text = dts.ToString("dddd") + " " + dts.ToString("D");
            if ((dts.DayOfWeek == DayOfWeek.Sunday | dts.DayOfWeek == DayOfWeek.Saturday))
            {
                BL.Background = new SolidColorBrush(Colors.Red);
                BL.Width = 4;
            }
            else
            {
                BL.Background = new SolidColorBrush(Colors.Blue);
                BL.Width = 3;
            }
            if (dts.Date == DateTime.Today)
            {
                BL.Background = new SolidColorBrush(Colors.Green);
                BL.Width = 4;
            }
            EDL.Children.Add(new NewEvent(this));
            //var tps = (etps == "0") ? "" : $" and l.EventTypeCode in ({etps})";
            var cmd = (App.Current as App).NpDs.CreateCommand(
                   $@"Select l.id,lt.class_name,lt.{lang}_short_name,l.comment,l.description,l.event_type_id
                      From ll_event l join ll_event_type lt on l.event_type_id = lt.id
                      Where l.event_time='{dts:yyyyMMdd}' Order by l.id");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                var ss = (rd.GetString(2) != "" ? rd.GetString(2) + ": " : "") + 
                         rd.GetString(3) + " " + rd.GetString(4);
                var ev = new Event(ss, rd.GetInt32(0), rd.GetInt16(5));
                if (etps != "0")
                {
                    var ar = etps.Split(",");
                    ev.Visibility = ar.Contains(ev.tp.ToString()) ?
                        Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
                }
                EDL.Children.Add(ev);
            }
            rd.Close();
        }

        public void ApllyFilter(string etps)
        {
            var ar = etps.Split(",");
            foreach (var et in EDL.Children)
            {
                if (et.GetType().Name != "Event") continue;
                ((Event)et).Visibility = ar.Contains((et as Event).tp.ToString()) ? 
                    Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
            }
        }

        public void ResetFilter()
        {
            foreach (var et in EDL.Children)
            {
                if (et.GetType().Name == "Event") ((Event)et).Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }

        }

        public void AddEvent(Int16 ntp)
        {
            EDL.Children.Add(new Event(ntp, dt));
        }
    }
}
