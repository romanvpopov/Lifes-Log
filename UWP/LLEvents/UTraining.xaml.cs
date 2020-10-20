using Microsoft.Data.SqlClient;
using System;
using Windows.UI.Xaml;

namespace LL.LLEvents
{
    public sealed partial class UTraining : EventBody
    {
        private string lang = (App.Current as App).lang;

        public UTraining(SqlCommand cmd, Int32 Code, Int16 ntp)
        {
            InitializeComponent();
            cmd.CommandText = $"Select lf.Code,isNull(lf.{lang}_FieldName,''),isNull(lf.{lang}_FieldSmallName,''),isNull(lv.FieldValue,'')" +
                $"From LLFieldEvent lf left join LLEventValue lv on lf.Code = lv.FieldEventCode and lv.EventCode={Code} " +
                $"Where lf.EventTypeCode ={ntp} Order by lf.{lang}_FieldName";
            var rd = cmd.ExecuteReader();
            while (rd.Read()) {
                switch (rd.GetInt32(0)) {
                    case 9 : Calories.Text = rd.GetString(1); VCalories.Tag = rd.GetString(2); VCalories.Text = rd.GetString(3); break;
                    case 10: Distanсe.Text = rd.GetString(1); VDistance.Tag = rd.GetString(2); VDistance.Text = rd.GetString(3); break;
                    case 11: Time.Text = rd.GetString(1);     VTime.Tag = rd.GetString(2); VTime.Text = rd.GetString(3); break;
                }
            }
        }    

        public override void GetFocus() { VCalories.Focus(FocusState.Programmatic); }

        public override void InsertBody(SqlCommand cmd, Int32 cd)
        {
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},9,'{VCalories.Text}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},10,'{VDistance.Text}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},11,'{VTime.Text}')";
                cmd.ExecuteNonQuery();
        }
        public override string ToString()
        {
            return (VCalories.Tag+" : "+VCalories.Text+" "+VDistance.Tag + " : " + VDistance.Text+" "+VTime.Tag + " : " + VTime.Text);

        }
        private void C_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) { VDistance.Focus(FocusState.Programmatic); }
        }

        private void D_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) { VTime.Focus(FocusState.Programmatic); }
        }

        private void T_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf(); }
        }

    }
}
