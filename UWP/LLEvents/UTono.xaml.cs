using System.Data.SqlClient;
using System;
using Windows.UI.Xaml;

namespace LL.LLEvents
{
    public sealed partial class UTono 
    {
        public UTono(SqlCommand cmd, int code, short ntp)
        {
            InitializeComponent();
            if (code <= 0) return;
            cmd.CommandText = $"Select FieldEventCode,FieldValue From LLEventValue Where EventCode={code}";
            var rd = cmd.ExecuteReader();
            while (rd.Read()) {
                switch (rd.GetInt32(0)) {
                    case 2: MS.Text = rd.GetString(1); break;
                    case 3: MD.Text = rd.GetString(1); break;
                    case 4: MP.Text = rd.GetString(1); break;
                    case 5: ES.Text = rd.GetString(1); break;
                    case 6: ED.Text = rd.GetString(1); break;
                    case 7: EP.Text = rd.GetString(1); break;
                }
            }
        }    

        public override void GetFocus() {
            if (MS.Text == "") {
                MS.Focus(FocusState.Programmatic);
            } else {
                ES.Focus(FocusState.Programmatic);
            }
        }

        public override void InsertBody(SqlCommand cmd, Int32 cd)
        {
            if (MS.Text != "")
            {
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},2,'{MS.Text}')";
                cmd.ExecuteNonQuery();
            }
            if (MD.Text != "")
            {
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},3,'{MD.Text}')";
                cmd.ExecuteNonQuery();
            }
            if (MP.Text != "")
            {
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},4,'{MP.Text}')";
                cmd.ExecuteNonQuery();
            }
            if (ES.Text != "")
            {
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},5,'{ES.Text}')";
                cmd.ExecuteNonQuery();
            }
            if (ED.Text != "")
            {
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},6,'{ED.Text}')";
                cmd.ExecuteNonQuery();
            }
            if (EP.Text != "")
            {
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},7,'{EP.Text}')";
                cmd.ExecuteNonQuery();
            }
        }
        public override string ToString()
        {
            return (MS.Text != "" | MP.Text != "" ? "утро " + MS.Text + "/" + MD.Text : "") + (MP.Text != "" ? "П" + MP.Text : "") +
               (ES.Text != "" | EP.Text != "" ? " вечер " + ES.Text + "/" + ED.Text : "") + (EP.Text != "" ? "П" + EP.Text : "");

        }
        private void MS_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { MD.Focus(FocusState.Programmatic); }
        }

        private void MD_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { MP.Focus(FocusState.Programmatic); }
        }

        private void ES_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { ED.Focus(FocusState.Programmatic); }
        }

        private void ED_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { EP.Focus(FocusState.Programmatic); }
        }

        private void MP_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf(); }
        }

        private void EP_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf(); }
        }
    }
}
