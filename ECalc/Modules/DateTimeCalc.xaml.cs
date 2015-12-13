using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for DateTimeCalc.xaml
    /// </summary>
    public partial class DateTimeCalc : UserControl
    {
        public DateTimeCalc()
        {
            InitializeComponent();
        }

        private string TimeSpan2String(TimeSpan ts)
        {
            return string.Format("{0} day(s), {1} hour(s), {2} minute(s), {3} seconds", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
        }

        private void BtnDifCalc_Click(object sender, RoutedEventArgs e)
        {
            var result = DateSelect1.SelectedDateTime - DateSelect2.SelectedDateTime;
            TbResult.Text = TimeSpan2String(result);
        }
    }
}
