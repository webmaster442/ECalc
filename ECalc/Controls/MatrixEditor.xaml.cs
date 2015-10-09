using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for MatrixEditor.xaml
    /// </summary>
    public partial class MatrixEditor : UserControl
    {
        public MatrixEditor()
        {
            InitializeComponent();
        }

        private void RenderEditor(int rows, int columns)
        {
            double[,] copy = null;

            if (Editor.Children.Count > 0)
            {
                int r = Editor.RowDefinitions.Count;
                int c = Editor.ColumnDefinitions.Count;
                copy = new double[r, c];
                foreach (TextBox tx in Editor.Children)
                {
                    r = Grid.GetRow(tx);
                    c = Grid.GetColumn(tx);
                    try
                    {
                        copy[r, c] = Convert.ToDouble(tx.Text);
                    }
                    catch (Exception)
                    {
                        copy[r, c] = 0;
                    }
                }
            }

            Editor.Children.Clear();
            Editor.ColumnDefinitions.Clear();
            Editor.RowDefinitions.Clear();

            for (int i = 0; i < rows; i++) Editor.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < columns; i++) Editor.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i=0; i<rows; i++)
            {
                for (int j=0; j<columns; j++)
                {
                    TextBox tx = new TextBox();
                    tx.MinWidth = 60;
                    tx.Margin = new Thickness(2);
                    Grid.SetRow(tx, i);
                    Grid.SetColumn(tx, j);

                    if (copy != null)
                    {
                        if (i < copy.GetLength(0) && j < copy.GetLength(1)) tx.Text = copy[i, j].ToString();
                    }
                    Editor.Children.Add(tx);
                }
            }
        }

        private void BtnCreate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RenderEditor((int)Rows.Value, (int)Columns.Value);
        }
    }
}
