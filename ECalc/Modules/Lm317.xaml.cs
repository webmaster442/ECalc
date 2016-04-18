using System.Windows;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for Lm317.xaml
    /// </summary>
    public partial class Lm317 : UserControl
    {
        private bool _loaded;

        public Lm317()
        {
            InitializeComponent();
        }

        private void ValueChanged(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;

            if (TabMode.SelectedIndex == 0)
            {
                var vout = 1.25 * (1 + (VregR2.Value / VregR1.Value)) + (50e-6 * VregR2.Value);
                if (vout > VregVin.Value) vout = VregVin.Value;
                var power = VregVin.Value - vout;
                TbOutput.Text = string.Format("Vout = {0:0.0000} V, Dissipated power: {1:0.0000} W/A", vout, power);
            }
            else
            {
                var iout = 1.25 / IregR1.Value;
                var p = (iout * iout) * IregR1.Value;
                TbOutput.Text = string.Format("Iout = {0:0.0000} A, Power of R1 = {1:0.0000} W", iout, p);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
        }
    }
}
