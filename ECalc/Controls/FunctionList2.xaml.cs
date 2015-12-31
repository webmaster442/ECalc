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
using ECalc.Classes;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for FunctionList2.xaml
    /// </summary>
    public partial class FunctionList2 : UserControl
    {
        public FunctionList2()
        {
            InitializeComponent();
        }

        internal IFunction Funtions
        {
            get;
            set;
        }
    }
}
