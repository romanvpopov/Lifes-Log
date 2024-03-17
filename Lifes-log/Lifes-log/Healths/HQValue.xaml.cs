using Microsoft.UI.Xaml.Controls;
using System;

namespace Lifes_log.Healths
{
    public sealed partial class HQValue : UserControl
    {
        public HQValue(String tx, Boolean bg)
        {
            InitializeComponent();
            TX.Text = tx;
            if (!bg) BL.Background = null;
        }
    }
}
