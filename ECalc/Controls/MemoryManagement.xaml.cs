using ECalc.Classes;
using ECalc.IronPythonEngine;
using ECalc.IronPythonEngine.Types;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for MemoryManagement.xaml
    /// </summary>
    public partial class MemoryManagement : UserControl, IMemManager
    {
        private ObservableCollection<MemoryItem> _memory;
        private EditNewVariableDialog _editdialog;
        private bool _designtime;

        public MemoryManagement()
        {
            InitializeComponent();
            _designtime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (_designtime) return;
            _memory = new ObservableCollection<MemoryItem>();
            _editdialog = new EditNewVariableDialog();
            _editdialog.SaveClicked += ed_SaveClicked;
            MemList.ItemsSource = _memory;
        }

        public event RoutedEventHandler CancelClicked;
        public event StringEventHandler InsertClicked;
        public event RoutedEventHandler FromExpressionClick;

        #region IMemManager
        /// <summary>
        /// Gets the value of a register item
        /// </summary>
        /// <param name="name">item to get</param>
        /// <returns>the value of the item</returns>
        public object GetItem(string name)
        {
            if (name == "$ans") return Engine.Ans;
            var query = from i in _memory where string.Compare(name, i.Name) == 0 select i;
            var result = query.FirstOrDefault();
            if (result == null) return null;
            else return result.Value;
        }

        /// <summary>
        /// Set an item with name
        /// </summary>
        /// <param name="name">name of variable</param>
        /// <param name="value">value of variable</param>
        public void SetItem(string name, object value)
        {
            Dispatcher.Invoke(() =>
            {
                if (name == "$ans") return;
                var query = (from i in _memory where string.Compare(name, i.Name) == 0 select i).FirstOrDefault();
                if (query == null) _memory.Add(new MemoryItem(name, value));
                else
                {
                    int index = _memory.IndexOf(query);
                    _memory[index].Value = value;
                }
            });
        }

        /// <summary>
        /// Set an item with default name
        /// </summary>
        /// <param name="value">value of variable</param>
        public void SetItem(object value)
        {
            _memory.Add(new MemoryItem(value));
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

        #region Serialization

        private void Serialize(string target)
        {
            using (var Stream = File.Create(target))
            {
                var xs = new XmlSerializer(typeof(MemoryItem[]));
                xs.Serialize(Stream, _memory.ToArray());
            }
        }

        private void DeSerialize(string target)
        {
            using (var Stream = File.OpenRead(target))
            {
                var xs = new XmlSerializer(typeof(MemoryItem[]));
                var mem = (MemoryItem[])xs.Deserialize(Stream);
                foreach (var m in mem) _memory.Add(m);
            }
        }

        #endregion

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (CancelClicked != null)
            {
                CancelClicked(sender, e);
            }
        }

        private void BtnFromLast_Click(object sender, RoutedEventArgs e)
        {
            _memory.Add(new MemoryItem(Engine.Ans));
        }

        private void BtnFromExpression_Click(object sender, RoutedEventArgs e)
        {
            if (FromExpressionClick != null)
            {
                FromExpressionClick(sender, e);
            }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (MemList.SelectedIndex < 0) return;
            var content = _memory[MemList.SelectedIndex].Name;
            if (InsertClicked != null)
            {
                InsertClicked(sender, new StringEventArgs(content));
            }
            MemList.SelectedIndex = -1;
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        { 
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Memory Files | *.mem";
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DeSerialize(ofd.FileName);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "Memory Files | *.mem";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Serialize(sfd.FileName);
            }
        }

        private void ed_SaveClicked(object sender, RoutedEventArgs e)
        {
            if (_editdialog.IsEditDialog)
            {
                switch (_editdialog.Index)
                {
                    case 0:
                        _memory[MemList.SelectedIndex].Value = _editdialog.Double;
                        break;
                    case 1:
                        _memory[MemList.SelectedIndex].Value = _editdialog.Complex;
                        break;
                    case 2:
                        _memory[MemList.SelectedIndex].Value = _editdialog.Fraction;
                        break;
                    case 3:
                        _memory[MemList.SelectedIndex].Value = _editdialog.Vector;
                        break;
                    case 4:
                        _memory[MemList.SelectedIndex].Value = _editdialog.Matrix;
                        break;
                    case 5:
                        _memory[MemList.SelectedIndex].Value = _editdialog.Set;
                        break;
                }
            }
            else
            {
                switch (_editdialog.Index)
                {
                    case 0:
                        _memory.Add(new MemoryItem(_editdialog.Double));
                        break;
                    case 1:
                        _memory.Add(new MemoryItem(_editdialog.Complex));
                        break;
                    case 2:
                        _memory.Add(new MemoryItem(_editdialog.Fraction));
                        break;
                    case 3:
                        _memory.Add(new MemoryItem(_editdialog.Vector));
                        break;
                    case 4:
                        _memory.Add(new MemoryItem(_editdialog.Matrix));
                        break;
                    case 5:
                        _memory.Add(new MemoryItem(_editdialog.Set));
                        break;
                }
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            _editdialog.IsEditDialog = false;
            MainWindow.ShowDialog(_editdialog);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            object o = _memory[MemList.SelectedIndex].Value;
            if (o is double) _editdialog.Double = (double)o;
            else if (o is Complex) _editdialog.Complex = (Complex)o;
            else if (o is Fraction) _editdialog.Fraction = (Fraction)o;
            else if (o is Matrix) _editdialog.Matrix = (Matrix)o;
            else if (o is Set) _editdialog.Set = (Set)o;
            _editdialog.IsEditDialog = true;
            MainWindow.ShowDialog(_editdialog);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MemList.SelectedIndex > -1)
            {
                _memory.RemoveAt(MemList.SelectedIndex);
            }
        }
    }
}
