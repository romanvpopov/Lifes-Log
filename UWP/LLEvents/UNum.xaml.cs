using System.Data.SqlClient;
using System;
using System.Linq;
using Windows.UI.Xaml;

namespace LL.LLEvents
{
    public sealed partial class UNum : EventBody
    {
        private Int32 vc;
        public override void GetFocus() { Qty.Focus(FocusState.Programmatic); }
        private readonly string lang = (App.Current as App).lang;

        public UNum(SqlCommand cmd, Int32 Code, Int16 ntp)
        {
            this.InitializeComponent();
            cmd.CommandText = $"Select lf.Code,isNull(lv.FieldValue,''),lf.{lang}_FieldName " +
                $"From LLFieldEvent lf left join LLEventValue lv on lf.Code = lv.FieldEventCode and lv.EventCode={Code} " +
                $"Where lf.EventTypeCode ={ntp}";
            var rd = cmd.ExecuteReader();
            if (rd.Read()) {
                vc = rd.GetInt32(0);
                Qty.Text = rd.GetString(1);
                Value.Text = rd.GetString(2);
                }
        }

        public override void InsertBody(SqlCommand cmd, Int32 cd)
        {
            cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{vc},'{Qty.Text}')";
            cmd.ExecuteNonQuery();
        }

        public override string ToString() { return Qty.Text; }

        private void Qty_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf?.Invoke(); }
        }

        private void Qty_BeforeTextChanging(Windows.UI.Xaml.Controls.TextBox sender, Windows.UI.Xaml.Controls.TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}   