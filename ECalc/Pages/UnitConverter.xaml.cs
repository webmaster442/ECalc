using ECalc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for UnitConverterPage.xaml
    /// </summary>
    internal partial class UnitConverterPage : UserControl
    {
        private UnitConverter _conv;
        private Unit[] _source, _dest;
        private bool _loaded;
        private string _currentheader;

        public UnitConverterPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _conv = new UnitConverter();
            _conv.FillTreeview(TreeSource);
            _currentheader = "";
            _loaded = true;
        }

        private void TreeSource_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeSource.SelectedItem == null) TblInput.Text = "Input:";
            else TblInput.Text = string.Format("Input {0}:", (TreeSource.SelectedItem as TreeViewItem).Header);
            _source = _conv.TreeviewItemToCategory((TreeViewItem)TreeSource.SelectedItem);


            object parent = ((TreeViewItem)TreeSource.SelectedItem).Parent;

            if (parent != null && (parent is TreeViewItem))
            {
                string header = ((TreeViewItem)parent).Header.ToString();
                if (_currentheader != header)
                {
                    TreeDestination.Items.Clear();
                    TreeDestination.Items.Add(_conv.ListCategory(_source, header, true));
                    _currentheader = header;
                }
            }

            Calculate();
        }

        private void TreeDestination_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeDestination.SelectedItem == null)
            {
                TblOutput.Text = "Output:";
                return;
            }
            else TblOutput.Text = string.Format("Output {0}:", (TreeDestination.SelectedItem as TreeViewItem).Header);
            _dest = _conv.TreeviewItemToCategory((TreeViewItem)TreeDestination.SelectedItem);
            Calculate();
        }

        private double ParsePrefix()
        {
            string item = ((ComboBoxItem)CbPrefix.SelectedItem).Content.ToString();
            switch (item)
            {
                case "yotta":
                    return Math.Pow(10, 24);    
                case "zetta":
                    return Math.Pow(10, 21);
                case "exa":
                    return Math.Pow(10, 18);
                case "peta":
                    return Math.Pow(10, 15);
                case "terra":
                    return Math.Pow(10, 12);
                case "giga":
                    return Math.Pow(10, 9);
                case "mega": 
                    return Math.Pow(10, 6);
                case "kilo": 
                    return 1000;
                case "hekto":
                    return 100;
                case "deka":
                    return 10;
                case "-":
                    return 1;
                case "deci":
                    return Math.Pow(10, -1);
                case "centi": 
                    return Math.Pow(10, -2);
                case "milli": 
                    return Math.Pow(10, -3);
                case "mikro": 
                    return Math.Pow(10, -6);
                case "nano": 
                    return Math.Pow(10, -9);
                case "pico": 
                    return Math.Pow(10, -12);
                case "femto": 
                    return Math.Pow(10, -15);
                case "atto": 
                    return Math.Pow(10, -18);
                case "zepto": 
                    return Math.Pow(10, -21);
                case "yocto": 
                    return Math.Pow(10, -24);
                default:
                    return 1;
            }
        }

        private void Calculate()
        {
            if (!_loaded) return;
            double outval = 0;
            double inval = 0;
            bool res = double.TryParse(TbInput.Text, out inval);
            if (!res) outval = 0;
            inval *= ParsePrefix();
            if (_source != _dest)
            { 
                TbOutput.Value = double.NaN;
                return;
            }
            if (_source == null || _dest == null)
            {
                TbOutput.Value = double.NaN;
                return;
            }
            outval =  _conv.Convert((TreeSource.SelectedItem as TreeViewItem).Header.ToString(), (TreeDestination.SelectedItem as TreeViewItem).Header.ToString(), _dest, inval);
            TbOutput.Value = outval;
        }

        private void TbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Calculate();
        }

        private void CbPrefix_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }
    }
}
