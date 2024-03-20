using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;

namespace Lifes_log.Healths
{
    public sealed partial class HqDay : UserControl
    {
        public HqDay(int cd, DateTime dt, string key, string cmt, string values)
        {
            InitializeComponent();
            DT.Text = dt.ToString("d");
            if (cmt != "")
            {
                DT.Foreground = new SolidColorBrush { Color = Colors.Blue };
                ToolTipService.SetToolTip(DT, new ToolTip { Content = cmt });
            }
            var cmd = App.NpDs.CreateCommand($@"
                    Select vt.key,lv.dec_value
                    From ll_value_type vt left join ll_value lv on
                        vt.key = lv.value_type_key and lv.event_id ={cd}
                    Where vt.key in ({values})
                    Order by vt.{App.lang}_name");
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    VS.Children.Add(new HQValue(rd.IsDBNull(1) ? "" : rd.GetDecimal(1).ToString(),
                        (rd.GetString(0) == key)));
                }
                rd.Close();
        }

        private void BR_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ((Border)sender).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void BR_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ((Border)sender).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }
    }

}
