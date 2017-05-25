using AppLib.WPF.Controls;
using System.Windows;

namespace ECalc.ExcelInterop
{
    /// <summary>
    /// Interaction logic for ExcelInteropControl.xaml
    /// </summary>
    public partial class ExcelInteropControl : DockedWindow
    {
        public ExcelInteropControl()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.DisconnectCommand.Execute(null);
        }

        private void DockedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DockTarget = Application.Current.MainWindow;
            this.DockDirection = DockDirections.Right;
        }
    }
}
