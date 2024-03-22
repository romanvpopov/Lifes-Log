using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.WinUI.Collections;

namespace Lifes_log.Healths
{
    public class HqDayList(short tps, DateTime dts, string values, string keys)
        : IIncrementalSource<HqDay>
    {
        public async Task<IEnumerable<HqDay>> 
            GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            List<HqDay> items = [];
            var cmd = App.NpDs.CreateCommand($@"
                    Select l.id,l.event_time,l.comment
                    From ll_event l
                    Where l.event_type_id = {tps} and l.event_time<='{dts:yyyyMMdd}'
                    Order by l.event_time Desc
                    LIMIT {pageSize}");
            var rd = cmd.ExecuteReader();
                //while (await rd.ReadAsync(cancellationToken))
                while (await rd.ReadAsync(cancellationToken))
                {
                    items.Add(new HqDay(rd.GetInt16(0), rd.GetDateTime(1),
                        keys, rd.GetString(2),values));
                    dts = rd.GetDateTime(1).AddDays(-1);
                }
            await rd.CloseAsync();
            return items;
        }
    }
}
