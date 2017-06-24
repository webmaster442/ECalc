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
        Presets Presets;

        public FFMpegGui()
        {
            InitializeComponent();
            Presets = new Presets();
            PresetList.ItemsSource = Presets;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OutputNamer.SetFiles(InputFiles.Files);
            if (PresetList.SelectedIndex >=0)
            {
                var preset = Presets[PresetList.SelectedIndex];
                OutputNamer.SetPreferedExtension(preset.Extension);
                CmdEditor.CommandLine = preset.CommandLine;
            }
        }
    }
}
