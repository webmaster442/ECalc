using ECalc.Classes;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for KeyPad.xaml
    /// </summary>
    public partial class KeyPad : UserControl, IMemManager
    {
        private static ObservableCollection<MemoryItem> _memory;

        public KeyPad()
        {
            InitializeComponent();
            _memory = new ObservableCollection<MemoryItem>();
            MemList.ItemsSource = _memory;
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

        private void BtnMem_Click(object sender, RoutedEventArgs e)
        {
            Memory.Visibility = System.Windows.Visibility.Visible;
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

        public object GetItem(string name)
        {
            var query = from i in _memory where string.Compare(name, i.Name) == 0 select i;
            var result = query.FirstOrDefault();
            if (result == null) return result;
            else return result.Value;
        }
    }
}
