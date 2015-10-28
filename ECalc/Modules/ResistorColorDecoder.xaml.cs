using ECalc.Classes;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ResistorColorDecoder.xaml
    /// </summary>
    public partial class ResistorColorDecoder : UserControl
    {
        private bool _loaded;
        private PrefixDictionary _prefixes;

        public ResistorColorDecoder()
        {
            InitializeComponent();
            _prefixes = new PrefixDictionary();
        }

        private void Calculate()
        {
            if (!_loaded) return;
            double value = 0.0;
            double tolerance = 0.0;
            switch (TabType.SelectedIndex)
            {
                case 0:
                    value = ((band41.Value * 10) + band42.Value) * band4m.Value;
                    tolerance = band4t.Value;
                    break;
                case 1:
                    value = ((band51.Value * 100) + (band52.Value * 10) + band53.Value) * band5m.Value;
                    tolerance = band5t.Value;
                    break;
                case 2:
                    value = ((band61.Value * 100) + (band62.Value * 10) + band63.Value) * band6m.Value;
                    tolerance = band6t.Value;
                    break;
            }

            switch (TabType.SelectedIndex)
            {
                case 0:
                case 1:
                    TbResult.Text = string.Format("{0} Ω ± {1} %", _prefixes.DivideToPrefix(value), tolerance);
                    break;
                case 2:
                    TbResult.Text = string.Format("{0} Ω ± {1} % {2} ppm", _prefixes.DivideToPrefix(value), tolerance, band6temp.Value);
                    break;
            }

        }

        private void SelectionChanged(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
            Calculate();
        }

        private void TabType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }
    }
}
