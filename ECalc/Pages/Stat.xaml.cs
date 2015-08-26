using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for Stat.xaml
    /// </summary>
    public partial class Stat : UserControl
    {
        private ObservableCollection<double> _numbers;
        private Type _enumerablestat;

        public Stat()
        {
            InitializeComponent();
            _numbers = new ObservableCollection<double>();
            LbItems.ItemsSource = _numbers;
            RenderFunctionList();
            _enumerablestat = typeof(Maths.EnumerableStat);
        }

        private void RenderFunctionList()
        {
            Type enumerablestat = typeof(Maths.EnumerableStat);
            var methoods = from m in enumerablestat.GetMethods() where m.IsStatic == true && m.IsPublic == true && m.ReturnType == typeof(double) select m;
            Style style = this.FindResource("BtnStyle") as Style;
            foreach (var m in methoods)
            {
                Button b = new Button();
                b.Content = m.Name;
                b.Click += B_Click;
                b.Style = style;
                FunctionList.Children.Add(b);
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var name = ((Button)sender).Content.ToString();
                var methood = (from m in _enumerablestat.GetMethods() where m.Name == name select m).FirstOrDefault();
                if (methood != null)
                {
                    object result = methood.Invoke(null, new object[] { _numbers });
                    TbResult.Text = string.Format("{0}: {1}", methood.Name, result);
                }
            }
            catch (Exception ex)
            {
                TbResult.Text = "Error";
                MainWindow.ErrorDialog("An error happened. Collection is maybe empty");
            }
        }

        private void Input_AddClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _numbers.Add(Input.Number);
                Input.Clear();
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "Text files |*.txt";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var reader = File.OpenText(ofd.FileName))
                    {
                        string line = "";
                        do
                        {
                            line = reader.ReadLine();
                            try
                            {
                                double number = Convert.ToDouble(line);
                                _numbers.Add(number);
                            }
                            catch (Exception) { }
                        }
                        while (line != null);
                    }
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Filter = "Text files |*.txt";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var writer = File.CreateText(sfd.FileName))
                    {
                        foreach (var number in _numbers)
                        {
                            writer.WriteLine(number);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            _numbers.Clear();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LbItems.SelectedIndex > -1)
            {
                _numbers.RemoveAt(LbItems.SelectedIndex);
            }
        }
    }
}
