using Windows.Globalization.NumberFormatting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Npgsql;
using Microsoft.UI.Xaml.Controls;

namespace Lifes_log.LLEvents
{
    public sealed partial class UExercise
    {
        private readonly string calUnit;

        public UExercise(NpgsqlCommand cmd, int code)
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
                Where lt.key in ('cal','time')";
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                switch (rd.GetString(0))
                {
                    case "cal":
                        Calories.Text = rd.GetString(1);
                        VCalories.Value = (double)rd.GetDecimal(2);
                        VCalories.NumberFormatter = new DecimalFormatter {
                            IntegerDigits = 1, FractionDigits = 0,
                            NumberRounder = new IncrementNumberRounder { Increment = 1 }
                        };
                        calUnit = rd.GetString(4);
                        break;
                    case "time":
                        Time.Text = rd.GetString(1);
                        VTime.Ts = rd.GetTimeSpan(3);
                        VTime.Sf = ()=> { VCalories.Focus(FocusState.Programmatic); };
                        break;
                }
            }
        }

        public override void GetFocus() { VTime.GetFocus(); }

        public override void InsertBody(NpgsqlCommand cmd, int cd)
        {
            cmd.CommandText = $@"
                Insert into ll_value (event_id,value_type_key,dec_value)
                                   Values({cd},         'cal',{VCalories.Text})";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $@"
                Insert into ll_value (event_id,value_type_key,interval_value)
                                   Values({cd},        'time','{VTime.Ts}')";
            cmd.ExecuteNonQuery();
        }

        public override string ToString()
        {
            return (VTime + "," + VCalories.Text + " " + calUnit);

        }

        private void VCalories_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Sf?.Invoke();
            }
        }

        private void VCalories_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (double.IsNaN(args.NewValue)) { sender.Value = 0; }
        }
    }
}