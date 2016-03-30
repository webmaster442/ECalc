using ECalc.Engineering;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ResistorSolver.xaml
    /// </summary>
    public partial class ResistorSolver : UserControl
    {
        public ResistorSolver()
        {
            InitializeComponent();
        }

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            var serie = (ResistorSeries)SeriesSelector.SelectedIndex;

            if (RbCombination.IsChecked == true) TbResult.Text = ResistorValueSolver.Solve(TargetValue.Value, serie);
            else TbResult.Text = ResistorValueSolver.StandardResistorValueApproximation(TargetValue.Value, serie);

        }
    }
}
