using Microsoft.UI.Xaml.Controls;
using System;

namespace Lifes_log.Healths
{
    public sealed partial class HQEvent : UserControl
    {
        private String etp;
        private Int16 tp;
        private readonly string lang = (App.Current as App).lang;

        public HQEvent(Int16 Tp, String HName)
        {
            this.InitializeComponent();
            TX.Text = HName;
            tp = Tp;
            etp = "0";
            GetData(tp);
            DS.ItemsSource = new HQDayList(tp, DateTime.Today, etp, 0);
        }

        private void SetField(Int32 cd)
        {
            DS.ItemsSource = new HQDayList(tp, DateTime.Today, etp, cd);
        }

        private void GetData(Int16 tp)
        {
            var cmd = (App.Current as App).NpDs.CreateCommand($@"
                    Select Distinct le.Code,le.{lang}_FieldName
                    From LLFieldEvent le join LLUnit lu on le.UnitCode = lu.Code
                    join LLEvent l on l.EventTypeCode = le.EventTypeCode
                    join LLEventValue lv on lv.EventCode = l.Code and lv.FieldEventCode = le.Code
                    Where le.EventTypeCode = {tp} Order by le.Code");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                etp = etp + "," + rd.GetInt32(0).ToString();
                HD.Children.Add(new HQHead(rd.GetInt32(0), rd.GetString(1)) { Set = SetField });
            }
        }
    }
}
