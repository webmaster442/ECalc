using MahApps.Metro.Controls.Dialogs;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for MultiLineResultDialog.xaml
    /// </summary>
    public partial class MultiLineResultDialog : CustomDialog
    {
        public MultiLineResultDialog()
        {
            InitializeComponent();
        }

        private async void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var main = (MainWindow)App.Current.MainWindow;
            await main.HideMetroDialogAsync(this);
        }

        public string Text
        {
            get { return TbResult.Text; }
            set { TbResult.Text = value; }
        }
    }
}
