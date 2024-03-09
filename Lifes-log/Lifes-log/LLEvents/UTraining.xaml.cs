using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Npgsql;
using Windows.Globalization.NumberFormatting;

namespace Lifes_log.LLEvents
{
    public sealed partial class UTraining 
    {
        private readonly string lang = (App.Current as App)?.lang;

        public UTraining(NpgsqlCommand cmd, int code, short ntp)
        {
            InitializeComponent();
            cmd.CommandText = (code>0)?$@"
                Select lt.key, lt.{lang}_name,lv.dec_value,lv.interval_value
                From ll_value lv join ll_value_type lt on lv.value_type_key = lt.key
                Where lv.event_id={code}":$@"
                Select lt.key, lt.ru_name,0 as dec_value,0 as interval_value
                From ll_value_type lt
                Where lt.key in ('cal','dist','time')";
            var rd = cmd.ExecuteReader();
            while (rd.Read()) {
                switch (rd.GetString(0)) {
                    case "cal":
                        Calories.Text = rd.GetString(1);
                        VCalories.Value = (double)rd.GetDecimal(2);
                        VCalories.NumberFormatter = new DecimalFormatter {
                            IntegerDigits = 4,
                            FractionDigits = 0,
                        }; break;
                    case "dist":
                        Distanсe.Text = rd.GetString(1);
                        VDistance.Value = (double)rd.GetDecimal(2);
                        VDistance.NumberFormatter = new DecimalFormatter {
                            IntegerDigits = 5,
                            FractionDigits = 0,
                        }; break;
                    case "time":
                        Time.Text = rd.GetString(1);
                        VTime.Time = rd.GetTimeSpan(3); break;
                }
            }
        }

        public override void GetFocus() { VCalories.Focus(FocusState.Programmatic); }

        public override void InsertBody(NpgsqlCommand cmd, int cd)
        {
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{VCalories.Tag},'{VCalories.Text}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{VDistance.Tag},'{VDistance.Text}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{VTime.Tag},'{VTime.Time}')";
                cmd.ExecuteNonQuery();
        }
        public override string ToString()
        {
            return (VCalories.Text+" "+Calories.Tag+", " +VDistance.Text + " " + Distanсe.Tag+", "+VTime.Time + " " + Time.Tag);

        }

        private void VTime_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf(); }
        }

        private void VDistance_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { VTime.Focus(FocusState.Programmatic); }
        }

        private void VCalories_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { VDistance.Focus(FocusState.Programmatic); }
        }
    }
}
