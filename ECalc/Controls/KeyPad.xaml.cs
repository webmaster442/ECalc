using ECalc.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for KeyPad.xaml
    /// </summary>
    internal partial class KeyPad : UserControl, IMemManager
    {
        private ConstantList _constants;
        private bool _loaded;

        public KeyPad()
        {
            InitializeComponent();
            _constants = new ConstantList();
            ConstList.ItemsSource = _constants;
            DecimalSeperator.Content = " ";
        }

        public event RoutedEventHandler ExecuteClicked;
        public event RoutedEventHandler FromExpressionClicked;

        public event StringEventHandler ButtonClicked;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Prefixes.Visibility = System.Windows.Visibility.Collapsed;
            Keys.Visibility = System.Windows.Visibility.Visible;
        }

        private void Exponent_Click(object sender, RoutedEventArgs e)
        {
            Prefixes.Visibility = System.Windows.Visibility.Visible;
            Keys.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void BtnConstCancel_Click(object sender, RoutedEventArgs e)
        {
            Constants.Visibility = System.Windows.Visibility.Collapsed;
            Keys.Visibility = System.Windows.Visibility.Visible;
        }

        private void BtnMem_Click(object sender, RoutedEventArgs e)
        {
            MemMan.Visibility = System.Windows.Visibility.Visible;
            Keys.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void BtnCnst_Click(object sender, RoutedEventArgs e)
        {
            Constants.Visibility = System.Windows.Visibility.Visible;
            Keys.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            if (ExecuteClicked != null)
            {
                ExecuteClicked(sender, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null)
            {
                var content = ((Button)sender).Content.ToString();
                if (Prefixes.Visibility == System.Windows.Visibility.Visible)
                {
                    Prefixes.Visibility = System.Windows.Visibility.Collapsed;
                    Keys.Visibility = System.Windows.Visibility.Visible;
                }

                switch (content)
                {
                    case "mod":
                        ButtonClicked(sender, new StringEventArgs(content));
                        break;
                    case "micro":
                        ButtonClicked(sender, new StringEventArgs("u"));
                        break;
                    default:
                        ButtonClicked(sender, new StringEventArgs(content.Substring(0, 1)));
                        break;
                }
            }
        }

        private void MemMan_CancelClicked(object sender, RoutedEventArgs e)
        {
            MemMan.Visibility = System.Windows.Visibility.Collapsed;
            Keys.Visibility = System.Windows.Visibility.Visible;
        }

        private void MemMan_InsertClicked(object sender, StringEventArgs e)
        {
            if (ButtonClicked != null)
            {
                if (MemMan.Visibility == System.Windows.Visibility.Visible)
                {
                    MemMan.Visibility = System.Windows.Visibility.Collapsed;
                    Keys.Visibility = System.Windows.Visibility.Visible;
                }
                ButtonClicked(sender, e);
            }
        }

        private void MemMan_FromExpressionClick(object sender, RoutedEventArgs e)
        {
            if (FromExpressionClicked != null)
            {
                if (MemMan.Visibility == System.Windows.Visibility.Visible)
                {
                    MemMan.Visibility = System.Windows.Visibility.Collapsed;
                    Keys.Visibility = System.Windows.Visibility.Visible;
                }
                FromExpressionClicked(sender, e);
            }
        }

        private void BtnInsertConst_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null)
            {
                if (ConstList.SelectedIndex > -1)
                {
                    var content = _constants[ConstList.SelectedIndex].Name;
                    if (Constants.Visibility == System.Windows.Visibility.Visible)
                    {
                        Constants.Visibility = System.Windows.Visibility.Collapsed;
                        Keys.Visibility = System.Windows.Visibility.Visible;
                    }
                    ButtonClicked(sender, new StringEventArgs(content));
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded) return;
            ConstantCategory cat = ConstantCategory.Mathematical;
            bool res = Enum.TryParse<ConstantCategory>(CbSelector.SelectedItem.ToString(), out cat);
            if (!res) cat = ConstantCategory.Mathematical;
            _constants.Category = cat;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
        }

        public object GetItem(string name)
        {
            if (name.StartsWith("&"))
            {
                return ConstantDB.Lookup(name);
            }
            else return MemMan.GetItem(name);
        }

        public void PushTemp(object value)
        {
            MemMan.PushTemp(value);
        }

        public void ClearTemp()
        {
            MemMan.ClearTemp();
        }

        public string[] ListRegisters(string query)
        {
            return MemMan.ListRegisters(query);
        }

        public void SetItem(string name, object value)
        {
            MemMan.SetItem(name, value);
        }

        public void SetItem(object value)
        {
            MemMan.SetItem(value);
        }
    }
}
