using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;

namespace Lifes_log.Healths
{
    public sealed partial class HQHead : UserControl
    {
        public Action<int> Set;

        public HQHead(int cd, string tx)
        {
            InitializeComponent();
            BL.Tag = cd.ToString();
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

        private void BL_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Set(int.Parse(((Border)sender).Tag.ToString()!));
        }

    }
}
