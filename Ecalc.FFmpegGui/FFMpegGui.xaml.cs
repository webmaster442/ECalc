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
    /// Interaction logic for FFMpegGui.xaml
    /// </summary>
    public partial class FFMpegGui : UserControl
    {
        private Presets _presets;

        public FFMpegGui()
        {
            InitializeComponent();
            _presets = new Presets();
            PresetsList.ItemsSource = _presets;
        }

        private void PresetsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PresetsList.SelectedIndex >= 0)
            {
                var key = (KeyValuePair<string, string>)PresetsList.SelectedItem;
                CmdLineEditor.CommandLine = key.Value;
            }
        }
    }
}
