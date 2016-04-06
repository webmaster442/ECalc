using ECalc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for WindowManager.xaml
    /// </summary>
    public partial class WindowsManager : UserControl
    {
        private DispatcherTimer _timer;
        private int _lastcounter;

        public WindowsManager()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1.5);
            _timer.Tick += _timer_Tick;
            _timer.IsEnabled = true;
            _lastcounter = 0;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void BtnMinimizeAll_Click(object sender, RoutedEventArgs e)
        {
            WindowManager.MinimizeAll();
        }

        private void BtnRestoreAll_Click(object sender, RoutedEventArgs e)
        {
            WindowManager.RestoreAll();
        }

        private void BtnCloseAll_Click(object sender, RoutedEventArgs e)
        {
            WindowManager.CloseAll();
            Refresh();
        }

        private void Refresh()
        {
            var previews = WindowManager.Previews;

            if (previews.Count != _lastcounter)
            {
                WinList.ItemsSource = null;
                WinList.ItemsSource = previews;
                _lastcounter = previews.Count;
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void WinBringToFront_Click(object sender, RoutedEventArgs e)
        {
            var index = WinList.SelectedIndex;
            if (index < 0) return;
            var window = WindowManager.GetWindow(index);
            WindowManager.BringToFront(window);
        }

        private void WinClose_Click(object sender, RoutedEventArgs e)
        {
            var index = WinList.SelectedIndex;
            if (index < 0) return;
            try
            {
                var window = WindowManager.GetWindow(index);
                WindowManager.UnRegisterChild(window);
                window.Close();
            }
            catch (Exception) { }
            Refresh();
        }
    }
}
