using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for StatInput.xaml
    /// </summary>
    public partial class StatInput : UserControl
    {
        public StatInput()
        {
            InitializeComponent();
            Keypad.Target = TbDisplay;
        }

        public event RoutedEventHandler AddClick;

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (TbDisplay.Text.Length - 1 > 0)
            {
                string s = TbDisplay.Text.Substring(0, TbDisplay.Text.Length - 1);
                TbDisplay.Text = s;
            }
            else TbDisplay.Text = "";
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TbDisplay.Text = "";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddClick != null)
            {
                AddClick(sender, e);
            }
        }

        public double Number
        {
            get { return Convert.ToDouble(TbDisplay.Text); }
            set { TbDisplay.Text = value.ToString(); }
        }
    }
}
