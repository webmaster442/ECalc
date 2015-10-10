using System;
using System.Windows;
using System.Windows.Controls;
using ECalc.Maths;
using ECalc.Classes;

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

        public event RoutedEventHandler RegisterComboOpened;

        public event StringEventHandler LoadClicked;

        /// <summary>
        /// Renders the editor
        /// </summary>
        /// <param name="rows">Number of desired rows</param>
        /// <param name="columns">Number of desired columns</param>
        private void RenderEditor(int rows, int columns)
        {
            string[,] copy = null;

            if (Editor.Children.Count > 0)
            {
                int r = Editor.RowDefinitions.Count;
                int c = Editor.ColumnDefinitions.Count;
                copy = new string[r, c];
                foreach (TextBox tx in Editor.Children)
                {
                    r = Grid.GetRow(tx);
                    c = Grid.GetColumn(tx);
                    if (!string.IsNullOrEmpty(tx.Text)) copy[r, c] = tx.Text;
                }
            }

            Editor.Children.Clear();
            Editor.ColumnDefinitions.Clear();
            Editor.RowDefinitions.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

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
                        else tx.Text = "0";
                    }
                    else tx.Text = "0";
                    Editor.Children.Add(tx);
                }
            }
        }

        public void RenderEditor(DoubleMatrix matrix)
        {
            Editor.Children.Clear();
            Editor.RowDefinitions.Clear();
            Editor.ColumnDefinitions.Clear();

            for (int i = 0; i < matrix.Rows; i++) Editor.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < matrix.Columns; i++) Editor.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    TextBox tx = new TextBox();
                    tx.MinWidth = 60;
                    tx.Margin = new Thickness(2);
                    Grid.SetRow(tx, i);
                    Grid.SetColumn(tx, j);
                    tx.Text = matrix[i, j].ToString();
                    Editor.Children.Add(tx);
                }
            }
        }

        public DoubleMatrix GetMatrix()
        {
            DoubleMatrix m = new DoubleMatrix(Editor.RowDefinitions.Count, Editor.ColumnDefinitions.Count);
            int i = 0;
            int j = 0;
            foreach (TextBox tx in Editor.Children)
            {
                i = Grid.GetRow(tx);
                j = Grid.GetColumn(tx);
                m[i, j] = Convert.ToDouble(tx.Text);
            }
            return m;
        }

        private void BtnCreate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RenderEditor((int)Rows.Value, (int)Columns.Value);
            PopupCreate.IsOpen = false;
        }

        private void BtnCreatePopup_Click(object sender, RoutedEventArgs e)
        {
            PopupCreate.IsOpen = true;
            PopupLoad.IsOpen = false;
        }

        private void RegisterCombo_DropDownOpened(object sender, EventArgs e)
        {
            if (RegisterComboOpened != null) RegisterComboOpened(sender, null);
        }

        private void BtnLoadPopup_Click(object sender, RoutedEventArgs e)
        {
            PopupCreate.IsOpen = false;
            PopupLoad.IsOpen = true;
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (LoadClicked != null) LoadClicked(sender, new StringEventArgs(RegisterCombo.SelectedItem.ToString()));
        }
    }
}
