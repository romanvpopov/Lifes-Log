using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Lifes_log;

namespace WinUI3.LLEvents
{
    public sealed partial class NewEvent : UserControl
    {
        private Boolean exp;
        private readonly string lang = (App.Current as App).lang;
        private Day pd;

        public NewEvent(Day Pd)
        {
            this.InitializeComponent();
            pd = Pd;
            Body.Content = new TextBlock { Text = "..." };
        }

        private void UserControl_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (!exp)
            {
                Body.Content = new NewEventBody(this, pd);
                exp = true;
            }
        }

        public void Collapse()
        {
            Body.Content = new TextBlock { Text = "..." };
            exp = false;
        }

        private void Border_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!exp) (sender as Border).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void Border_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }
    }
}
