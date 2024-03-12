﻿using System;
using Windows.Globalization.NumberFormatting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Npgsql;

namespace Lifes_log.LLEvents
{
    public sealed partial class UBPM
    {
        private readonly string lang = (App.Current as App)?.lang;

        public UBPM(NpgsqlCommand cmd, int code)
        {
            InitializeComponent();
            cmd.CommandText = (code > 0)
                ? $@"
                Select lt.key, lt.{lang}_name,lv.dec_value
                From ll_value lv join ll_value_type lt on lv.value_type_key = lt.key
                Where lv.event_id={code}"
                : $@"
                Select lt.key, lt.{lang}_name,0 as dec_value
                From ll_value_type lt
                Where lt.key in ('sys','dia','pulse')";
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                switch (rd.GetString(0))
                {
                    case "sys":
                        MST.Text = rd.GetString(1);
                        MS.Value = (double)rd.GetDecimal(2);
                        MS.NumberFormatter = new DecimalFormatter
                        {
                            IntegerDigits = 1, FractionDigits = 0
                        };
                        MS.Maximum = 300;
                        break;
                    case "dia":
                        MDT.Text = rd.GetString(1);
                        MD.Value = (double)rd.GetDecimal(2);
                        MD.NumberFormatter = new DecimalFormatter
                        {
                            IntegerDigits = 1, FractionDigits = 0,
                        };
                        MD.Maximum = 300;
                        break;
                    case "pulse":
                        MPT.Text = rd.GetString(1);
                        MP.Value = (double)rd.GetDecimal(2);
                        MP.NumberFormatter = new DecimalFormatter
                        {
                            IntegerDigits = 1, FractionDigits = 0,
                        };
                        MP.Maximum = 300;
                        break;
                }
            }
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
            return MS.Text + "/" + MD.Text + " " + MPT.Text + ":" + MP.Text;
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
    }
}