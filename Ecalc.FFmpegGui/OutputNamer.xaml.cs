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
using AppLib.Common.Extensions;

namespace Ecalc.FFmpegGui
{
    /// <summary>
    /// Interaction logic for OutputNamer.xaml
    /// </summary>
    public partial class OutputNamer : UserControl
    {
        public OutputNamer()
        {
            InitializeComponent();
        }

        private OutputNamerViewModel ViewModel
        {
            get { return (OutputNamerViewModel)DataContext; }
        }

        public void SetFiles(IEnumerable<string> s)
        {
            ViewModel.Inputs.Clear();
            ViewModel.Inputs.AddRange(s);
        }

        public void SetPreferedExtension(string s)
        {
            ViewModel.Extension = s;
        }
    }
}
