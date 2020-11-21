using System;
using Windows.UI.Xaml.Controls;

namespace LL.Healths
{
    public sealed partial class HQValue : UserControl
    {
        public HQValue(String tx, Boolean bg)
        {
            this.InitializeComponent();
            TX.Text = tx;
            if (!bg) BL.Background = null;
        }
    }
}
