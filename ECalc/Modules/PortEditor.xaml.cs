using AppLib.WPF.Extensions;
using ECalc.Controls.Special;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Modules
{
    /// <summary>
    /// Interaction logic for PortEditor.xaml
    /// </summary>
    public partial class PortEditor : UserControl
    {
        public PortEditor()
        {
            InitializeComponent();
        }

        private void IterateEditors(Action<PortEdit8Bit> Callback)
        {
            var tab = TabSelector.SelectedItem as TabItem;
            var ctrl = tab.Content as UIElement;
            if (ctrl is PortEdit8Bit)
            {
                Callback.Invoke(ctrl as PortEdit8Bit);
                return;
            }
            foreach (var editor in ctrl.FindChildren<PortEdit8Bit>())
            {
                Callback.Invoke(editor);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            IterateEditors((ed) =>
            {
                ed.Reset();
            });
        }

        private void BtnInvert_Click(object sender, RoutedEventArgs e)
        {
            IterateEditors((ed) =>
            {
                ed.Invert();
            });
        }
    }
}
