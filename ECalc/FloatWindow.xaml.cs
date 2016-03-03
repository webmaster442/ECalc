using MahApps.Metro.Controls;
using System.Windows.Controls;
using System;
using ECalc.Classes;

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
            WindowManager.RegisterChild(this);
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
            WindowManager.UnRegisterChild(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
