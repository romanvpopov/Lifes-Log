﻿using Windows.UI.Xaml.Controls;

namespace LL.Settings
{
    public sealed partial class SetEventType : Page
    {
        public SetEventType()
        {
            this.InitializeComponent();
            U1.Content = new SetSHM("");
            U2.Content = new SetSHM("S");
            U3.Content = new SetSHM("H");
        }
    }
}
