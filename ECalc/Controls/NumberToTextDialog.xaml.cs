using ECalc.Classes;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Numerics;
using System.Speech.Synthesis;
using System.Windows;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for NumberToTextWindow.xaml
    /// </summary>
    public partial class NumberToTextDialog : CustomDialog, IDisposable
    {
        private SpeechSynthesizer _synthesizer;

        public NumberToTextDialog()
        {
            InitializeComponent();
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SpeakCompleted += _synthesizer_SpeakCompleted;
        }

        public void SetNumber(object o)
        {
            if (o is double)
            {
                TbText.Text = NumberToText.NumberText((double)o);
            }
            else if (o is Complex)
            {
                Complex par = (Complex)o;
                TbText.Text = string.Format("Real part: {0}, Imaginary part: {1}",
                                            NumberToText.NumberText(par.Real),
                                            NumberToText.NumberText(par.Imaginary));
            }
            else if (o is Fraction)
            {
                Fraction fr = (Fraction)o;
                TbText.Text = string.Format("{0} over {1}",
                                            NumberToText.NumberText(fr.Numerator),
                                            NumberToText.NumberText(fr.Denominator));
            }
            else
            {
                TbText.Text = "Not a number";
            }
        }

        private void BtnSayIT_Click(object sender, RoutedEventArgs e)
        {
            _synthesizer.Volume = Convert.ToInt32(SliderVolume.Value);
            _synthesizer.Rate = Convert.ToInt32(SliderSpeed.Value);
            BtnSayIT.IsEnabled = false;
            _synthesizer.SpeakAsync(TbText.Text);
        }

        private void _synthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            BtnSayIT.IsEnabled = true;
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TbText.Text);
        }

        private async void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            await main.HideMetroDialogAsync(this);
        }

        protected virtual void Dispose(bool native)
        {
            if (_synthesizer != null)
            {
                _synthesizer.Dispose();
                _synthesizer = null;
            }
        }
            
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
