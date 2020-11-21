using Microsoft.Data.SqlClient;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace LL.Healths
{
    public sealed partial class HQDay : UserControl
    {
        public HQDay(Int32 cd, DateTime dt, String etp, Int32 cdf)
        {
            this.InitializeComponent();
            DT.Text = dt.ToString("d");
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText = $"Select Distinct le.Code,isNull(lv.FieldValue,'') " +
                    $"From LLFieldEvent le join LLUnit lu on le.UnitCode = lu.Code " +
                    $"join LLEvent l on l.EventTypeCode = le.EventTypeCode " +
                    $"left join LLEventValue lv on lv.EventCode = l.Code and lv.FieldEventCode = le.Code " +
                    $"Where l.Code = {cd} and le.Code in ({etp}) Order by le.Code";
                var rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    VS.Children.Add(new HQValue(rd.GetString(1), (rd.GetInt32(0)==cdf)));
                }
            }
        }

        private void Border_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ContentDialogBorderThemeBrush"];
        }

        private void Border_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (sender as Border).BorderBrush = (SolidColorBrush)Resources["ButtonBorderThemeBrush"];
        }
    }
}
