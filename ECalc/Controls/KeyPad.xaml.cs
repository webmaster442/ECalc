using ECalc.Classes;
using ECalc.IronPythonEngine;
using System;
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
        }

        public event RoutedEventHandler ExecuteClicked;
        public event RoutedEventHandler FromExpressionClicked;
        public event StringEventHandler ButtonClicked;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Prefixes.Visibility = Visibility.Collapsed;
            Keys.Visibility = Visibility.Visible;
        }

        private void Exponent_Click(object sender, RoutedEventArgs e)
        {
            Prefixes.Visibility = Visibility.Visible;
            Keys.Visibility = Visibility.Collapsed;
        }

        private void BtnConstCancel_Click(object sender, RoutedEventArgs e)
        {
            Constants.Visibility = Visibility.Collapsed;
            Keys.Visibility = Visibility.Visible;
        }

        private void BtnMem_Click(object sender, RoutedEventArgs e)
        {
            MemMan.Visibility = Visibility.Visible;
            Keys.Visibility = Visibility.Collapsed;
        }

        private void BtnCnst_Click(object sender, RoutedEventArgs e)
        {
            Constants.Visibility = Visibility.Visible;
            Keys.Visibility = Visibility.Collapsed;
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            ExecuteClicked?.Invoke(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null)
            {
                var content = ((Button)sender).Content.ToString();

                if (Prefixes.Visibility == Visibility.Visible)
                {
                    Prefixes.Visibility = Visibility.Collapsed;
                    Keys.Visibility = Visibility.Visible;
                }

                switch (content)
                {
                    case "mod":
                        ButtonClicked(sender, new StringEventArgs("%"));
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

        public enum Views
        {
            Keys,
            MemMan,
            Prefixes,
            Constants
        }

        public void SwitchTo(Views view)
        {
            Keys.Visibility = Visibility.Visible;
            MemMan.Visibility = Visibility.Collapsed;
            Constants.Visibility = Visibility.Collapsed;
            Prefixes.Visibility = Visibility.Collapsed;
            switch (view)
            {
                case Views.Constants:
                    Constants.Visibility = Visibility.Visible;
                    break;
                case Views.MemMan:
                    MemMan.Visibility = Visibility.Visible;
                    break;
                case Views.Prefixes:
                    Prefixes.Visibility = Visibility.Visible;
                    break;
                default:
                    return;
            }
        }

        private void MemMan_CancelClicked(object sender, RoutedEventArgs e)
        {
            MemMan.Visibility = Visibility.Collapsed;
            Keys.Visibility = Visibility.Visible;
        }

        private void MemMan_InsertClicked(object sender, StringEventArgs e)
        {
            if (ButtonClicked != null)
            {
                if (MemMan.Visibility == Visibility.Visible)
                {
                    MemMan.Visibility = Visibility.Collapsed;
                    Keys.Visibility = Visibility.Visible;
                }
                ButtonClicked(sender, new StringEventArgs(string.Format("Var(\'{0}\')", e.Text)));
                //ButtonClicked(sender, e);
            }
        }

        private void MemMan_FromExpressionClick(object sender, RoutedEventArgs e)
        {
            if (FromExpressionClicked != null)
            {
                if (MemMan.Visibility == Visibility.Visible)
                {
                    MemMan.Visibility = Visibility.Collapsed;
                    Keys.Visibility = Visibility.Visible;
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
                    if (Constants.Visibility == Visibility.Visible)
                    {
                        Constants.Visibility = Visibility.Collapsed;
                        Keys.Visibility = Visibility.Visible;
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
            return MemMan.GetItem(name);
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

        public void Hybernate()
        {
            MemMan.Hybernate();
        }

        public void Restore()
        {
            MemMan.Restore();
        }
    }
}
