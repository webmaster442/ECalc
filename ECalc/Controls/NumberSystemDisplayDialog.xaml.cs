using ECalc.Classes;
using ECalc.Maths;
using MahApps.Metro.Controls.Dialogs;
using System;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for NumberSystemDisplayDialog.xaml
    /// </summary>
    public partial class NumberSystemDisplayDialog : CustomDialog
    {
        private object _display;

        public NumberSystemDisplayDialog()
        {
            InitializeComponent();
        }

        public object DisplayValue
        {
            get { return _display; }
            set
            {
                _display = value;
                RefreshDisplays();
            }
        }

        private void RefreshDisplays()
        {
            if (Helpers.IsComplex(_display))
            {
                Binary.Text = "Complex results not supported";
                Octal.Text = "Complex results not supported";
                Hexa.Text = "Complex results not supported";
                Roman.Text = "Complex results not supported";
            }
            else
            {
                double d = (double)_display;
                bool isint = (d - Math.Truncate(d)) == 0;
                if (isint)
                {
                    long val = Convert.ToInt64(d);
                    Binary.Text = Convert.ToString(val, 2);
                    Octal.Text = Convert.ToString(val, 8);
                    Hexa.Text = Convert.ToString(val, 16);
                    if (val < 3999 && val > 0)
                    {
                        Roman.Text = NumberSystemConversions.IntToRoman(Convert.ToInt32(val));
                    }
                    else Roman.Text = "Roman numbers are supported only on integers in range of 0 and 3999";
                }
                else
                {
                    Roman.Text = "Floating point numbers not suppoerted";
                    Binary.Text = "Floating point numbers not suppoerted";
                    Octal.Text = "Floating point numbers not suppoerted";

                }
            }
        }

        private async void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            await main.HideMetroDialogAsync(this);
        }
    }
}
