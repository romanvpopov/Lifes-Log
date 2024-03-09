using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;


namespace Lifes_log.LLEvents
{
    public sealed partial class IntervalPicker : UserControl
    {
        public Action Sf;
        public IntervalPicker(TimeSpan ts)
        {
            this.InitializeComponent();
            VHour.Value = ts.Hours;
            VMinute.Value = ts.Minutes;
            VSecond.Value = ts.Seconds;
        }

        private void VHour_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { VMinute.Focus(FocusState.Programmatic); }
        }

        private void VMinute_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { VSecond.Focus(FocusState.Programmatic); }
        }

        private void VSecond_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf?.Invoke();}
        }
    }
}
