using System;
using System.Collections.ObjectModel;

namespace Lifes_log.LLEvents
{
    public class DayList: ObservableCollection<Day>
    {
        public DateTime dt;
        public string etps;
        private new const int Count = 1000;

        public bool HasMoreItems { get; set; }

        public DayList(DateTime dts)
        {
            HasMoreItems = true;
            dt = dts;
            etps = "0";
            for (var i = 0; i > -Count; i--) Add(new Day(this.dt.AddDays(i), etps));
            this.dt = this.dt.AddDays(-Count);
        }

    }
}
