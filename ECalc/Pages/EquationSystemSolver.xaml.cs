using ECalc.Extensions;
using ECalc.Maths;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for EquationSystemSolver.xaml
    /// </summary>
    public partial class EquationSystemSolver : UserControl
    {
        private string[] _labels = new string[] { "A", "B", "C", "D", "E", "F" };
        private int _equations;
        private bool _loaded;

        public EquationSystemSolver()
        {
            InitializeComponent();
            _equations = 2;
            RenderUi();
        }

        private void RenderUi()
        {
            EquationData.Children.Clear();
            EquationData.RowDefinitions.Clear();
            EquationData.ColumnDefinitions.Clear();

            for (int i=0; i<_equations; i++)
            {
                EquationData.RowDefinitions.Add(new RowDefinition());
                EquationData.ColumnDefinitions.Add(new ColumnDefinition());
            }
            EquationData.RowDefinitions.Add(new RowDefinition());
            EquationData.ColumnDefinitions.Add(new ColumnDefinition());
            EquationData.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i=1; i<_equations+1; i++)
            {
                for (int j=0; j<_equations; j++)
                {
                    TextBox tx = new TextBox();
                    tx.Margin = new Thickness(3);
                    tx.Name = string.Format("Tb_{0}_{1}", i, j);
                    tx.Text = "0";
                    tx.Width = 80;
                    Grid.SetRow(tx, i);
                    Grid.SetColumn(tx, j);
                    EquationData.Children.Add(tx);
                }
            }

            for (int i=0; i<_equations; i++)
            {
                TextBlock tx = new TextBlock();
                tx.TextAlignment = TextAlignment.Center;
                tx.Text = _labels[i];
                Grid.SetRow(tx, 0);
                Grid.SetColumn(tx, i);
                EquationData.Children.Add(tx);
            }

            TextBlock eq = new TextBlock();
            eq.Text = "=";
            eq.VerticalAlignment = VerticalAlignment.Center;
            eq.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(eq, _equations);
            Grid.SetRowSpan(eq, _equations);
            EquationData.Children.Add(eq);

            for (int i=0; i<_equations; i++)
            {
                TextBox tx = new TextBox();
                tx.Margin = new Thickness(3);
                tx.Name = string.Format("Tb_{0}_{1}", _equations+1, i);
                tx.Width = 80;
                tx.Text = "0";
                Grid.SetRow(tx, i+1);
                Grid.SetColumn(tx, _equations + 1);
                EquationData.Children.Add(tx);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            var radios = EqSelector.FindChildren<RadioButton>();
            var num = (from r in radios where r.IsChecked == true select Convert.ToInt32(r.Content)).FirstOrDefault();
            _equations = num;
            RenderUi();
        }

        private DoubleMatrix GetInputMatrix()
        {
            DoubleMatrix m = new DoubleMatrix(_equations, _equations + 1);

            var children = EquationData.FindChildren<TextBox>();

            foreach (var child in children)
            {
                int col = Grid.GetColumn(child);
                int row = Grid.GetRow(child) - 1;
                if (col > _equations) col -= 1;
                double val = Convert.ToDouble(child.Text);
                m[row, col] = val;
            }

            return m;
        }

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            try
            {
                DoubleMatrix inputmatrix = GetInputMatrix();
                double[] resultvector = inputmatrix.GetColumn(_equations);
                DoubleMatrix D = inputmatrix.TrimTo(_equations, _equations);
                double detd = D.Determinant();
                DoubleMatrix[] eqs = new DoubleMatrix[_equations];
                StringBuilder outtext = new StringBuilder();

                for (int i = 0; i < eqs.Length; i++)
                {
                    eqs[i] = (DoubleMatrix)D.Clone();
                    eqs[i].SetColumn(i, resultvector);
                    outtext.AppendFormat("{0}: {1}\r\n", _labels[i], eqs[i].Determinant() / detd);
                }
                TBResults.Text = outtext.ToString();

            }
            catch (Exception ex)
            {
                TBResults.Text = ex.Message;
            }
        }


    }
}
