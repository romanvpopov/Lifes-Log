using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.WinUI.Collections;

namespace Lifes_log.LLEvents
{
    public class DayList(DateTime dts) : IIncrementalSource<Day>
    {
        public DateTime dt = dts.AddDays(1);
        public string etps = "0";

        public async Task<IEnumerable<Day>> 
            GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            List<Day> items = [];

            for (var i = 1; i <= pageSize; i++)
            {
                items.Add(new Day(dt.AddDays(-(pageIndex*pageSize + i)), etps));
            }
            await Task.Delay(1, cancellationToken);
            return items;
        }
    }
}
