using ECalc.Classes;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for BasicKeyPad.xaml
    /// </summary>
    public partial class BasicKeyPad : UserControl
    {
        public BasicKeyPad()
        {
            InitializeComponent();
        }

        public TextBox Target { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var content = ((Button)sender).Content.ToString();
            if (Target != null)
            {
                Target.Text += content;
            }
        }
    }
}
