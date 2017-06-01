using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for CopyPasteDialog.xaml
    /// </summary>
    public partial class CopyPasteDialog : CustomDialog
    {
        public CopyPasteDialog()
        {
            InitializeComponent();
            ListPastes.ItemsSource = App.AppClipboard;
        }

        private async void PART_Paste_Click(object sender, RoutedEventArgs e)
        {

            var main = (MainWindow)Application.Current.MainWindow;
            await main.HideMetroDialogAsync(this);
        }

        private void PART_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void PART_Cancel_Click(object sender, RoutedEventArgs e)
        {
            var main = (MainWindow)Application.Current.MainWindow;
            await main.HideMetroDialogAsync(this);
        }
    }
}
