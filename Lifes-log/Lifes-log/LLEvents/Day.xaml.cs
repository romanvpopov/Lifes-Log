using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using Lifes_log;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3.LLEvents
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
            TX.Text = dts.ToString("dddd") + " " + dts.ToString("D");
            if ((dts.DayOfWeek == System.DayOfWeek.Sunday | dts.DayOfWeek == System.DayOfWeek.Saturday))
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
                ss = (rd.GetString(2) != "" ? rd.GetString(2) + ": " : "") + rd.GetString(3) + " " + rd.GetString(4);
                var ev = new Event(ss, rd.GetInt32(0), rd.GetInt16(5));
                if (etps != "0")
                {
                    String[] ar = etps.Split(",");
                    if (ar.Contains(ev.tp.ToString()))
                    {
                        ev.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                    }
                    else
                    {
                        ev.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                    }
                }
                EDL.Children.Add(ev);
            }
            rd.Close();
        }

        public void ApllyFilter(String etps)
        {
            String[] ar = etps.Split(",");
            foreach (object et in EDL.Children)
            {
                if (et.GetType().Name == "Event")
                {
                    if (ar.Contains((et as Event).tp.ToString()))
                    {
                        (et as Event).Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                    }
                    else
                    {
                        (et as Event).Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                    }
                }
            }
        }

        public void ResetFilter()
        {
            foreach (object et in EDL.Children)
            {
                if (et.GetType().Name == "Event") (et as Event).Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }

        }

        public void AddEvent(Int16 ntp)
        {
            EDL.Children.Add(new Event(ntp, dt));
        }
    }
}
