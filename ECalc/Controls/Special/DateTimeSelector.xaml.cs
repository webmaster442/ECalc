using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for DateTimeSelector.xaml
    /// </summary>
    public partial class DateTimeSelector : UserControl
    {
        public DateTimeSelector()
        {
            InitializeComponent();
        }

        public DateTime SelectedDateTime
        {
            get
            {
                var dt = (DateTime)DatePicker.SelectedDate;
                return dt;
            }
            set
            {
                DateTime dt = value;
                DatePicker.SelectedDate = dt;
            }
        }

        private void BtnLocal_Click(object sender, RoutedEventArgs e)
        {
            SelectedDateTime = DateTime.Now;
        }

        private void BtnUTC_Click(object sender, RoutedEventArgs e)
        {
            SelectedDateTime = DateTime.UtcNow;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker.Culture = CultureInfo.CurrentUICulture;
            DatePicker.FirstDayOfWeek = CultureInfo.CurrentUICulture.DateTimeFormat.FirstDayOfWeek;
        }
    }
}
