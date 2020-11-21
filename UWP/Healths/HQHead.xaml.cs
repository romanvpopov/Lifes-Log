using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace LL.Healths
{
    public sealed partial class HQHead : UserControl
    {
        public Action<Int32> Set;

        public HQHead(Int32 cd, String tx)
        {
            this.InitializeComponent();
            BL.Tag = cd.ToString();
            TX.Text = tx;
        }

        private void BL_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void BL_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }

        private void BL_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Set(Int32.Parse((sender as Border).Tag.ToString()));
        }
    }
}
