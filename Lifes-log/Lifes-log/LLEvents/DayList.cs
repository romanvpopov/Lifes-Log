using System;
using System.Collections.ObjectModel;

namespace Lifes_log.LLEvents
{
    public class DayList: ObservableCollection<Day>
    {
        public DateTime dt;
        public String etps = "0";
        private readonly int count = 1000;

        public bool HasMoreItems { get; set; }

        public DayList(DateTime Dt)
        {
            HasMoreItems = true;
            dt = Dt;
            etps = "0";
            for (int i = 0; i > -count; i--) Add(new Day(dt.AddDays(i), etps));
            dt = dt.AddDays(-count);
        }

    }
}
