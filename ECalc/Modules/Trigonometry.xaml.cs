using ECalc.Maths;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for Trigonometry.xaml
    /// </summary>
    public partial class Trigonometry : UserControl
    {
        public Trigonometry()
        {
            InitializeComponent();
        }

        private void A_ValueChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                
                TbError.Visibility = Visibility.Collapsed;
                var newC = Math.Sqrt((B.Value * B.Value) + (A.Value * A.Value));
                var newAlpha = TrigFunctions.ArcSin(A.Value / newC);
                var newBeta = 90 - newAlpha;
                if (newBeta < 0 || double.IsNaN(newBeta)) throw new ArgumentException();
                C.SetValue(newC);
                Alpha.SetValue(newAlpha);
                Beta.SetValue(newBeta);
            }
            catch (Exception)
            {
                TbError.Visibility = Visibility.Visible;
            }
        }

        private void C_ValueChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                TbError.Visibility = Visibility.Collapsed;
                var newB = Math.Sqrt((C.Value * C.Value) - (B.Value * B.Value));
                var newAlpha = TrigFunctions.ArcSin(newB / C.Value);
                var newBeta = 90 - newAlpha;
                if (newBeta < 0 || double.IsNaN(newBeta)) throw new ArgumentException();
                B.SetValue(newB);
                Alpha.SetValue(newAlpha);
                Beta.SetValue(newBeta);
            }
            catch (Exception)
            {
                TbError.Visibility = Visibility.Visible;
            }
        }

        private void Alpha_ValueChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                TbError.Visibility = Visibility.Collapsed;
                var newa = TrigFunctions.Tan(Alpha.Value) * B.Value;
                var newBeta = 90 - Alpha.Value;
                if (newBeta < 0 || double.IsNaN(newBeta)) throw new ArgumentException();
                var newC = Math.Sqrt((B.Value * B.Value) + (newa * newa));
                C.SetValue(newC);
                A.SetValue(newa);
                Beta.SetValue(newBeta);
            }
            catch (Exception)
            {
                TbError.Visibility = Visibility.Visible;
            }
        }

        private void Beta_ValueChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                TbError.Visibility = Visibility.Collapsed;
                var newb = TrigFunctions.Tan(Beta.Value) * A.Value;
                var newAlpha = 90 - Beta.Value;
                if (newAlpha < 0 || double.IsNaN(newAlpha)) throw new ArgumentException();
                var newC = Math.Sqrt((newb * newb) + (A.Value * A.Value));
                C.SetValue(newC);
                B.SetValue(newb);
                Alpha.SetValue(newAlpha);
            }
            catch (Exception)
            {
                TbError.Visibility = Visibility.Visible;
            }
        }
    }
}
