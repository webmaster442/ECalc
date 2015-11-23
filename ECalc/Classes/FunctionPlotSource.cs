using System.Collections.ObjectModel;

namespace ECalc.Classes
{
    public class FunctionSettings
    {
        public string Code { get; set; }
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
    }

    class FunctionPlotSource: ObservableCollection<FunctionSettings>
    {
        public FunctionPlotSource(): base()
        {
            this.Add(new FunctionSettings
            {
                Code = "Sin($x)",
                XMin = -180,
                XMax = 360,
                YMin = -1.05,
                YMax = 1.05
            });

            this.Add(new FunctionSettings
            {
                Code = "Cos($x)",
                XMin = -180,
                XMax = 360,
                YMin = -1.05,
                YMax = 1.05
            });

            this.Add(new FunctionSettings
            {
                Code = "Tan($x)",
                XMin = -180,
                XMax = 360,
                YMin = -100,
                YMax = 100
            });

            this.Add(new FunctionSettings
            {
                Code = "Ctg($x)",
                XMin = -180,
                XMax = 360,
                YMin = -100,
                YMax = 100
            });

            this.Add(new FunctionSettings
            {
                Code = "Sec($x)",
                XMin = -180,
                XMax = 360,
                YMin = -100,
                YMax = 100
            });

            this.Add(new FunctionSettings
            {
                Code = "Cosec($x)",
                XMin = -180,
                XMax = 360,
                YMin = -100,
                YMax = 100
            });

            this.Add(new FunctionSettings
            {
                Code = "Pow($x;2)",
                XMin = -15,
                XMax = 15,
                YMin = -5,
                YMax = 200
            });

            this.Add(new FunctionSettings
            {
                Code = "Pow($x;3)",
                XMin = -15,
                XMax = 15,
                YMin = -200,
                YMax = 200
            });

            this.Add(new FunctionSettings
            {
                Code = "Pow(2; $x)",
                XMin = -5,
                XMax = 10,
                YMin = -5,
                YMax = 200
            });
        }
    }
}
