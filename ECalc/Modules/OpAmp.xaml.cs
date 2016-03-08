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
    /// Interaction logic for OpAmp.xaml
    /// </summary>
    public partial class OpAmp : UserControl
    {
        public OpAmp()
        {
            InitializeComponent();
        }

        private void ValueChanged(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void Calculate()
        {
            double a = 0;
            double vout = 0;
            switch (TabModeSelector.SelectedIndex)
            {
                case 0:
                    a = 1 + (NonInvertR2.Value / NonInvertR1.Value);
                    vout = a * NonInvertVin.Value;
                    TbOutput.Text = string.Format("Vout = {0} V, Gain = {1}", vout, a);
                    break;
                case 1:
                    a = -1 * (InvertRf.Value / InvertRin.Value);
                    vout = a * InvertVin.Value;
                    TbOutput.Text = string.Format("Vout = {0} V, Gain = {1}", vout, a);
                    break;
                case 2:
                    vout = UgainVin.Value;
                    TbOutput.Text = string.Format("Vout = {0} V, Gain = {1}", vout, 1);
                    break;
                case 3:
                    double v2 = (DifRf.Value + DifR1.Value) * DifRg.Value;
                    v2 /= (DifRg.Value + DifR2.Value) * DifR1.Value;
                    v2 *= DifV2.Value;
                    double v1 = (DifRf.Value / DifR1.Value) * DifV1.Value;
                    vout = v2 - v1;
                    TbOutput.Text = string.Format("Vout = {0} V", vout);
                    break;
            }
        }

        private void TabModeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }
    }
}
