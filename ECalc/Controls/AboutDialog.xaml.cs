using System.Windows.Documents;
using MahApps.Metro.Controls.Dialogs;
using WPFLib.Extensions;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class AboutDialog : CustomDialog
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var links = this.FindChildren<Hyperlink>();
            foreach (var link in links)
            {
                link.Click += Link_Click;
            }
        }

        private void Link_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            System.Diagnostics.Process.Start(link.NavigateUri.ToString());
        }

        private async void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var main = (MainWindow)App.Current.MainWindow;
            await main.HideMetroDialogAsync(this);
        }
    }
}
