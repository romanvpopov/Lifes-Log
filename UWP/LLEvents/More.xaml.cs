using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace LL.LLEvents
{
    public sealed partial class More : UserControl
    {
        private DateTime dt;
        private Int16 lp;
        public Action<DateTime,More> Up;
        public Action<DateTime,More> Down;
        public Boolean PB { set { PR.IsActive = value; } }

        public More(DateTime Dt, Int16 Lp)
        {
            dt = Dt; lp = Lp;
            this.InitializeComponent();
            TX.Text = dt.ToString("d");
        }

        private void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Up != null) {
                PR.IsActive = true;
                Up(dt, this);
                dt = new DateTime(dt.Year-1,12,31);
                TX.Text = dt.ToString("d");
            }
            if (Down != null) {
                PR.IsActive = true;
                Down(dt,this);
                dt = new DateTime(dt.Year+1, 1, 1);
                TX.Text = dt.ToString("d");
            } 
        }
    }
}
