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
            get => TimeSpan.Parse(VHour.Value + ":" + VMinute.Value + ":" + VSecond.Value);
        }

        public IntervalPicker()
        {
            InitializeComponent();
            var decF = new DecimalFormatter { 
                IntegerDigits = 1, FractionDigits = 0,
                NumberRounder = new IncrementNumberRounder { Increment = 1 }
            };
            VHour.NumberFormatter = decF;
            VHour.Maximum = 99;
            VMinute.NumberFormatter = decF;
            VMinute.Maximum = 60;
            VSecond.NumberFormatter = decF;
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

        private void V_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (double.IsNaN(args.NewValue)) { sender.Value = 0; }
        }
    }
}