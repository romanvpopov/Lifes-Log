using Microsoft.UI.Xaml.Controls;
using System;
using CommunityToolkit.WinUI.Collections;

namespace Lifes_log.Healths
{
    public sealed partial class HqEvent : UserControl
    {
        private readonly short tp;
        private readonly string val;

        public HqEvent(short tps, string hName)
        {
            InitializeComponent();
            TX.Text = hName;
            tp = tps;
            val="''";
            var cmd = App.NpDs.CreateCommand($@"
                Select Distinct vt.key,vt.{App.lang}_name
                From  ll_value_type vt join ll_value lv on vt.key =lv.value_type_key
                    join ll_event l on lv.event_id=l.id
                Where l.event_type_id = {tps}
                Order by vt.{App.lang}_name");
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                HD.Children.Add(new HqHead(rd.GetString(0), rd.GetString(1)) { Set = SetField });
                val=val+",'"+rd.GetString(0)+"'";
            }
            rd.Close();
            DS.ItemsSource = new IncrementalLoadingCollection<HqDayList, HqDay>
                (new HqDayList(tp, DateTime.Today, val, ""));
        }
        private void SetField(string key)
        {
            DS.ItemsSource = new IncrementalLoadingCollection<HqDayList, HqDay>
                (new HqDayList(tp, DateTime.Today, val, key));
        }

    }
}
