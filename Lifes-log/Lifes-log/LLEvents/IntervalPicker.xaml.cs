using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Globalization.NumberFormatting;


namespace Lifes_log.LLEvents
{
    public sealed partial class IntervalPicker : UserControl
    {
        public Action Sf;

        public TimeSpan Ts
        {
            set
            {
                VHour.Value = value.Hours;
                VMinute.Value = value.Minutes;
                VSecond.Value = value.Seconds;
            }
            get => TimeSpan.Parse(VHour.Text + ":" + VMinute.Text + ":" + VSecond.Text);
        }

        public IntervalPicker()
        {
            this.InitializeComponent();
            VHour.NumberFormatter = new DecimalFormatter
            {
                IntegerDigits = 1, FractionDigits = 0
            };
            VHour.Maximum = 99;
            VMinute.NumberFormatter = new DecimalFormatter
            {
                IntegerDigits = 1, FractionDigits = 0
            };
            VMinute.Maximum = 60;
            VSecond.NumberFormatter = new DecimalFormatter
            {
                IntegerDigits = 1, FractionDigits = 0
            };
            VSecond.Maximum = 60;
        }

        public override string ToString()
        {
            return (VHour.Value > 0 ? VHour.Text + " " + THour.Text + " " : "") +
                   (VMinute.Value > 0 ? VMinute.Text + " " + TMinute.Text + " " : "") +
                   (VSecond.Value > 0 ? VSecond.Text + " " + TSecond.Text + " " : "");
        }

        public void GetFocus()
        {
            VMinute.Focus(FocusState.Programmatic);
        }

        private void VHour_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                VMinute.Focus(FocusState.Programmatic);
            }
        }

        private void VMinute_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                VSecond.Focus(FocusState.Programmatic);
            }
        }

        private void VSecond_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Sf?.Invoke();
            }
        }
    }
}