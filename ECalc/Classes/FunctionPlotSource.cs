using System.Collections.ObjectModel;

namespace ECalc.Classes
{
    public class FunctionSettings
    {
        public string CodeY { get; set; }
        public string CodeX { get; set; }
        public bool Parametric { get; set; }
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public double tMin { get; set; }
        public double tMax { get; set; }
        public double tStep { get; set; }
        public string Title { get; set; }

        public FunctionSettings() { }

        public FunctionSettings(string code, double xmin, double xmax, double ymin, double ymax)
        {
            Parametric = false;
            Title = code;
            CodeY = code;
            XMin = xmin;
            XMax = xmax;
            YMin = ymin;
            YMax = ymax;
        }

        public FunctionSettings(string title, string codex, string codey, double xmin, double xmax, double ymin, double ymax,
                                double tmin, double tmax, double tstep) : this(codey, xmin, xmax, ymin, ymax)
        {
            Parametric = true;
            Title = title;
            CodeX = codex;
            tMin = tmin;
            tMax = tmax;
            tStep = tstep;
        }
    }

    class FunctionPlotSource: ObservableCollection<FunctionSettings>
    {
        public FunctionPlotSource()
        {
            this.Add(new FunctionSettings("Sin(Var('x'))", -180, 360, -1.05, 1.05));
            this.Add(new FunctionSettings("Cos(Var('x'))", -180, 360, -1.05, 1.05));
            this.Add(new FunctionSettings("Tan(Var('x'))", -180, 360, -100, 100));
            this.Add(new FunctionSettings("Ctg(Var('x'))", -180, 360, -100, 100));
            this.Add(new FunctionSettings("Sec(Var('x'))", -180, 360, -100, 100));
            this.Add(new FunctionSettings("Cosec(Var('x'))",-180,360,-100, 100));
            this.Add(new FunctionSettings("Pow(Var('x'),2)", -15, 15, -5, 200));
            this.Add(new FunctionSettings("Pow(Var('x'),3)", -15, 15, -200, 200));
            this.Add(new FunctionSettings("Pow(2; Var('x'))", -5, 10, -5, 200));
            this.Add(new FunctionSettings("Log(Var('x'), 2)", -0.06, 2, -12, 1.1));
            this.Add(new FunctionSettings("Elipse", "8*Sin(Var('t'))", "8*Cos(Var('t'))", -9, 9, -9, 9, 0, 360, 1));
            this.Add(new FunctionSettings("Spiral", "Sin(Var('t'))*Var('t')/16", "Cos(Var('t'))*Var('t')/16", -62, 52, -65, 63, 0, 1000, 1));
            this.Add(new FunctionSettings("Lissajous", "3*Sin(Var('t')*3)", "3*Sin(Var('t')*2)", -3.2, 3.1, -4, 4, 0, 1000, 1));
        }
    }
}
