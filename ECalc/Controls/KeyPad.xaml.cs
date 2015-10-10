using ECalc.Classes;
using System;
using System.Collections.ObjectModel;
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
        private static ObservableCollection<MemoryItem> _memory;
        private ConstantList _constants;
        private bool _loaded;

        public KeyPad()
        {
            InitializeComponent();
            _memory = new ObservableCollection<MemoryItem>();
            _constants = new ConstantList();
            MemList.ItemsSource = _memory;
            ConstList.ItemsSource = _constants;
            DecimalSeperator.Content = Engine.DecimalSeperator;
        }

        public event RoutedEventHandler ExecuteClicked;

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

        private void BtnMemCancel_Click(object sender, RoutedEventArgs e)
        {
            Memory.Visibility = System.Windows.Visibility.Collapsed;
            Keys.Visibility = System.Windows.Visibility.Visible;
        }

        private void BtnConstCancel_Click(object sender, RoutedEventArgs e)
        {
            Constants.Visibility = System.Windows.Visibility.Collapsed;
            Keys.Visibility = System.Windows.Visibility.Visible;
        }

        private void BtnMem_Click(object sender, RoutedEventArgs e)
        {
            Memory.Visibility = System.Windows.Visibility.Visible;
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
                    default:
                        ButtonClicked(sender, new StringEventArgs(content.Substring(0, 1)));
                        break;
                }
            }
        }

        private void BtnMemInsert_Click(object sender, RoutedEventArgs e)
        {
             if (ButtonClicked != null)
             {
                 if (MemList.SelectedIndex > -1)
                 {
                     var content = _memory[MemList.SelectedIndex].Name;

                     if (Memory.Visibility == System.Windows.Visibility.Visible)
                     {
                         Memory.Visibility = System.Windows.Visibility.Collapsed;
                         Keys.Visibility = System.Windows.Visibility.Visible;
                     }

                     ButtonClicked(sender, new StringEventArgs(content));
                 }
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

        private void BtnMemAdd_Click(object sender, RoutedEventArgs e)
        {
            _memory.Add(new MemoryItem(Engine.Ans));
            if (Memory.Visibility == System.Windows.Visibility.Visible)
            {
                Memory.Visibility = System.Windows.Visibility.Collapsed;
                Keys.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void BtnMemReplace_Click(object sender, RoutedEventArgs e)
        {
            if (MemList.SelectedIndex > -1)
            {
                var item = _memory[MemList.SelectedIndex];
                _memory[MemList.SelectedIndex] = new MemoryItem(item.Name, Engine.Ans);
            }
            if (Memory.Visibility == System.Windows.Visibility.Visible)
            {
                Memory.Visibility = System.Windows.Visibility.Collapsed;
                Keys.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void BtnMemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MemList.SelectedIndex > -1)
            {
                _memory.RemoveAt(MemList.SelectedIndex);
            }
        }

        #region IMemManager
        /// <summary>
        /// Gets the value of a register item
        /// </summary>
        /// <param name="name">item to get</param>
        /// <returns>the value of the item</returns>
        public object GetItem(string name)
        {
            if (name.StartsWith("$"))
            {
                if (name == "$ans") return Engine.Ans;
                var query = from i in _memory where string.Compare(name, i.Name) == 0 select i;
                var result = query.FirstOrDefault();
                if (result == null) return null;
                else return result.Value;
            }
            else
            {
                return ConstantDB.Lookup(name);
            }
        }

        /// <summary>
        /// Set an item with name
        /// </summary>
        /// <param name="name">name of variable</param>
        /// <param name="value">vallue of variable</param>
        public void SetItem(string name, object value)
        {
            if (double.IsNaN(ConstantDB.Lookup(name)))
            {
                if (name == "$ans") return;
                var query = (from i in _memory where string.Compare(name, i.Name) == 0 select i).FirstOrDefault();
                if (query == null) _memory.Add(new MemoryItem(name, value));
                else
                {
                    int index = _memory.IndexOf(query);
                    _memory[index].Value = value;
                }
            }
        }

        /// <summary>
        /// Lists register names
        /// </summary>
        /// <param name="query">query string. If null or empty all registers will be returned</param>
        /// <returns>An array of register names</returns>
        public string[] ListRegisters(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return (from i in _memory select i.Name).ToArray();
            }
            else
            {
                var q = from i in _memory where i.Name.StartsWith(query) select i.Name;
                return q.ToArray();
            }
        }
        #endregion

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded) return;
            ConstantCategory cat = ConstantCategory.Mathematical;
            Enum.TryParse<ConstantCategory>(CbSelector.SelectedItem.ToString(), out cat);
            _constants.Category = cat;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
        }
    }
}
