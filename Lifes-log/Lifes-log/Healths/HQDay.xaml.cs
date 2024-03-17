using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;

namespace Lifes_log.Healths
{
    public sealed partial class HQDay : UserControl
    {
        private String cmt;
        public HQDay(Int32 cd, DateTime dt, String etp, Int32 cdf, String Cmt)
        {
            this.InitializeComponent();
            DT.Text = dt.ToString("d");
            cmt = Cmt;
            if (cmt != "")
            {
                DT.Foreground = new SolidColorBrush { Color = Colors.Blue };
                ToolTipService.SetToolTip(DT, new ToolTip { Content = cmt });
            }
            var cmd = (App.Current as App).NpDs.CreateCommand($@"
                    Select Distinct le.Code,isNull(lv.FieldValue,'')
                    From LLFieldEvent le join LLUnit lu on le.UnitCode = lu.Code 
                    join LLEvent l on l.EventTypeCode = le.EventTypeCode
                    left join LLEventValue lv on lv.EventCode = l.Code and lv.FieldEventCode = le.Code
                    Where l.Code = {cd} and le.Code in ({etp}) Order by le.Code");
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    VS.Children.Add(new HQValue(rd.GetString(1), (rd.GetInt32(0) == cdf)));
                }
            }

        private void BR_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void BR_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }
    }
}
