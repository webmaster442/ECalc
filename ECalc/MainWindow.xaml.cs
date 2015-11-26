using ECalc.Classes;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
using ECalc.Api;
using ECalc.Pages;
using ECalc.Controls;
using System.Threading.Tasks;

namespace ECalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {


        public MainWindow()
        {
            Modules = new ModuleLoader();
            Modules.LoadFromNameSpace("ECalc.Modules");
            InitializeComponent();

        }

        /// <summary>
        /// Switches to another user control
        /// </summary>
        /// <param name="control">A user control to show</param>
        public static void SwithToControl(UserControl control)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.TransitionControl.Content = null;
            foreach (Flyout flyout in main.Flyouts.Items) flyout.IsOpen = false;
            GC.WaitForPendingFinalizers();
            GC.Collect();
            main.TransitionControl.Content = control;
        }

        /// <summary>
        /// Display an error dialog
        /// </summary>
        /// <param name="error">error text</param>
        public static async void ErrorDialog(string error)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            await main.ShowMessageAsync("Error", error, MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// Display a generic dialog
        /// </summary>
        /// <param name="error">error text</param>
        public static async void ShowDialog(string title, string text, MessageDialogStyle style)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            await main.ShowMessageAsync(title, text, style);
        }

        /// <summary>
        /// Property to acces application modules
        /// </summary>
        public static ModuleLoader Modules
        {
            get;
            private set;
        }

        /// <summary>
        /// Shows a custom dialog
        /// </summary>
        /// <param name="dialog">dialog to display</param>
        public static async void ShowDialog(CustomDialog dialog)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            await main.ShowMetroDialogAsync(dialog);
        }

        public static void SetTitle(string titletext)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.Title = titletext;
        }

        private void WindowCommandMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyOut.IsOpen = !MenuFlyOut.IsOpen;
        }

        private void WindowCommandKeyboard_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "osk.exe";
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConfigFileHelpers.SerializeUsageStats();
            Properties.Settings.Default.Save();
        }

        private void WindowCommandAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog w = new AboutDialog();
            ShowDialog(w);
        }
    }
}
