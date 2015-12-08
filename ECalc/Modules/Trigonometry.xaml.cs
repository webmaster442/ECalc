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
                TbAlpha.Text = string.Format("{0:0.0000}", newAlpha);
                TbBeta.Text = string.Format("{0:0.0000}", newBeta);
                C.SetValue(newC);
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
                TbAlpha.Text = string.Format("{0:0.0000}", newAlpha);
                TbBeta.Text = string.Format("{0:0.0000}", newBeta);
            }
            catch (Exception)
            {
                TbError.Visibility = Visibility.Visible;
            }
        }
    }
}
