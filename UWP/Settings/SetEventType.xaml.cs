using Windows.UI.Xaml.Controls;

namespace LL.Settings
{
    public sealed partial class SetEventType
    {
        public SetEventType()
        {
            InitializeComponent();
            U1.Content = new SetSHM("");
            U2.Content = new SetSHM("S");
            U3.Content = new SetSHM("H");
        }
    }
}
