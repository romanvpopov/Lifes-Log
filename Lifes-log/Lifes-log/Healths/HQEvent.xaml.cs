using Microsoft.UI.Xaml.Controls;
using System;

namespace Lifes_log.Healths
{
    public sealed partial class HQEvent : UserControl
    {
        private string etp;
        private readonly short tp;

        public HQEvent(short Tp, string HName)
        {
            this.InitializeComponent();
            TX.Text = HName;
            tp = Tp;
            etp = "0";
            GetData(tp);
            DS.ItemsSource = new HQDayList(tp, DateTime.Today, etp, 0);
        }

        private void SetField(int cd)
        {
            DS.ItemsSource = new HQDayList(tp, DateTime.Today, etp, cd);
        }

        private void GetData(short tps)
        {
            var cmd = App.NpDs.CreateCommand($@"
                    Select Distinct le.Code,le.{App.lang}_FieldName
                    From LLFieldEvent le join LLUnit lu on le.UnitCode = lu.Code
                    join LLEvent l on l.EventTypeCode = le.EventTypeCode
                    join LLEventValue lv on lv.EventCode = l.Code and lv.FieldEventCode = le.Code
                    Where le.EventTypeCode = {tps} Order by le.Code");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                etp = etp + "," + rd.GetInt32(0).ToString();
                HD.Children.Add(new HQHead(rd.GetInt32(0), rd.GetString(1)) { Set = SetField });
            }
        }
    }
}
