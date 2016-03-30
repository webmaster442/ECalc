using ECalc.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ProgramEditor.xaml
    /// </summary>
    public partial class ProgramEditor : UserControl
    {
        public ProgramEditor()
        {
            InitializeComponent();
            UpdateBinding();
        }

        private void UpdateBinding()
        {
            LbFunctions.ItemsSource = null;
            LbFunctions.ItemsSource = Engine.UserFunctions;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbName.Text))
            {
                MainWindow.ErrorDialog("Name can't be empty");
                return;
            }
            if (string.IsNullOrEmpty(TbCode.Text))
            {
                MainWindow.ErrorDialog("Code can't be empty");
                return;
            }

            var f = new UserFuntion
            {
                Name = TbName.Text,
                Commands = TbCode.Text,
                ArgCount = (int)NumArgCount.Value
            };
            Engine.UserFunctions.Add(f);
            UpdateBinding();
        }

        private void LoadFunctionEditor(UserFuntion f)
        {
            TbName.Text = f.Name;
            NumArgCount.Value = f.ArgCount;
            TbCode.Text = f.Commands;
        }

        private void LbFunctions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbFunctions.SelectedItem == null) return;
            int index = LbFunctions.SelectedIndex;
            LoadFunctionEditor(Engine.UserFunctions[index]);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LbFunctions.SelectedItem == null) return;
            int index = LbFunctions.SelectedIndex;
            Engine.UserFunctions.RemoveAt(index);
            UpdateBinding();
        }
    }
}
