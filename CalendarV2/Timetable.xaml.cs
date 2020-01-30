using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CalendarV2
{
    public sealed partial class Timetable : UserControl
    {
        public Timetable()
        {
            this.InitializeComponent();

            for (int i = 0; i < 24; i++)
            {
                ListBoxItem item = new ListBoxItem();
                TextBlock tb = new TextBlock();
                string s = i.ToString();

                tb.Text = ((i < 10) ? "0" : "") + s + ":00";
                item.Name = s;
                item.Content = tb;
                lbTimes.Items.Add(item);
            }
                
        }
    }
}
