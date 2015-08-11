using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;

namespace ECalc.Controls.Special
{
    /// <summary>
    /// Interaction logic for IpInput.xaml
    /// </summary>
    public partial class IpInput : UserControl
    {
        public IpInput()
        {
            InitializeComponent();
        }

        public IPAddress IP
        {
            get
            {
                IPAddress addr = new IPAddress(new byte[] { Convert.ToByte(Octet0.Value),
                                                            Convert.ToByte(Octet1.Value),
                                                            Convert.ToByte(Octet2.Value),
                                                            Convert.ToByte(Octet3.Value) });
                return addr;
            }
            set
            {
                if (value.AddressFamily != AddressFamily.InterNetwork) return;
                byte[] bytes = value.GetAddressBytes();
                Octet0.Value = bytes[0];
                Octet1.Value = bytes[1];
                Octet2.Value = bytes[2];
                Octet3.Value = bytes[3];
            }
        }
    }
}
