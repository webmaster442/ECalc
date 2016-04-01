using ECalc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using ECalc.IronPythonEngine;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : UserControl
    {
        private Engine _engine;


        public Calculator()
        {
            InitializeComponent();
            Display.Focus();
            _engine = new Engine();
        }

        private async void KeyPad_ExecuteClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await _engine.EvaluateAsync(Display.EquationText);
                Display.AddToHistory();
                Display.ResultText = result;
            }
            catch (Exception ex)
            {
                Display.ResultText = "Error";
                MainWindow.ErrorDialog(ex.Message);
            }
            Display.EquationText = "";
        }

        private void KeyPad_ButtonClicked(object sender, StringEventArgs e)
        {
            if (Engine.IsOperator(e.Text))
            {
                Display.EquationText += string.Format(" {0} ", e.Text);
            }
            else Display.EquationText += e.Text;
        }

        private async void Keypad_FromExpressionClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await _engine.EvaluateAsync(Display.EquationText);
                Keypad.SetItem(result);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        private void FunctionList_FunctionButtonCliked(object sender, StringEventArgs e)
        {
            Display.EquationText += string.Format(" {0}( ", e.Text);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designTime) return;
            _engine = new Engine();
            //_engine.MemoryManager = Keypad;
            FncList.Funtions = Engine.Functions;
        }

        private void Display_ModeChanged(object sender, StringEventArgs e)
        {
            TrigMode output;
            if (Enum.TryParse<TrigMode>(e.Text, out output))
            {
                Engine.Mode = output;
            }
        }

        private void Extended_BackClicked(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => { InputSelector.SelectedIndex = 0; });
        }
    }
}
