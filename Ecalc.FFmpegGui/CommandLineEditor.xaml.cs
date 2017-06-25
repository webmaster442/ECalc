using System.Windows;
using System.Windows.Controls;

namespace Ecalc.FFmpegGui
{
    /// <summary>
    /// Interaction logic for CommandLineEditor.xaml
    /// </summary>
    public partial class CommandLineEditor : UserControl
    {
        public CommandLineEditor()
        {
            InitializeComponent();
        }

        private void ClearParams(object sender, RoutedEventArgs e)
        {
            Parameters.Text = "";
        }

        private void Trim(object sender, RoutedEventArgs e)
        {
            Parameters.Text = Parameters.Text.Trim();
        }


        private void InsertNewParam(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            var text = menu.ToolTip?.ToString();
            if (string.IsNullOrEmpty(text)) return;
            Parameters.Text = Parameters.Text.Insert(Parameters.CaretIndex, " " + text);
        }

        public string CommandLine
        {
            get { return Parameters.Text; }
            set { Parameters.Text = value; }
        }

        public string FFMPegPath
        {
            get { return FFMpegPathBox.SelectedFile; }
            set { FFMpegPathBox.SelectedFile = value; }
        }
    }
}
