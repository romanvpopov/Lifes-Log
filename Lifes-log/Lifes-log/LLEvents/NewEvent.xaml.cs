using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Lifes_log.LLEvents
{
    public sealed partial class NewEvent
    {
        private bool exp;
        private readonly Day pd;

        public NewEvent(Day pds)
        {
            InitializeComponent();
            pd = pds;
            Body.Content = new TextBlock { Text = "..." };
        }

        private void UserControl_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (exp) return;
            Body.Content = new NewEventBody(this, pd);
            exp = true;
        }

        public void Collapse()
        {
            Body.Content = new TextBlock { Text = "..." };
            exp = false;
        }

        private void Border_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!exp) ((Border)sender).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void Border_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((Border)sender).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }
    }
}
