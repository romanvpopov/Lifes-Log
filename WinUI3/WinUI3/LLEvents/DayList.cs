using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

namespace WinUI3.LLEvents
{
    public class DayList : ObservableCollection<Day>, ISupportIncrementalLoading
    {
        public DateTime dt;
        public String etps;
        public DayList(DateTime Dt)
        {
            HasMoreItems = true;
            dt = Dt;
            etps = "0";
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
                            for (int i = 0; i > -count; i--) Add(new Day(dt.AddDays(i), etps));
                            dt = dt.AddDays(-count);
                        });
                    return new LoadMoreItemsResult() { Count = count };
                }).AsAsyncOperation();
        }
    }
}
