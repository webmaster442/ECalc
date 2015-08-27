using ECalc.Engineering;
using System.Windows;
using System.Windows.Controls;

namespace MTools.ToolsAnalog
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
            ResistorSeries serie = (ResistorSeries)SeriesSelector.SelectedIndex;
            TbResult.Text = ResistorValueSolver.Solve(TargetValue.Value, serie);
        }
    }
}
