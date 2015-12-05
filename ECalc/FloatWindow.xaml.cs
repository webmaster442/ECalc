using MahApps.Metro.Controls;
using System.Windows.Controls;
using System;

namespace ECalc
{
    /// <summary>
    /// Interaction logic for FloatWindow.xaml
    /// </summary>
    public partial class FloatWindow : MetroWindow
    {
        public FloatWindow()
        {
            InitializeComponent();
        }

        public void SetWindowContent(UserControl u, string title)
        {
            this.Content = u;
            this.Title = title;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.Content is IDisposable)
            {
                (Content as IDisposable).Dispose();
            }
            Content = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
