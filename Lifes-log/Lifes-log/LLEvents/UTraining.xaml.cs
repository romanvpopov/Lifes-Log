using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Npgsql;
using Windows.Globalization.NumberFormatting;

namespace Lifes_log.LLEvents
{
    public sealed partial class UTraining
    {
        private readonly string distUnit, calUnit;

        public UTraining(NpgsqlCommand cmd, int code, short ntp)
        {
            InitializeComponent();
            cmd.CommandText = (code > 0)
                ? $@"
                Select lt.key, lt.{App.lang}_short_name,lv.dec_value,lv.interval_value,lt.{App.lang}_unit_name
                From ll_value lv join ll_value_type lt on lv.value_type_key = lt.key
                Where lv.event_id={code}"
                : $@"
                Select lt.key, lt.{App.lang}_short_name,0 as dec_value,
                       cast('00:00:00' as interval) as interval_value,lt.{App.lang}_unit_name
                From ll_value_type lt
                Where lt.key in ('cal','dist','time')";
            var rd = cmd.ExecuteReader();
            var decF = new DecimalFormatter {
                IntegerDigits = 2,
                FractionDigits = 0,
                IsGrouped = true,
                NumberRounder = new IncrementNumberRounder { Increment = 1 }
            };
            while (rd.Read())
            {
                switch (rd.GetString(0))
                {
                    case "cal":
                        Calories.Text = rd.GetString(1);
                        VCalories.Value = (double)rd.GetDecimal(2);
                        VCalories.NumberFormatter = decF;
                        calUnit = rd.GetString(4);
                        break;
                    case "dist":
                        Distanсe.Text = rd.GetString(1);
                        VDistance.Value = (double)rd.GetDecimal(2);
                        VDistance.NumberFormatter = decF;
                        distUnit = rd.GetString(4);
                        break;
                    case "time":
                        Time.Text = rd.GetString(1);
                        VTime.Ts = rd.GetTimeSpan(3);
                        VTime.Sf = ()=> { VCalories.Focus(FocusState.Programmatic); };
                        break;
                }
            }
        }

        public override void GetFocus()
        {
            VDistance.Focus(FocusState.Programmatic);
        }

        public override void InsertBody(NpgsqlCommand cmd, int cd)
        {
            cmd.CommandText =$@"
                Insert into ll_value (event_id,value_type_key,dec_value)
                                   Values({cd},         'cal',{(double.IsNaN(VCalories.Value) ? 0:VCalories.Value)})";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $@"
                Insert into ll_value (event_id,value_type_key,dec_value)
                                   Values({cd},        'dist',{(double.IsNaN(VDistance.Value) ? 0 : VDistance.Value)})";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $@"
                Insert into ll_value (event_id,value_type_key,interval_value)
                                   Values({cd},        'time','{VTime.Ts}')";
            cmd.ExecuteNonQuery();
        }

        public override string ToString()
        {
            return VDistance.Text + " " + distUnit + ", " + VTime + "," + VCalories.Value + " " + calUnit;
        }

        private void VDistance_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                VTime.Focus(FocusState.Programmatic);
            }
        }

        private void V_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (double.IsNaN(args.NewValue)) { sender.Value = 0; }
        }

        private void VCalories_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Sf?.Invoke();
            }
        }
    }
}