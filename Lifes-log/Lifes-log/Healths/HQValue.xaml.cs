using Microsoft.UI.Xaml.Controls;

namespace Lifes_log.Healths
{
    public sealed partial class HQValue : UserControl
    {
        public HQValue(string tx, bool bg)
        {
            InitializeComponent();
            TX.Text = tx;
            if (!bg) BL.Background = null;
        }
    }
}
