using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LL.Healths
{
    public sealed partial class HQEvent : UserControl
    {
        public HQEvent(SqlConnection sq, Int16 tp, String HName)
        {
            this.InitializeComponent();
            TX.Text = HName;
        }
    }
}
