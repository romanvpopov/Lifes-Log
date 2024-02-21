using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LL.LLEvents
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

        private void UserControl_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (exp) return;
            Body.Content = new NewEventBody(this, pd);
            exp = true;
        }

        public void Collapse() {
            Body.Content = new TextBlock { Text = "..." };
            exp = false;
        }

        private void Border_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            if (!exp) (sender as Border).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void Border_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }
    }
} 
