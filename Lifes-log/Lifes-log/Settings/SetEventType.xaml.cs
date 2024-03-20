using Microsoft.UI.Xaml.Controls;

namespace Lifes_log.DBSettings
{
    public sealed partial class SetEventType : Page
    {
        public SetEventType()
        {
            InitializeComponent();
            U1.Content = new SetShm("");
            U2.Content = new SetShm("S");
            U3.Content = new SetShm("H");
        }
    }
}
