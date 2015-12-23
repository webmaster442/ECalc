using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for TimeSpanSelector.xaml
    /// </summary>
    public partial class TimeSpanSelector : UserControl
    {
        public TimeSpanSelector()
        {
            InitializeComponent();
        }

        public TimeSpan TimeSpan
        {
            get
            {
                return new TimeSpan((int)Days.Value, (int)Hours.Value, (int)Minutes.Value, (int)Seconds.Value);
            }
            set
            {
                Days.Value = value.Days;
                Hours.Value = value.Hours;
                Minutes.Value = value.Minutes;
                Seconds.Value = value.Seconds;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan = TimeSpan.FromSeconds(0);
        }
    }
}
