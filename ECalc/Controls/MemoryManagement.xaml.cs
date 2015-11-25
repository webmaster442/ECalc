using ECalc.Classes;
using ECalc.Maths;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for MemoryManagement.xaml
    /// </summary>
    public partial class MemoryManagement : UserControl, IMemManager
    {
        private ObservableCollection<MemoryItem> _memory;

        public MemoryManagement()
        {
            InitializeComponent();
            _memory = new ObservableCollection<MemoryItem>();
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
            if (name == "$ans") return;
            var query = (from i in _memory where string.Compare(name, i.Name) == 0 select i).FirstOrDefault();
            if (query == null) _memory.Add(new MemoryItem(name, value));
            else
            {
                int index = _memory.IndexOf(query);
                _memory[index].Value = value;
            }
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
            if (InsertClicked != null)
            {
                var content = _memory[MemList.SelectedIndex].Name;
                InsertClicked(sender, new StringEventArgs(content));
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        { 
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Memory Files | *.mem";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "Memory Files | *.mem";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            EditNewVariableDialog ed = new EditNewVariableDialog();
            MainWindow.ShowDialog(ed);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            object o = _memory[MemList.SelectedIndex].Value;
            EditNewVariableDialog ed = new EditNewVariableDialog();
            if (o is double) ed.Double = (double)o;
            else if (o is Complex) ed.Complex = (Complex)o;
            else if (o is Fraction) ed.Fraction = (Fraction)o;
            else if (o is DoubleMatrix) ed.Matrix = (DoubleMatrix)o;
            ed.IsEditDialog = true;
            MainWindow.ShowDialog(ed);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            _memory.RemoveAt(MemList.SelectedIndex);
        }
    }
}
