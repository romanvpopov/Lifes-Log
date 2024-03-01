using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;

namespace Lifes_log.LLEvents
{
    public sealed partial class Event 
    {
        private bool exp;
        public int Code;
        public DateTime Dt;
        public readonly short tp;
        private readonly string lang = (App.Current as App)?.lang;

        public Event(string st, int eventCode, short eventType)
        {
            InitializeComponent();
            Code = eventCode;
            tp = eventType;
            if (st != "")
            {
                Body.Content = new TextBlock { Text = st, TextWrapping = TextWrapping.Wrap };
                exp = false;
            }
            else
            {
                Body.Content = new UNote(tp, this);
                exp = true;
            }
        }

        public Event(short eventType, DateTime dt)
        {
            InitializeComponent();
            Code = 0;
            tp = eventType;
            Dt = dt;
            Body.Content = new UNote(tp, this);
            exp = true;
            BD.Background = (SolidColorBrush)Resources["TextBoxBackgroundThemeBrush"];
        }


        private void UserControl_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (!exp & Code > 0)
            {
                Body.Content = new UNote(Code, this);
                BD.BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
                //BD.Background = (SolidColorBrush)Resources["TextBoxBackgroundThemeBrush"];
                exp = true;
            }
        }

        public void Collapse()
        {
            if (Code > 0)
            {
                var cmd = (App.Current as App).NpDs.CreateCommand(
                  $@"Select lt.{lang}_ShortName,l.Comment,l.Descr
                   From llEvent l join LLEventType lt on l.EventTypeCode = lt.Code Where l.Code={Code}");
                var rd = cmd.ExecuteReader();
                rd.Read();
                Body.Content = new TextBlock
                {
                    Text = (rd.GetString(0) != "" ? rd.GetString(0) + ": " : "") + rd.GetString(1) + " " + rd.GetString(2),
                    TextWrapping = TextWrapping.Wrap
                };
            }
            else Visibility = Visibility.Collapsed;
            exp = false;
            //BD.Background = null;
            BD.BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void Border_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //if (!exp) 
                (sender as Border).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void Border_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }
    }
}
