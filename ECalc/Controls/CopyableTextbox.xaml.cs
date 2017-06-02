using AppLib.Common.MessageHandler;
using ECalc.Classes;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for CopyableTextbox.xaml
    /// </summary>
    public partial class CopyableTextBox : UserControl
    {
        private PrefixDictionary _dict;

        public CopyableTextBox()
        {
            InitializeComponent();
            _dict = new PrefixDictionary();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(CopyableTextBox), new PropertyMetadata(0.0d, ValueChanged));

        public static readonly DependencyProperty ApendTextProperty =
            DependencyProperty.Register("ApendText", typeof(string), typeof(CopyableTextBox), new PropertyMetadata("", ValueChanged));

        public static readonly DependencyProperty PrefixesProperty =
            DependencyProperty.Register("Prefixes", typeof(bool), typeof(CopyableTextBox), new PropertyMetadata(true, ValueChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string ApendText
        {
            get { return (string)GetValue(ApendTextProperty); }
            set { SetValue(ApendTextProperty, value); }
        }

        public bool Prefixes
        {
            get { return (bool)GetValue(PrefixesProperty); }
            set { SetValue(PrefixesProperty, value); }
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as CopyableTextBox;
            if (sender != null)
            {
                if (sender.Prefixes)
                    sender.TbDisplay.Text = string.Format("{0} {1}", sender._dict.DivideToPrefix(sender.Value), sender.ApendText);
                else
                    sender.TbDisplay.Text = string.Format("{0} {1}", sender.Value, sender.ApendText);
            }
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            var us = new CultureInfo("en-US");
            Messager.Instance.SendMessage(new CopyPasteData(Value.ToString(us)));
        }
    }
}
