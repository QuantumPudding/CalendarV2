using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CalendarV2
{
    public sealed partial class Calendar : UserControl
    {
        #region Globals

        public Color ForeColor
        {
            get { return forecolor; }
            set
            {
                SolidColorBrush forebrush = new SolidColorBrush(value);

                txtTitle.Foreground = forebrush;
                btnLastYear.Foreground = forebrush;
                btnLastYear.Foreground = forebrush;

                bdrDays.Background = forebrush;
                bdrNotes.Background = forebrush;

                wgridNotes.Foreground = forebrush;

                forecolor = value;
            }
        }
        public Color BackColor
        {
            get { return backcolor; }
            set
            {
                SolidColorBrush backbrush = new SolidColorBrush(value);

                txtMonday.Foreground = backbrush;
                txtTuesday.Foreground = backbrush;
                txtWednesday.Foreground = backbrush;
                txtThursday.Foreground = backbrush;
                txtFriday.Foreground = backbrush;
                txtSaturday.Foreground = backbrush;
                txtSunday.Foreground = backbrush;
                txtNotes.Foreground = backbrush;          

                btnLastYear.Background = backbrush;
                btnLastYear.Background = backbrush;

                wgridNotes.Background = backbrush;

                Border.Background = backbrush;

                backcolor = value;
            }
        }
        public Color UnfocusedColor { get; set; }

        public DateTime Date
        {
            get { return date; }
            set
            {
                //Change of year - needs reinitialising
                if (value.Year != date.Year)
                {
                    InitialiseCalendar(value.Year);

                    date = value;
                    HighlightMonth(value.Month, value.Day);
                    //ScrollToMonth(value.Month, value.Day);
                }

                date = value;
            }
        }

        public int DisplayedMonth
        {
            get { return displayedMonth; }
            set
            {
                SetTitle(date.Year, value);
                displayedMonth = value;
            }
        }

        private DateTime date;
        private Color forecolor;
        private Color backcolor;
        private Grid nextYearGrid;
        private Grid previousYearGrid;

        private int[] scrollTriggers;
        private int displayedMonth;

        private int rowHeight;
        private int gridHeight;
        private int gridSection;
        private int offset;

        private bool addingNotes;
        
        #endregion

        #region Initialisation

        public Calendar()
        {
            this.InitializeComponent();
            this.Loaded += (object sender, RoutedEventArgs e) =>
                {
                    //Set up scroll triggers
                    rowHeight = (int)DateGrid.ColumnDefinitions[0].ActualWidth;
                    gridHeight = rowHeight * 53;
                    gridSection = gridHeight / 12;
                    offset = (int)(scrollerDate.ViewportHeight * 0.5) - rowHeight;

                    scrollTriggers = new int[12];
                    for (int i = 0; i < 12; i++)
                        scrollTriggers[i] = i * (gridHeight / 12) - offset;
                    
                    //Initialise date
                    Date = DateTime.Now;
                };
        }

        private List<CalendarButton> InitialiseCalendar(int year)
        {
            DateTime firstday = new DateTime(year, 1, 1);
            List<CalendarButton> dates = new List<CalendarButton>();
            DateGrid.Children.Clear();

            //Get starting day offset
            int weekDay = ((int)firstday.DayOfWeek + 6) % 7;
            int weekCount = 0;
            int count = 0;

            //Initialise the first week's row
            //if (DateGrid.RowDefinitions[weekCount] == null)
            DateGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeight) });

            //For each month
            for (int i = 1; i < 13; i++)
            {
                int daysInMonth = DateTime.DaysInMonth(year, i);

                //For each day in that month
                for (int j = 1; j < daysInMonth + 1; j++)
                {                 
                    bool today = (i == date.Month && j == date.Day);

                    //Generate date item
                    CalendarButton b = new CalendarButton(this);
                    b.Name = j.ToString() + "/" + i.ToString() + "/" + year.ToString();
                    b.Day = j;
                    b.Tag = new DateTime(year, i, j);
                    b.Tapped += OnDateButtonTapped;
                    b.EventChanged += OnDateEventChanged;

                    if (today)
                        b.Info = "Today";

                    //Set item to corresponding grid cell
                    Grid.SetRow(b, weekCount);
                    Grid.SetColumn(b, weekDay);

                    DateGrid.Children.Add(b);

                    //Cycle/increment the day and week
                    weekDay = (weekDay + 1) % 7;
                    if (weekDay == 0)
                    {
                        //Row height equals the column width
                        DateGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeight) });
                        weekCount++;
                    }

                    count++;
                }
            }

            return dates;
        }

        #endregion

        #region Methods

        private void OnDateEventChanged(object sender, EventArgs e)
        {

        }

        private void OnDateButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            CalendarButton b = (CalendarButton)sender;
            DateTime dt = (DateTime)b.Tag;
            Date = new DateTime(date.Year, dt.Month, dt.Day);

            HighlightMonth(date.Month, date.Day);
            txtNotes.Text = "Notes - " + b.Name;
        }

        private async void ScrollToMonth(int month, int day)
        {
            DisplayedMonth = month;

            //Create scrolling offset. 0 for Jan
            double d = (month > 1) ? (int)DateGrid.ColumnDefinitions[0].ActualWidth * (day / 5) : 0;
            d -= scrollerDate.ViewportHeight;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, new DispatchedHandler(() =>
            {
                scrollerDate.ScrollToVerticalOffset(scrollTriggers[month] + d);
            }));
        }

        private void OnCalendarScroll(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer s = sender as ScrollViewer;

            for (int i = 1; i < 13; i++)
                if (s.VerticalOffset > scrollTriggers[i - 1])
                    DisplayedMonth = i;
        }

        private void HighlightMonth(int month, int day)
        {
            foreach (CalendarButton b in DateGrid.Children)
            {
                Color fcolor = UnfocusedColor;
                SolidColorBrush borderBrush = new SolidColorBrush(fcolor);
                SolidColorBrush backBrush = new SolidColorBrush(backcolor);
                DateTime dt = (DateTime)b.Tag;

                if (dt.Month == month)
                {
                    fcolor = forecolor;
                    borderBrush = new SolidColorBrush(fcolor);

                    if (dt.Day == day)
                    {
                        backBrush = new SolidColorBrush(fcolor);
                        fcolor = backcolor;
                    }
                }

                b.ForeColor = fcolor;
                b.Border.Background = backBrush;
                b.Border.BorderBrush = borderBrush;
            }
        }

        private void OnNextYearTapped(object sender, RoutedEventArgs e)
        {
            if (date.Year + 1 == DateTime.Now.Year)
                Date = DateTime.Now;
            else
                Date = new DateTime(date.Year + 1, 1, 1);
        }

        private void OnPreviousYearTapped(object sender, RoutedEventArgs e)
        {
            if (date.Year - 1 == DateTime.Now.Year)
                Date = DateTime.Now;
            else
                Date = new DateTime(date.Year - 1, 1, 1);
        }

        private void OnNotesTapped(object sender, RoutedEventArgs e)
        {
            if (wgridNotes.SelectedIndex == -1)
            {
                if (!addingNotes)
                {
                    TextBox tb = new TextBox();
                    tb.Width = wgridNotes.ActualWidth / 2;
                    tb.Margin = new Thickness(0, 0, 0, 0);
                    tb.FontSize = 18;
                    tb.BorderBrush = new SolidColorBrush(Colors.Black);
                    tb.BorderThickness = new Thickness(1);
                    tb.KeyDown += OnNotesTextBoxKeyDown;
                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                    tb.VerticalAlignment = VerticalAlignment.Top;

                    wgridNotes.Items.Add(tb);
                    addingNotes = true;

                    tb.Focus(FocusState.Programmatic);
                }
                else ((TextBox)wgridNotes.Items[wgridNotes.Items.Count - 1]).Focus(FocusState.Programmatic);
            }
        }

        private void OnNotesTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox tb = (TextBox)sender;
                ListBoxItem item = new ListBoxItem();
                TextBlock text = new TextBlock();

                text.Text = tb.Text;
                text.FontSize = 18;
                text.Foreground = new SolidColorBrush(Colors.Black);
                text.Margin = new Thickness(10, 0, 0, 0);
                text.TextWrapping = TextWrapping.Wrap;

                item.Content = text;
                item.Height = text.Height;

                wgridNotes.Items.Remove(tb);
                wgridNotes.Items.Add(item);

                addingNotes = false;
                e.Handled = true;
            }
        }

        private void SetTitle(int year, int month)
        {
            switch (month)
                {
                    case 1:
                        txtTitle.Text = "January";
                        break;
                    case 2:
                        txtTitle.Text = "February";
                        break;
                    case 3:
                        txtTitle.Text = "March";
                        break;
                    case 4:
                        txtTitle.Text = "April";
                        break;
                    case 5:
                        txtTitle.Text = "May";
                        break;
                    case 6:
                        txtTitle.Text = "June";
                        break;
                    case 7:
                        txtTitle.Text = "July";
                        break;
                    case 8:
                        txtTitle.Text = "August";
                        break;
                    case 9:
                        txtTitle.Text = "September";
                        break;
                    case 10:
                        txtTitle.Text = "October";
                        break;
                    case 11:
                        txtTitle.Text = "November";
                        break;
                    case 12:
                        txtTitle.Text = "December";
                        break;
                }
            txtTitle.Text += " - " + year.ToString();
        }

        #endregion
    }
}
