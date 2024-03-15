using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.WinUI.Collections;

namespace Lifes_log.LLEvents
{
    public class DayL : IncrementalLoadingCollection<DayList, Day>
    {
        public DateTime Dt { set { dls.dt = value.AddDays(1); } }
        public string Etps { set { dls.etps = value; } }
        private readonly DayList dls;

        public DayL(DayList source) : base(source)
        {
            dls = source;
        }

    }

    public class DayList : IIncrementalSource<Day>
    {
        public DateTime dt;
        public string etps;

        public DayList(DateTime dts)
        {
            dt = dts.AddDays(1);
            etps = "0";
        }

        public async Task<IEnumerable<Day>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            List<Day> items = new();

            for (var i = 1; i <= pageSize; i++)
            {
                items.Add(new Day(dt.AddDays(-(pageIndex*pageSize + i)), etps));
            }
            await Task.Delay(1, cancellationToken);
            return items;
        }
    }
}
