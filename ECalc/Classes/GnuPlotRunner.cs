using ECalc.Properties;
using System.Diagnostics;
using System.IO;

namespace ECalc.Classes
{
    internal class GnuPlotRunner: Process
    {
        public GnuPlotRunner() : base()
        {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = Path.Combine(Settings.Default.GNUPlotPath, "gnuplot.exe"),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
        }

        public new bool Start()
        {
            var result = base.Start();
            BeginOutputReadLine();
            return result;
        }
    }
}
