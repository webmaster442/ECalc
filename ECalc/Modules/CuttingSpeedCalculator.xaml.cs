using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for CuttingSpeedCalculator.xaml
    /// </summary>
    public partial class CuttingSpeedCalculator : UserControl
    {
        public CuttingSpeedCalculator()
        {
            InitializeComponent();
        }

        private double CalculateV()
        {
            double tmp = D.Value * Math.PI * N.Value;
            return tmp / 1000;
        }

        private double CalculateN()
        {
            double tmp = V.Value * 1000;
            return tmp / (D.Value * Math.PI);
        }

        private void N_ValueChanged(object sender, RoutedEventArgs e)
        {
            var newv = CalculateV();
            V.SetValue(newv);
        }

        private void V_ValueChanged(object sender, RoutedEventArgs e)
        {
            var newn = CalculateN();
            N.SetValue(newn);
        }
    }
}
