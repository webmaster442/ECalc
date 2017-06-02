using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;
using AppLib.Common.MessageHandler;

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

        private async Task Close()
        {
            var main = (MainWindow)Application.Current.MainWindow;
            await main.HideMetroDialogAsync(this);
        }

        private async void PART_Paste_Click(object sender, RoutedEventArgs e)
        {
            if (ListPastes.SelectedIndex < 0) return;
            var data = App.AppClipboard[ListPastes.SelectedIndex];
            Messager.Instance.SendMessage(typeof(Display), data);
            await Close();
        }

        private void PART_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ListPastes.SelectedIndex < 0) return;
            App.AppClipboard.RemoveAt(ListPastes.SelectedIndex);
        }

        private async void PART_DeleteALL_Click(object sender, RoutedEventArgs e)
        {
            var q = MessageBox.Show("Delete all data", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (q == MessageBoxResult.Yes)
            {
                App.AppClipboard.Clear();
                await Close();
            }
        }

        private async void PART_Cancel_Click(object sender, RoutedEventArgs e)
        {
            await Close();
        }
    }
}
