﻿using ECalc.Classes;
using ECalc.Maths;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Numerics;
using System.Windows;

namespace ECalc.Controls
{
    /// <summary>
    /// Interaction logic for NumberSystemDisplayDialog.xaml
    /// </summary>
    public partial class NumberSystemDisplayDialog : CustomDialog
    {
        public NumberSystemDisplayDialog()
        {
            InitializeComponent();
        }

        public void SetDisplay(object o)
        {
            NumBox.DisplayNumber(o);
        }

        private async void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var main = (MainWindow)Application.Current.MainWindow;
            await main.HideMetroDialogAsync(this);
        }
    }
}
