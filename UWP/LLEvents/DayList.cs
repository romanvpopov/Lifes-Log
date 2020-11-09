using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LL.LLEvents
{
    public class DayList : ObservableCollection<Day>, ISupportIncrementalLoading
    {
        private DateTime dt;
        public String etps;
        public DayList()
        {
            HasMoreItems = true;
            dt = DateTime.Today;
            etps = "0";
        }

        public bool HasMoreItems { get; }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var dsp = Window.Current.Dispatcher;
            return Task.Run<LoadMoreItemsResult>(
                async () =>
                {
                    await dsp.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () =>  {
                            for (int i = 0; i > -50; i--) Add(new Day(dt.AddDays(i), etps));
                            dt = dt.AddDays(-50);
                        });
                    return new LoadMoreItemsResult() { Count = 50 };

                }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
