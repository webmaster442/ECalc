using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ECalc.Classes;
using System.Text;
using System;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    internal partial class Display : UserControl
    {
        private List<string> _history;
        private int _historyIndex;

        public Display()
        {
            InitializeComponent();
            _history = new List<string>();
            _historyIndex = 0;
        }

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
                    sb.Append(' ');
                }
            }
            Reverse(sb);
            if (parts.Length > 1)
            {
                sb.Append(",");
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
                if (CbThousandGrouping.IsChecked == true)
                {
                    SetValue(ResultTextProperty, Group(value));
                }
                else SetValue(ResultTextProperty, value);
            }
        }

        /// <summary>
        /// Calculator mode changed event
        /// </summary>
        public event StringEventHandler ModeChanged;

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

        private void BtnHistoryPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_history.Count == 0) return;
            _historyIndex--;
            if (_historyIndex < 0) _historyIndex = (_history.Count - 1);
            EquationText = _history[_historyIndex];

        }

        private void BtnHistoryNext_Click(object sender, RoutedEventArgs e)
        {
            if (_history.Count == 0) return;
            _historyIndex++;
            if (_historyIndex > (_history.Count - 1)) _historyIndex = 0;
            EquationText = _history[_historyIndex];
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
    }
}
