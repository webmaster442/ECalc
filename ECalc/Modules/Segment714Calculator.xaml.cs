using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using AppLib.WPF.Extensions;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for Segment714Calculator.xaml
    /// </summary>
    public partial class Segment714Calculator : UserControl
    {
        public Segment714Calculator()
        {
            InitializeComponent();
        }

        private bool IsOn(ToggleButton tb)
        {
            var res = Segment7.FindChild<ToggleButton>(tb.Name);
            if (res == null) res = Segment14.FindChild<ToggleButton>(tb.Name);
            if (res == null) throw new ArgumentException("Not found");
            else return (bool)res.IsChecked;
        }

        private void Calculate7Segment()
        {
            int cc = 0;
            if (IsOn(Seg7a)) cc += CbLSBOrder.IsChecked == true ? 1 : 128;
            if (IsOn(Seg7b)) cc += CbLSBOrder.IsChecked == true ? 2 : 64;
            if (IsOn(Seg7c)) cc += CbLSBOrder.IsChecked == true ? 4 : 32;
            if (IsOn(Seg7d)) cc += CbLSBOrder.IsChecked == true ? 8 : 16;
            if (IsOn(Seg7e)) cc += CbLSBOrder.IsChecked == true ? 16 : 8;
            if (IsOn(Seg7f)) cc += CbLSBOrder.IsChecked == true ? 32 : 4;
            if (IsOn(Seg7g)) cc += CbLSBOrder.IsChecked == true ? 64 : 2;
            if (IsOn(Seg7dp)) cc += CbLSBOrder.IsChecked == true ? 128 : 1;
            int ca = 255 - cc;

            TbCc.Text = Convert.ToString(cc, 16);
            TbCa.Text = Convert.ToString(ca, 16);
        }

        private void Calculate14Segment()
        {
            int cc = 0;
            if (IsOn(Seg14a)) cc += CbLSBOrder.IsChecked == true ? 1 : 16384;
            if (IsOn(Seg14b)) cc += CbLSBOrder.IsChecked == true ? 2 : 8192;
            if (IsOn(Seg14c)) cc += CbLSBOrder.IsChecked == true ? 4 : 4096;
            if (IsOn(Seg14d)) cc += CbLSBOrder.IsChecked == true ? 8 : 2048;
            if (IsOn(Seg14e)) cc += CbLSBOrder.IsChecked == true ? 16 : 1024;
            if (IsOn(Seg14f)) cc += CbLSBOrder.IsChecked == true ? 32 : 512;
            if (IsOn(Seg14g1)) cc += CbLSBOrder.IsChecked == true ? 64 : 256;
            if (IsOn(Seg14g2)) cc += 128;
            if (IsOn(Seg14h)) cc += CbLSBOrder.IsChecked == true ? 256 : 64;
            if (IsOn(Seg14i)) cc += CbLSBOrder.IsChecked == true ? 512 : 32;
            if (IsOn(Seg14j)) cc += CbLSBOrder.IsChecked == true ? 1024 : 16;
            if (IsOn(Seg14k)) cc += CbLSBOrder.IsChecked == true ? 2048 : 8;
            if (IsOn(Seg14l)) cc += CbLSBOrder.IsChecked == true ? 4096 : 4;
            if (IsOn(Seg14m)) cc += CbLSBOrder.IsChecked == true ? 8192 : 2;
            if (IsOn(Seg14dp)) cc += CbLSBOrder.IsChecked == true ?  16384: 1;
            int ca = 0x7fff - cc;

            TbCc.Text = Convert.ToString(cc, 16);
            TbCa.Text = Convert.ToString(ca, 16);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var s7 = Segment7.FindChildren<ToggleButton>();
            foreach (var i in s7) i.Click += SegmentClicked;
            var s14 = Segment14.FindChildren<ToggleButton>();
            foreach (var i in s14) i.Click += SegmentClicked;
        }

        private void SegmentClicked(object sender, RoutedEventArgs e)
        {
            if (TabDisplay.SelectedIndex == 0) Calculate7Segment();
            else Calculate14Segment();
        }

        private void TabDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabDisplay.SelectedIndex == 0) Calculate7Segment();
            else Calculate14Segment();
        }
    }
}
