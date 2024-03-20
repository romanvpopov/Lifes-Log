using System.Collections.ObjectModel;
using System.Linq;
using Windows.Globalization.NumberFormatting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Npgsql;

namespace Lifes_log.LLEvents
{
    public sealed partial class UList
    {
        private readonly ObservableCollection<ListBodyField> lists = [];
        private readonly DecimalFormatter decF = new() {
            IntegerDigits = 1, FractionDigits = 2,
            NumberRounder = new IncrementNumberRounder { Increment = 0.01 }
        };

        public UList(NpgsqlCommand cmd, int code)
        {
            InitializeComponent();
            cmd.CommandText = $@"
                Select lt.key,lt.{App.lang}_name,lt.{App.lang}_short_name,lv.dec_value
                From ll_value_type lt left join ll_value lv on lt.key = lv.value_type_key and lv.event_id={code}
                Where lt.key like 'bcb%'";
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lists.Add(new ListBodyField
                {
                    key = rd.GetString(0),
                    Name = rd.GetString(1),
                    ShortName = rd.GetString(2),
                    Value = rd.IsDBNull(3) ? double.NaN : (double)rd.GetDecimal(3),
                    dcF = decF
                });
            }
            rd.Close();
            FieldList.ItemsSource = lists;
        }

        public override void InsertBody(NpgsqlCommand cmd, int cd)
        {
            foreach (var s in FieldList.Items.Cast<ListBodyField>())
            {
                if (double.IsNaN(s.Value)) continue;
                cmd.CommandText = $@"
                    Insert into ll_value (event_id,value_type_key,dec_value)
                    Values({cd},'{s.key}','{s.Value.ToString().Replace(",", ".")}')";
                cmd.ExecuteNonQuery();
            }
        }

        public override string ToString()
        {
            return FieldList.Items.Cast<ListBodyField>().Where(s => !double.IsNaN(s.Value))
                .Aggregate("", (current, s) => current + (s.ShortName + ":" + s.Value + " "));
        }

        private void Grid_KeyDown(object _, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Down & FieldList.Items.Last() == FieldList.SelectedItem) {Sf();}
        }

        private void Val_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter) return ;
            var tb = ((sender as NumberBox).Parent as StackPanel).Parent as ListViewItem;
            tb.ToString();
            tb.Focus(FocusState.Programmatic);
        }

        private void ListViewItem_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter) return;
            var tb = (((sender as ListViewItem).Content as StackPanel).Children).ElementAt(1) as NumberBox;
            tb.Focus(FocusState.Programmatic);
        }
    }

    public class ListBodyField
    {
        public string key;
        public string Name;
        public string ShortName;
        public double Value;
        public DecimalFormatter dcF;
    }
}