using ECalc.Classes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Pages
{
    /// <summary>
    /// Interaction logic for WindowManager.xaml
    /// </summary>
    public partial class WindowsManager : UserControl
    {
        public WindowsManager()
        {
            InitializeComponent();
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
            WinList.ItemsSource = null;
            WinList.ItemsSource = WindowManager.Previews;
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
