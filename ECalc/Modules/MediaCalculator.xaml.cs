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
using ECalc.Classes;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ValueSelector.xaml
    /// </summary>
    public partial class MediaCalculator : UserControl
    {
        public MediaCalculator()
        {
            InitializeComponent();
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            double filesize = 0;
            switch (TabTool.SelectedIndex)
            {
                case 0:
                    double megapixels = ImgHeight.Value * ImgWidth.Value;
                    filesize = megapixels * ((double)BitsPerPixel.SelectedItem / 8);
                    break;
                case 1:
                    filesize = Lenght.TimeSpan.TotalSeconds * SampleRate.Value * (double)Channels.SelectedItem * ((double)BitDepth.SelectedItem / 8);
                    break;
            }
            TbFileSize.Text = Helpers.DivideToFileSize(filesize);
        }
    }
}
