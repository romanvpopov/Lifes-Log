﻿using LL.LLEvents;
using System.Data.SqlClient;
using Windows.UI.Xaml.Navigation;

namespace LL
{
    public sealed partial class Search
    {
        public Search()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var ss = e.Parameter as string;
            if (ss == "") return;
            using (SqlConnection sq = new SqlConnection((App.Current as App).ConStr))
            {
                sq.Open();
                var cmd = sq.CreateCommand();
                cmd.CommandText =
                   $"Select DateEvent From llEvent Where Comment like '%{ss}%' or Descr like '%{ss}%' Order by DateEvent";
                try
                {
                    var rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read()) EL.Items.Add(new Day(rd.GetDateTime(0), "0"));
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
