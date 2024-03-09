using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Npgsql;
using Windows.Globalization.NumberFormatting;

namespace Lifes_log.LLEvents
{
    public sealed partial class UNum
    {
        private readonly string vkey;
        public override void GetFocus() { Qty.Focus(FocusState.Programmatic); }
        private readonly string lang = (App.Current as App)?.lang;

        public UNum(int code, short ntp)
        {
            InitializeComponent();
            var cmd = (App.Current as App)?.NpDs.CreateCommand($@"
                Select vt.key,COALESCE(lv.dec_value,0),vt.{lang}_name
                From ll_event_type lt
                       join ll_value_type vt on lt.add_values = vt.key
                  left join ll_value lv on lv.value_type_key=vt.key and lv.event_id = {code}
                Where lt.id ={ntp}");
            var rd = cmd.ExecuteReader();
            if (!rd.Read()) return;
            vkey = rd.GetString(0);
            Value.Text = rd.GetString(2);
            Qty.Value = (double)rd.GetDecimal(1);
            Qty.NumberFormatter = new DecimalFormatter
            {
                IntegerDigits = 3,
                FractionDigits = 1,
                NumberRounder = new IncrementNumberRounder { Increment = 0.1 }
            }; 
        }

        public override void InsertBody(NpgsqlCommand cmd, Int32 cd)
        {
            cmd.CommandText = $@"
                Insert into ll_value (event_id,value_type_key,dec_value)
                                   Values({cd},      '{vkey}',{Qty.Text.Replace(",", ".")})";
            cmd.ExecuteNonQuery();
        }

        public override string ToString() { return Qty.Text; }

        private void Qty_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Sf?.Invoke();
            }

        }
    }
}   