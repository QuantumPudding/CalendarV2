using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CalendarV2
{
    public sealed partial class CalendarButton : UserControl
    {
        public Color ForeColor
        {
            set
            {
                SolidColorBrush brush = new SolidColorBrush(value);
                txtDayInfo.Foreground = brush;
                txtDayNumber.Foreground = brush;
                txtEvent.Foreground = brush;
                txtEventBtn.Foreground = brush;
                btnAddEvent.Foreground = brush;
                Foreground = brush;
            }
        }

        public EventHandler EventChanged;
        public List<string> Notes;
        public string Event
        {
            get { return txtEvent.Text; }
            set { txtEvent.Text = value; }
        }
        public string Info
        {
            get { return txtDayInfo.Text; }
            set { txtDayInfo.Text = value; }
        }
        public int Day
        {
            get { return int.Parse(txtDayNumber.Text); }
            set { txtDayNumber.Text = value.ToString(); }
        }

        private Calendar parent;

        public CalendarButton(Calendar c)
        {
            this.InitializeComponent();
            Notes = new List<string>();
        }

        private void OnEventAdd(object sender, TappedRoutedEventArgs e)
        {
            TextBox tb = new TextBox();
            Grid.SetRow(tb, 1);
            Grid.SetColumn(tb, 0);
            Grid.SetColumnSpan(tb, 3);

            DataGrid.Children.Add(tb);

            tb.KeyDown += OnEntryKeyDown;
            tb.FontSize = 20;
            tb.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            tb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            tb.Margin = new Thickness(10, 0, 10, 0);
            tb.BorderThickness = new Thickness(1);
            tb.BorderBrush = new SolidColorBrush(Colors.Black);

            e.Handled = true;
        }

        private void OnEntryKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                txtEvent.Text = ((TextBox)sender).Text;
                DataGrid.Children.Remove((TextBox)sender);

                EventChanged(sender, null);
            }
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            
 	        base.OnTapped(e);
        }
    }
}
