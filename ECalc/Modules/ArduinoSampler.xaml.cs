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
using System.IO.Ports;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for ArduinoSampler.xaml
    /// </summary>
    public partial class ArduinoSampler : UserControl
    {
        private double _yscale;
        private double _xscale;
        private double _max;
        private double _min;
        private double _samples;
        private double _average;
        private int _visiblepoints;
        private SerialPort _port;

        public ArduinoSampler()
        {
            InitializeComponent();
        }

        private void InitGraph(int adcbits)
        {
            int adcmax = 1 << adcbits;
            _yscale = Graph.ActualHeight / adcmax;
            _xscale = Graph.ActualWidth / 500;
            _max = 0;
            _min = 0;
            _samples = 0;
            _average = 0;
            _visiblepoints = 0;
        }

        private void AddPoint(int value)
        {
            if (value > _max) _max = value;
            if (value < _min) _min = value;
            _samples += 1;
            _average = _average * (_samples - 1) / _samples + value / _samples;
            if (_visiblepoints > 500)
            {
                _visiblepoints = 0;
                Line.Points.Clear();
            }
            var p = new Point(_visiblepoints * _xscale, value * _yscale);
            Line.Points.Add(p);
            _visiblepoints++;
        }

        private void BtnOpenClose_Click(object sender, RoutedEventArgs e)
        {
            if (BtnOpenClose.IsChecked == true)
            {
                if (_port != null && _port.IsOpen)
                {
                    _port.Close();
                    _port = null;
                }
                BtnOpenClose.IsChecked = false;
            }
            else
            {
                var portselector = new AppLib.WPF.Dialogs.SerialPortDialog();
                portselector.SelectPortName(PortSelector.SelectedItem.ToString());
                if (portselector.ShowDialog() == true)
                {
                    _port = portselector.Port;
                }
            }
        }
    }
}
