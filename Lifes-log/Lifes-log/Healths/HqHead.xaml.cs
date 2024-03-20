using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;

namespace Lifes_log.Healths
{
    public sealed partial class HqHead : UserControl
    {
        public Action<string> Set;

        public HqHead(string key, string tx)
        {
            InitializeComponent();
            BL.Tag = key;
            TX.Text = tx;
        }

        private void BL_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ((Border)sender).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void BL_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ((Border)sender).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }

        private void BL_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Set(((Border)sender).Tag.ToString());
        }
    }
}
