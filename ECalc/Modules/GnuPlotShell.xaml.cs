using ECalc.Classes;
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

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for GnuPlotShell.xaml
    /// </summary>
    public partial class GnuPlotShell : UserControl, IDisposable
    {
        private GnuPlotRunner _gnuplot;

        public GnuPlotShell()
        {
            InitializeComponent();
        }

        ~GnuPlotShell()
        {
            Dispose(true);
        }

        protected void Dispose(bool native)
        {
            if (_gnuplot != null)
            {
                _gnuplot.Kill();
                _gnuplot.Dispose();
                _gnuplot = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _gnuplot = new GnuPlotRunner();
                _gnuplot.Start();
            }
            catch (Exception ex)
            {
                TbOutput.Foreground = new SolidColorBrush(Colors.Red);
                TbOutput.Text = "Error:\r\n" + ex.Message;
            }
        }
    }
}
