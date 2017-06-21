using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

    }
}
