using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace LL.LLEvents
{
    public sealed partial class More : UserControl
    {
        public DateTime dt;
        public Action<DateTime> Up;
        public More(DateTime Dt)
        {
            dt = Dt;
            this.InitializeComponent();
        }

        private void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            dt = dt.Add(TimeSpan.FromDays(-(App.Current as App).lenpage));
            Up(dt);
        }
    }
}
