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
    /// Interaction logic for NumSysKeyPad.xaml
    /// </summary>
    public partial class NumSysKeyPad : UserControl
    {
        public static readonly DependencyProperty NumSysProperty = DependencyProperty.Register("NumSys", typeof(int), typeof(NumSysKeyPad), new PropertyMetadata(10));

        public NumSysKeyPad()
        {
            InitializeComponent();
        }

        public int NumSys
        {
            get { return (int)GetValue(NumSysProperty); }
            set
            {
                if (value != 2 && value != 8 && value != 10 & value != 16) throw new ArgumentException("Value must be 2, 8, 10 or 16");
                SetValue(NumSysProperty, value);
                SetSystem(value);
            }
        }

        private void SetSystem(int system)
        {
            string[] enable = null;

            switch (system)
            {
                case 2:
                    enable = "0;1".Split(';');
                    break;
                case 8:
                    enable = "0;1;2;3;4;5;6;7".Split(';');
                    break;
                case 10:
                    enable = "0;1;2;3;4;5;6;7;8;9".Split(';');
                    break;
                case 16:
                    break;
            }

            foreach (Button b in KeyGrid.Children)
            {
                var content = (string)b.Content;
                if (enable == null) b.IsEnabled = true;
                else
                {
                    if (enable.Contains(content)) b.IsEnabled = true;
                    else b.IsEnabled = false;
                }
            }
        }

        public event StringEventHandler ButtonClicked;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null)
            {
                var content = ((Button)sender).Content.ToString();
                ButtonClicked(sender, new StringEventArgs(content));
            }
        }
    }
}
