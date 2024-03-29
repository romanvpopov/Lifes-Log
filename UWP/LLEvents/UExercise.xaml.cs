﻿using System.Data.SqlClient;
using System;
using Windows.UI.Xaml;

namespace LL.LLEvents
{
    public sealed partial class UExercise
    {
        private readonly string lang = (App.Current as App).lang;

        public UExercise(SqlCommand cmd, int code, short ntp)
        {
            InitializeComponent();
            cmd.CommandText = $@"
                Select lu.Code, lf.Code, isNull(lf.{lang}_FieldName,''), isNull(lf.{lang}_FieldSmallName,''),
                isNull(lv.FieldValue,''), isNull(lu.{lang}_UnitName,''), isNull(lu.{lang}_UnitSmallName,'')
                From LLFieldEvent lf left join LLEventValue lv on lf.Code = lv.FieldEventCode and lv.EventCode={code}
                left join LLUnit lu on lf.UnitCode=lu.Code Where lf.EventTypeCode ={ntp}";
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                switch (rd.GetInt16(0))
                {
                    case 9:
                        Calories.Text = rd.GetString(5);
                        Calories.Tag = rd.GetString(6);
                        VCalories.Tag = rd.GetInt32(1);
                        VCalories.Text = rd.GetString(4); break;
                    case 1:
                        Repeats.Text = rd.GetString(5);
                        Repeats.Tag = rd.GetString(6);
                        VRepeats.Tag = rd.GetInt32(1);
                        VRepeats.Text = rd.GetString(4); break;
                    case 11:
                        Time.Text = rd.GetString(5);
                        Time.Tag = rd.GetString(6);
                        VTime.Tag = rd.GetInt32(1);
                        VTime.Text = rd.GetString(4); break;
                }
            }
        }

        public override void GetFocus() { VCalories.Focus(FocusState.Programmatic); }

        public override void InsertBody(SqlCommand cmd, Int32 cd)
        {
            cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{VCalories.Tag},'{VCalories.Text}')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{VRepeats.Tag},'{VRepeats.Text}')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{VTime.Tag},'{VTime.Text}')";
            cmd.ExecuteNonQuery();
        }
        public override string ToString()
        {
            return (VCalories.Text + " " + Calories.Tag + ", " + VRepeats.Text + " " + Repeats.Tag + ", " + VTime.Text + " " + Time.Tag);

        }
        private void C_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { VRepeats.Focus(FocusState.Programmatic); }
        }

        private void D_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { VTime.Focus(FocusState.Programmatic); }
        }

        private void T_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf(); }
        }

    }
}