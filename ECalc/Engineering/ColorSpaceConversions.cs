using System;
using System.Windows.Media;

namespace ECalc.Engineering
{
    internal static class ColorSpaceConversions
    {
        public static Color FromHSB(double Hue, double Saturation, double Brightness)
        {
            double r = 0;
            double g = 0;
            double b = 0;

            if (Saturation == 0) r = g = b = Brightness;
            else
            {
                // the color wheel consists of 6 sectors. Figure out which sector you're in.
                double sectorPos = Hue / 60.0;
                var sectorNumber = (int)(Math.Floor(sectorPos));
                // get the fractional part of the sector
                double fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the color. 
                double p = Brightness * (1.0 - Saturation);
                double q = Brightness * (1.0 - (Saturation * fractionalSector));
                double t = Brightness * (1.0 - (Saturation * (1 - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        r = Brightness;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = Brightness;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = Brightness;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = Brightness;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = Brightness;
                        break;
                    case 5:
                        r = Brightness;
                        g = p;
                        b = q;
                        break;
                }
            }

            return Color.FromRgb(
                Convert.ToByte(Double.Parse(String.Format("{0:0.00}", r * 255.0))),
                Convert.ToByte(Double.Parse(String.Format("{0:0.00}", g * 255.0))),
                Convert.ToByte(Double.Parse(String.Format("{0:0.00}", b * 255.0)))
                );
        }

        public static Color FromHSB(HSB hsb)
        {
            return FromHSB(hsb.Hue, hsb.Saturation, hsb.Brightness);
        }

        public static HSB ToHSB(Color input)
        {
            double r = ((double)input.R / 255.0);
            double g = ((double)input.G / 255.0);
            double b = ((double)input.B / 255.0);

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            double h = 0.0;
            if (max == r && g >= b)
            {
                if (max - min == 0) h = 0.0;
                else h = 60 * (g - b) / (max - min);
            }
            else if (max == r && g < b) h = 60 * (g - b) / (max - min) + 360;
            else if (max == g) h = 60 * (b - r) / (max - min) + 120;     
            else if (max == b) h = 60 * (r - g) / (max - min) + 240;
            double s = (max == 0) ? 0.0 : (1.0 - ((double)min / (double)max));

            return new HSB
            {
                Hue = h,
                Saturation = s,
                Brightness = (double)max
            };
        }

        public static Color FromCMYK(double c, double m, double y, double k)
        {
            byte red = Convert.ToByte((1.0 - c) * (1.0 - k) * 255.0);
            byte green = Convert.ToByte((1.0 - m) * (1.0 - k) * 255.0);
            byte blue = Convert.ToByte((1.0 - y) * (1.0 - k) * 255.0);

            return Color.FromRgb(red, green, blue);
        }

        public static Color FromCMYK(CMYK cmyk)
        {
            return FromCMYK(cmyk.Cyan, cmyk.Magenta, cmyk.Yellow, cmyk.Black);
        }

        public static CMYK ToCMYK(Color input)
        {
            double c = (double)(255 - input.R) / 255;
            double m = (double)(255 - input.G) / 255;
            double y = (double)(255 - input.B) / 255;

            var min = (double)Math.Min(c, Math.Min(m, y));
            if (min == 1.0)
            {
                return new CMYK
                {
                    Cyan = 0,
                    Magenta = 0,
                    Yellow = 0,
                    Black = 1
                };
            }
            else
            {
                return new CMYK
                {
                    Cyan = (c - min) / (1 - min),
                    Magenta = (m - min) / (1 - min),
                    Yellow = (y - min) / (1 - min),
                    Black = min
                };
            }
        }

        public static Color FromHSL(double h, double s, double l)
        {
            if (s == 0)
            {
                // achromatic color (gray scale)
                return Color.FromRgb(
                    Convert.ToByte(Double.Parse(String.Format("{0:0.00}", l * 255.0))),
                    Convert.ToByte(Double.Parse(String.Format("{0:0.00}", l * 255.0))),
                    Convert.ToByte(Double.Parse(String.Format("{0:0.00}", l * 255.0)))
                    );
            }
            else
            {
                double q = (l < 0.5) ? (l * (1.0 + s)) : (l + s - (l * s));
                double p = (2.0 * l) - q;

                double Hk = h / 360.0;
                double[] T = new double[3];
                T[0] = Hk + (1.0 / 3.0);    // Tr
                T[1] = Hk;              // Tb
                T[2] = Hk - (1.0 / 3.0);    // Tg

                for (int i = 0; i < 3; i++)
                {
                    if (T[i] < 0) T[i] += 1.0;
                    if (T[i] > 1) T[i] -= 1.0;

                    if ((T[i] * 6) < 1)
                    {
                        T[i] = p + ((q - p) * 6.0 * T[i]);
                    }
                    else if ((T[i] * 2.0) < 1) //(1.0/6.0)<=T[i] && T[i]<0.5
                    {
                        T[i] = q;
                    }
                    else if ((T[i] * 3.0) < 2) // 0.5<=T[i] && T[i]<(2.0/3.0)
                    {
                        T[i] = p + (q - p) * ((2.0 / 3.0) - T[i]) * 6.0;
                    }
                    else T[i] = p;
                }

                return Color.FromRgb(
                    Convert.ToByte(Double.Parse(String.Format("{0:0.00}", T[0] * 255.0))),
                    Convert.ToByte(Double.Parse(String.Format("{0:0.00}", T[1] * 255.0))),
                    Convert.ToByte(Double.Parse(String.Format("{0:0.00}", T[2] * 255.0)))
                    );
            }
        }

        public static Color FromHSL(HSL hsl)
        {
            return FromHSL(hsl.Hue, hsl.Saturation, hsl.Luminance);
        }

        public static HSL ToHSL(Color input)
        {
            double h = 0, s = 0, l = 0;

            // normalizes red-green-blue values
            double nRed = (double)input.R / 255.0;
            double nGreen = (double)input.G / 255.0;
            double nBlue = (double)input.B / 255.0;

            double max = Math.Max(nRed, Math.Max(nGreen, nBlue));
            double min = Math.Min(nRed, Math.Min(nGreen, nBlue));

            // hue
            if (max == min)
            {
                h = 0; // undefined
            }
            else if (max == nRed && nGreen >= nBlue)
            {
                h = 60.0 * (nGreen - nBlue) / (max - min);
            }
            else if (max == nRed && nGreen < nBlue)
            {
                h = 60.0 * (nGreen - nBlue) / (max - min) + 360.0;
            }
            else if (max == nGreen)
            {
                h = 60.0 * (nBlue - nRed) / (max - min) + 120.0;
            }
            else if (max == nBlue)
            {
                h = 60.0 * (nRed - nGreen) / (max - min) + 240.0;
            }

            // luminance
            l = (max + min) / 2.0;

            // saturation
            if (l == 0 || max == min)
            {
                s = 0;
            }
            else if (0 < l && l <= 0.5)
            {
                s = (max - min) / (max + min);
            }
            else if (l > 0.5)
            {
                s = (max - min) / (2 - (max + min)); //(max-min > 0)?
            }

            return new HSL
            {
                Hue = h,
                Saturation = s,
                Luminance = l
            };
        }

        public static Color FromYUV(double y, double u, double v)
        {

            byte Red = Convert.ToByte((y + 1.139837398373983740 * v) * 255);
            byte Green = Convert.ToByte((y - 0.3946517043589703515 * u - 0.5805986066674976801 * v) * 255);
            byte Blue = Convert.ToByte((y + 2.032110091743119266 * u) * 255);

            return Color.FromRgb(Red, Green, Blue);
        }

        public static Color FromYUV(YUV yuv)
        {
            return FromYUV(yuv.Y, yuv.U, yuv.V);
        }

        public static YUV ToYUV(Color c)
        {
            var yuv = new YUV();

            // normalizes red/green/blue values
            double nRed = (double)c.R / 255.0;
            double nGreen = (double)c.G / 255.0;
            double nBlue = (double)c.B / 255.0;

            // converts
            yuv.Y = 0.299 * nRed + 0.587 * nGreen + 0.114 * nBlue;
            yuv.U = -0.1471376975169300226 * nRed - 0.2888623024830699774 * nGreen + 0.436 * nBlue;
            yuv.V = 0.615 * nRed - 0.5149857346647646220 * nGreen - 0.1000142653352353780 * nBlue;

            return yuv;
        }
    }
}
