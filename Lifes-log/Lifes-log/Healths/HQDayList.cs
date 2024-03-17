using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

namespace Lifes_log.Healths
{
    public class HQDayList : ObservableCollection<HQDay>//, ISupportIncrementalLoading
    {
        public DateTime dt;
        private readonly short tp;
        private readonly int cd;
        private readonly string etp;

        public HQDayList(Int16 Tp, DateTime Dt, String Etp, Int32 Cd)
        {
            HasMoreItems = true;
            tp = Tp;
            etp = Etp;
            cd = Cd;
            dt = Dt;
        }

        public bool HasMoreItems { get; set; }

        /*public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var dsp = Window.Current.Dispatcher;
            return Task.Run(
                async () => {
                    await dsp.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () => {
                            var cmd = App.NpDs.CreateCommand($@"
                                    Select Distinct Top {count} l.Code,l.DateEvent,l.Descr
                                    From LLFieldEvent le join LLUnit lu on le.UnitCode = lu.Code
                                    join LLEvent l on l.EventTypeCode = le.EventTypeCode
                                    join LLEventValue lv on lv.EventCode = l.Code and lv.FieldEventCode = le.Code
                                    Where l.EventTypeCode = {tp} and  l.DateEvent<='{dt:yyyyMMdd}'
                                    Order by l.DateEvent Desc");
                                var rd = cmd.ExecuteReader();
                                if (rd.HasRows)
                                {
                                    while (rd.Read())
                                    {
                                        Add(new HQDay(rd.GetInt32(0), rd.GetDateTime(1), etp, cd, rd.GetString(2)));
                                        dt = rd.GetDateTime(1).AddDays(-1);
                                    }
                                }
                                else { HasMoreItems = false; }
                            }
                        });
                    return new LoadMoreItemsResult() { Count = count };
                }).AsAsyncOperation<LoadMoreItemsResult>();
        }*/
    }
}
