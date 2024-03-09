using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Npgsql;

namespace Lifes_log.LLEvents
{
    public sealed partial class UList
    {
        private readonly string lang = (App.Current as App).lang;
        private readonly ObservableCollection<ListBodyField> lists = new();

        public UList(NpgsqlCommand cmd, int code, short ntp)
        {
            InitializeComponent();
            cmd.CommandText = 
                $"Select lf.Code,isNull(lf.{lang}_FieldName,''),isNull(lf.{lang}_FieldSmallName,''),isNull(lv.FieldValue,'')" +
                $"From LLFieldEvent lf left join LLEventValue lv on lf.Code = lv.FieldEventCode and lv.EventCode={code} " +
                $"Where lf.EventTypeCode ={ntp} Order by lf.{lang}_FieldName";
            var rd = cmd.ExecuteReader();
            while (rd.Read()) {
                lists.Add(new ListBodyField { Code = rd.GetInt32(0), Name = rd.GetString(1), ShortName = rd.GetString(2), Value = rd.GetString(3) });
            }
            FieldList.ItemsSource = lists;
        }

        public override void InsertBody(NpgsqlCommand cmd, int cd)
        {
            foreach (ListBodyField s in FieldList.Items.Cast<ListBodyField>())
            {
                if (s.Value == "") continue;
                cmd.CommandText = $"Insert into LLEventValue (EventCode,FieldEventCode,FieldValue) Values({cd},{s.Code},'{s.Value}')";
                cmd.ExecuteNonQuery();
            }
        }
        public override string ToString()
        {
            return FieldList.Items.Cast<ListBodyField>().Where(s => s.Value != "").
                Aggregate("", (current, s) => current + (s.ShortName + ":" + s.Value + " "));
        }

        private void TextBox_KeyUp(object _, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) { Sf(); }
        }

        private void ListViewItem_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter) return;
            var tb = (((sender as ListViewItem).Content as StackPanel).Children).ElementAt(1) as TextBox;
            tb.Focus(FocusState.Programmatic);
        }

        private void Grid_KeyDown(object _, KeyRoutedEventArgs e)
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


