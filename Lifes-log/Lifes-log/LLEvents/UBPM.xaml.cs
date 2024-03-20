using System;
using Windows.Globalization.NumberFormatting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Npgsql;
using Microsoft.UI.Xaml.Controls;

namespace Lifes_log.LLEvents
{
    public sealed partial class UBPM
    {

        public UBPM(NpgsqlCommand cmd, int code)
        {
            InitializeComponent();
            cmd.CommandText = (code > 0)
                ? $@"
                Select lt.key, lt.{App.lang}_name,lv.dec_value
                From ll_value lv join ll_value_type lt on lv.value_type_key = lt.key
                Where lv.event_id={code}"
                : $@"
                Select lt.key, lt.{App.lang}_name,0 as dec_value
                From ll_value_type lt
                Where lt.key in ('sys','dia','pulse')";
            var decF = new DecimalFormatter {
                IntegerDigits = 1, FractionDigits = 0,
                NumberRounder = new IncrementNumberRounder { Increment = 1 }
            };
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                switch (rd.GetString(0))
                {
                    case "sys":
                        MS.Header = rd.GetString(1);
                        MS.Value = (double)rd.GetDecimal(2);
                        MS.NumberFormatter = decF;
                        MS.Maximum = 300;
                        break;
                    case "dia":
                        MD.Header = rd.GetString(1);
                        MD.Value = (double)rd.GetDecimal(2);
                        MD.NumberFormatter = decF;
                        MD.Maximum = 300;
                        break;
                    case "pulse":
                        MP.Header = rd.GetString(1);
                        MP.Value = (double)rd.GetDecimal(2);
                        MP.NumberFormatter = decF;
                        MP.Maximum = 300;
                        break;
                }
            }
            rd.Close();
        }

        public override void GetFocus()
        {
            MS.Focus(FocusState.Programmatic);
        }

        public override void InsertBody(NpgsqlCommand cmd, Int32 cd)
        {
            cmd.CommandText = $@"
                Insert into ll_value (event_id,value_type_key,dec_value)
                                   Values({cd},         'sys',{MS.Text})";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $@"
                Insert into ll_value (event_id,value_type_key,dec_value)
                                   Values({cd},        'dia',{MD.Text})";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $@"
                Insert into ll_value (event_id,value_type_key,dec_value)
                                   Values({cd},       'pulse','{MP.Text}')";
            cmd.ExecuteNonQuery();
        }

        public override string ToString()
        {
            return MS.Text + "/" + MD.Text + " " + MP.Header + ":" + MP.Text;
        }

        private void MS_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                MD.Focus(FocusState.Programmatic);
            }
        }

        private void MD_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                MP.Focus(FocusState.Programmatic);
            }
        }

        private void MP_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Sf();
            }
        }

        private void V_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (double.IsNaN(args.NewValue)) { sender.Value = 0; }
        }
    }
}