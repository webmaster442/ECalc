using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for BitDisplay.xaml
    /// </summary>
    public partial class BitDisplay : UserControl
    {

        public BitDisplay()
        {
            InitializeComponent();
        }
    }

    public class CheckedToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            bool? val = (bool?)value;
            if (val == true) return new SolidColorBrush(Colors.IndianRed);
            else return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
