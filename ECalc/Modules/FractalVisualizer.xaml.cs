using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for FractalVisualizer.xaml
    /// </summary>
    public partial class FractalVisualizer : UserControl
    {
        public FractalVisualizer()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (Fractal == null) return;
            Point pan = new Point();
            pan.X = PanX.Value;
            pan.Y = PanY.Value;
            Fractal.Pan = pan;

            Point3D color = new Point3D();
            color.X = ColorX.Value;
            color.Y = ColorY.Value;
            color.Z = ColorZ.Value;
            Fractal.ColorScale = color;
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (Fractal == null) return;
            if (Mandelbrot.IsChecked == true) Fractal.Mode = 0;
            else if (Julia.IsChecked == true) Fractal.Mode = 1;
            else Fractal.Mode = 2;
        }
    }
}
