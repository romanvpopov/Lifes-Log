using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Lifes_log
{
    public sealed partial class Health : Page
    {
        private readonly string lang = (App.Current as App).lang;

        public Health()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var cmd = (App.Current as App).NpDs.CreateCommand($@"
                   Select Distinct lt.id,lt.priority,lt.{lang}_name From ll_event_type lt
                   Where lt.priority > 0 and lt.HSM = 'H' Order by priority");
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    //EL.Items.Add(new Healths.HQEvent(rd.GetInt16(0), rd.GetString(2)));
                }
                rd.Close();

        }
    }
}
