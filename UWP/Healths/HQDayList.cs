using System.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LL.Healths
{
    public class HQDayList : ObservableCollection<HQDay>, ISupportIncrementalLoading
    {
        public DateTime dt;
        private readonly Int16 tp;
        private readonly Int32 cd;
        private readonly String etp;
 
        public HQDayList(Int16 Tp, DateTime Dt, String Etp, Int32 Cd)
        {
            HasMoreItems = true;
            tp = Tp;
            etp = Etp;
            cd = Cd;
            dt = Dt;
        }

        public bool HasMoreItems { get; set; }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var dsp = Window.Current.Dispatcher;
            return Task.Run(
                async () => {
                    await dsp.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () => {
                            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr)) {
                                sq.Open();
                                var cmd = sq.CreateCommand();
                                cmd.CommandText = $@"
                                    Select Distinct Top {count} l.Code,l.DateEvent,l.Descr
                                    From LLFieldEvent le join LLUnit lu on le.UnitCode = lu.Code
                                    join LLEvent l on l.EventTypeCode = le.EventTypeCode
                                    join LLEventValue lv on lv.EventCode = l.Code and lv.FieldEventCode = le.Code
                                    Where l.EventTypeCode = {tp} and  l.DateEvent<='{dt:yyyyMMdd}'
                                    Order by l.DateEvent Desc";
                                var rd = cmd.ExecuteReader();
                                if (rd.HasRows) {
                                    while (rd.Read()) {
                                        Add(new HQDay(rd.GetInt32(0), rd.GetDateTime(1), etp, cd, rd.GetString(2)));
                                        dt = rd.GetDateTime(1).AddDays(-1);
                                    }                                    
                                }
                                else { HasMoreItems = false; }
                            }
                        });
                    return new LoadMoreItemsResult() { Count = count };
                }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
