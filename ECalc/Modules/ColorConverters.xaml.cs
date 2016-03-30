using ECalc.Engineering;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFLib.Controls;
using WPFLib.Extensions;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ColorConverters.xaml
    /// </summary>
    public partial class ColorConverters : UserControl
    {
        public ColorConverters()
        {
            InitializeComponent();
        }

        private enum ColorType
        {
            RGB, CMYK, HSL, HSB, YUV
        }

        private ColorType IdentifySender(object o)
        {
            EditableSlider slider = (EditableSlider)o;
            if (slider.Name.StartsWith("RGB")) return ColorType.RGB;
            else if (slider.Name.StartsWith("HSL")) return ColorType.HSL;
            else if (slider.Name.StartsWith("HSB")) return ColorType.HSB;
            else if (slider.Name.StartsWith("YUV")) return ColorType.YUV;
            else if (slider.Name.StartsWith("CMYK")) return ColorType.CMYK;
            else throw new ArgumentException("Invalid name");
        }

        private void ConvertColors(object sender, RoutedEventArgs e)
        {
            ColorType source = IdentifySender(sender);

            Color csource = Colors.Black;

            switch (source)
            {
                case ColorType.RGB:
                    csource = Color.FromRgb((byte)RGB_Red.Value, (byte)RGB_Green.Value, (byte)RGB_Blue.Value);
                    break;
                case ColorType.CMYK:
                    csource = ColorSpaceConversions.FromCMYK(CMYK_C.Value, CMYK_M.Value, CMYK_Y.Value, CMYK_K.Value);
                    break;
                case ColorType.HSL:
                    csource = ColorSpaceConversions.FromHSL(HSL_Hue.Value, HSL_Saturation.Value, HSL_Lumiance.Value);
                    break;
                case ColorType.HSB:
                    csource = ColorSpaceConversions.FromHSB(HSB_Hue.Value, HSB_Saturation.Value, HSB_Brightness.Value);
                    break;
                case ColorType.YUV:
                    csource = ColorSpaceConversions.FromYUV(YUV_Y.Value, YUV_U.Value, YUV_V.Value);
                    break;
            }

            var yuv = ColorSpaceConversions.ToYUV(csource);
            var hsl = ColorSpaceConversions.ToHSL(csource);
            var hsb = ColorSpaceConversions.ToHSB(csource);
            var cmyk = ColorSpaceConversions.ToCMYK(csource);

            if (source != ColorType.RGB)
            {
                RGB_Red.SetValue(csource.R);
                RGB_Green.SetValue(csource.G);
                RGB_Blue.SetValue(csource.B);
            }

            if (source != ColorType.CMYK)
            {
                CMYK_C.SetValue(cmyk.Cyan);
                CMYK_M.SetValue(cmyk.Magenta);
                CMYK_Y.SetValue(cmyk.Yellow);
                CMYK_K.SetValue(cmyk.Black);
            }

            if (source != ColorType.HSB)
            {
                HSB_Hue.SetValue(hsb.Hue);
                HSB_Saturation.SetValue(hsb.Saturation);
                HSB_Brightness.SetValue(hsb.Brightness);
            }

            if (source != ColorType.HSL)
            {
                HSL_Hue.SetValue(hsl.Hue);
                HSL_Saturation.SetValue(hsl.Saturation);
                HSL_Lumiance.SetValue(hsl.Luminance);
            }

            if (source != ColorType.YUV)
            {
                YUV_Y.SetValue(yuv.Y);
                YUV_U.SetValue(yuv.U);
                YUV_V.SetValue(yuv.V);
            }

            RectPreview.Fill = new SolidColorBrush(csource);
            var inv = Color.FromRgb((byte)(255 - csource.R), (byte)(255 - csource.G), (byte)(255 - csource.B));
            HexPreview.Foreground = new SolidColorBrush(inv);
            HexPreview.Text = string.Format("#{0:X2}{1:X2}{2:X2}", csource.R, csource.G, csource.B);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var sliders = this.FindChildren<EditableSlider>();
            foreach (var slider in sliders)
            {
                slider.ValueChanged += ConvertColors;
            }
        }
    }
}
