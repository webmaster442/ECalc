using System;
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
                DateTime dt = (DateTime)DatePicker.SelectedDate;
                return dt.Add(new TimeSpan((int)SelH.Value, (int)SelM.Value, (int)SelS.Value));
            }
            set
            {
                DateTime dt = value;
                DatePicker.SelectedDate = dt;
                SelH.Value = dt.Hour;
                SelM.Value = dt.Minute;
                SelS.Value = dt.Second;
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
    }
}
