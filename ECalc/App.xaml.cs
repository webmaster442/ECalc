using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ECalc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception data = (Exception)e.ExceptionObject;
            var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "EcalcCrashLog.txt");
            using (var text = File.CreateText(filename))
            {
                text.WriteLine("--------------------------------------------");
                text.WriteLine("Ecalc has crashed. Sorry for the inconvenience");
                text.WriteLine("Please forward this log's contents to the projects issues section:");
                text.WriteLine("https://github.com/webmaster442/ECalc/issues");
                text.WriteLine("--------------------------------------------");
                text.WriteLine("Crash timestamp: {0}", DateTime.Now);
                text.WriteLine("OS Version: {0}", Environment.OSVersion);
                text.WriteLine("OS, Process is 64 bit: {0}, {1}", Environment.Is64BitOperatingSystem, Environment.Is64BitProcess);
                text.WriteLine("--------------------------------------------");
                text.WriteLine("Exception Message & Source:\r\n{0}\r\n\r\n{1}", data.Message, data.Source);
                text.WriteLine("--------------------------------------------");
                text.WriteLine("Target Site:\r\n{0}", data.TargetSite);
                text.WriteLine("--------------------------------------------");
                text.WriteLine("Stack Trace:\r\n{0}", data.StackTrace);
                text.WriteLine("--------------------------------------------");
            }
            Process p = new Process();
            p.StartInfo.FileName = filename;
            p.Start();
        }
    }
}
