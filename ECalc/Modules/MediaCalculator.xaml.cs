using ECalc.Classes;
using System.Windows;
using System.Windows.Controls;

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
                case 2:
                    var persec = ((VidRate.Value / 8) * 1024) + ((AudioRate.Value / 8) * 1024);
                    filesize = VidLenght.TimeSpan.TotalSeconds * persec;
                    break;
            }
            TbFileSize.Text = Helpers.DivideToFileSize(filesize);
        }
    }
}
