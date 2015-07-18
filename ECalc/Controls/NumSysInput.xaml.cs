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

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for NumSysInput.xaml
    /// </summary>
    public partial class NumSysInput : UserControl
    {
        public NumSysInput()
        {
            InitializeComponent();
        }

        public static DependencyProperty DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof(string), typeof(NumSysInput), new PropertyMetadata("0"));
        public static DependencyProperty CaptionTextProperty = DependencyProperty.Register("CaptionText", typeof(string), typeof(NumSysInput), new PropertyMetadata("Input System:"));
        public static DependencyProperty NumberSystemProperty = DependencyProperty.Register("NumberSystem", typeof(NumberSystem), typeof(NumSysInput), new PropertyMetadata(NumberSystem.Dec));

        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public string CaptionText
        {
            get { return (string)GetValue(CaptionTextProperty); }
            set { SetValue(CaptionTextProperty, value); }
        }

        public NumberSystem NumberSystem
        {
            get { return (NumberSystem)GetValue(NumberSystemProperty); }
            set { SetValue(NumberSystemProperty, value); }
        }
    }
}
