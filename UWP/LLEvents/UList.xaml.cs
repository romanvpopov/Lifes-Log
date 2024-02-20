using System.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LL.LLEvents
{
    public sealed partial class UList : EventBody
    {
        private string lang = (App.Current as App).lang;
        private readonly ObservableCollection<ListBodyField> lsts = new ObservableCollection<ListBodyField>();

        public UList(SqlCommand cmd, Int32 Code, Int16 ntp)
        {
            this.InitializeComponent();
            cmd.CommandText = $"Select lf.Code,isNull(lf.{lang}_FieldName,''),isNull(lf.{lang}_FieldSmallName,''),isNull(lv.FieldValue,'')" +
                $"From LLFieldEvent lf left join LLEventValue lv on lf.Code = lv.FieldEventCode and lv.EventCode={Code} " +
                $"Where lf.EventTypeCode ={ntp} Order by lf.{lang}_FieldName";
            var rd = cmd.ExecuteReader();
            while (rd.Read()) {
                lsts.Add(new ListBodyField { Code = rd.GetInt32(0), Name = rd.GetString(1), ShortName = rd.GetString(2), Value = rd.GetString(3) });
            }
            FieldList.ItemsSource = lsts;
        }

        public override void InsertBody(SqlCommand cmd, Int32 cd)
        {
            foreach (ListBodyField s in FieldList.Items)
            {
                if (s.Value != "") {
                    cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{s.Code},'{s.Value}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public override string ToString() {
            var st = "";
            foreach (ListBodyField s in FieldList.Items) { if (s.Value != "") { st += s.ShortName + ":" + s.Value + " "; } }
            return st;
        }

        private void TextBox_KeyUp(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf(); }
        }

        private void ListViewItem_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                var tb = (((sender as ListViewItem).Content as StackPanel).Children).ElementAt(1) as TextBox;
                tb.Focus(FocusState.Programmatic);
            }
        }

        private void Grid_KeyDown(object _, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Down & FieldList.Items.Last() == FieldList.SelectedItem) { Sf(); }
        }
    }

    public class ListBodyField
    {
        public int Code;
        public string Name;
        public string ShortName;
        public string Value;
    }
}


