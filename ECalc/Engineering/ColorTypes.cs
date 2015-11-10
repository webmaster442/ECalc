using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Engineering
{
    public class HSB
    {
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Brightness { get; set; }
    }

    public class HSL
    {
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Luminance { get; set; }
    }

    public class CMYK
    {
        public double Cyan { get; set; }
        public double Magenta { get; set; }
        public double Yellow { get; set; }
        public double Black { get; set; }
    }
}
