using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    internal partial class Display : UserControl
    {
        private ObservableCollection<string> _history;

        public Display()
        {
            InitializeComponent();
            _history = new ObservableCollection<string>();
            CbEdit.ItemsSource = _history;
        }

        /// <summary>
        /// Occures whern enter is pressed in the editor
        /// </summary>
        public event RoutedEventHandler ExecuteRequested;

        /// <summary>
        /// Calculator mode changed event
        /// </summary>
        public event StringEventHandler ModeChanged;

        public static readonly DependencyProperty EquationTextProperty = DependencyProperty.Register("EquationText", typeof(string), typeof(Display), new PropertyMetadata(""));

        public static readonly DependencyProperty ResultTextProperty = DependencyProperty.Register("ResultText", typeof(string), typeof(Display), new PropertyMetadata("0"));

        private void Reverse(StringBuilder text)
        {
            if (text.Length > 1)
            {
                int pivotPos = text.Length / 2;
                for (int i = 0; i < pivotPos; i++)
                {
                    int iRight = text.Length - (i + 1);
                    char rightChar = text[i];
                    char leftChar = text[iRight];
                    text[i] = leftChar;
                    text[iRight] = rightChar;
                }
            }
        }

        private string Group(string s)
        {
            if (s == "Error") return s;
            string[] parts = s.Split(',');
            StringBuilder sb = new StringBuilder();
            int j = 0;
            for (int i=parts[0].Length-1; i>=0; i--)
            {
                sb.Append(parts[0][i]);
                j++;
                if (j>2)
                {
                    j = 0;
                    sb.Append(Engine.NumberGroupSeparator);
                }
            }
            Reverse(sb);
            if (parts.Length > 1)
            {
                sb.Append(Engine.DecimalSeperator);
                sb.Append(parts[1]);
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Equation Text
        /// </summary>
        public string EquationText
        {
            get { return (string)GetValue(EquationTextProperty); }
            set { SetValue(EquationTextProperty, value); }
        }

        /// <summary>
        /// Result Text
        /// </summary>
        public string ResultText
        {
            get { return (string)GetValue(ResultTextProperty); }
            set 
            {
                if (value.Contains("\n")) MainDisplay.FontSize = 20;
                else MainDisplay.FontSize = 40;
                if (CbThousandGrouping.IsChecked == true)
                {
                    SetValue(ResultTextProperty, Group(value));
                }
                else SetValue(ResultTextProperty, value);
            }
        }

        /// <summary>
        /// Add current Equation text to the history
        /// </summary>
        public void AddToHistory()
        {
            int index = _history.IndexOf(EquationText);
            if (index > -1) _history.RemoveAt(index);
            _history.Add(EquationText);
            if (_history.Count > 15) _history.RemoveAt(0);
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (EquationText.Length - 1 > 0)
            {
                string s = EquationText.Substring(0, EquationText.Length - 1);
                EquationText = s;
            }
            else EquationText = "";
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            this.EquationText = "";
            this.ResultText = "0";
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModeChanged != null)
            {
                var text = ((RadioButton)sender).Content.ToString();
                ModeChanged(sender, new StringEventArgs(text));
            }
        }

        private void BtnNumSys_Click(object sender, RoutedEventArgs e)
        {
            NumberSystemDisplayDialog nd = new NumberSystemDisplayDialog();
            nd.DisplayValue = Engine.Ans;
            MainWindow.ShowDialog(nd);
        }

        private void BtnNumToText_Click(object sender, RoutedEventArgs e)
        {
            NumberToTextDialog ntd = new NumberToTextDialog();
            ntd.SetNumber(Engine.Ans);
            MainWindow.ShowDialog(ntd);
        }

        private void BtnFractions_Click(object sender, RoutedEventArgs e)
        {
            string message = "Complex values not supported";
            try
            {
                if (!Helpers.IsComplex(Engine.Ans))
                {
                    if (Engine.Ans is Fraction) message = Engine.Ans.ToString();
                    else
                    {
                        Fraction f = new Fraction((double)Engine.Ans);
                        message = f.ToString();
                    }
                }
            }
            catch (Exception)
            {
                message = "Conversion is not possible";
            }
            MainWindow.ShowDialog("Result as Fraction", message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }

        private void BtnDivisiors_Click(object sender, RoutedEventArgs e)
        {
            string message = "Complex values not supported";
            if (!Helpers.IsComplex(Engine.Ans))
            {
                StringBuilder result = new StringBuilder();
                double x = Helpers.GetDouble(Engine.Ans);
                for (int i = 2; i < 21; i++)
                {
                    if (x % i == 0) result.AppendFormat("{0}, ", i);
                }
                message = result.ToString();
            }
            MainWindow.ShowDialog("Divisiors", message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
        }

        private void BitEngineMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = BitEngineMode.SelectedItem.ToString();
            BitEngineModes mode = BitEngineModes.Signed64bit;
            bool result = Enum.TryParse<BitEngineModes>(selection, out mode);
            if (result) Engine.BitEngineMode = mode;
        }

        private void CbPrefixDisplay_Checked(object sender, RoutedEventArgs e)
        {
            Engine.PreferPrefixes = (bool)CbPrefixDisplay.IsChecked;
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            CbEdit.Focus();
            //TbEditor.Focus();
        }

        private void TbEditor_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (ExecuteRequested != null) ExecuteRequested(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        private void BtnClipboardCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ResultText);
        }

        private void BtnClipboardPaste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText()) EquationText += Clipboard.GetText();
            else MainWindow.ErrorDialog("Clipboard doesn't contain valid text to be inserted");
        }

        private void BtnAnss_Click(object sender, RoutedEventArgs e)
        {
            EquationText += "$ans";
        }

        private void BtnPlot_Click(object sender, RoutedEventArgs e)
        {
            var plot = new Pages.Graphing();
            plot.FunctionY = CbEdit.Text;
            MainWindow.SwithToControl(plot);
        }
    }
}
