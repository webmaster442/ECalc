﻿using ECalc.Engineering;
using ECalc.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ECalc.Controls.Special
{
    static class MintermTableHelpers
    {
        public static Dictionary<int, bool?> GetMintermTableValues(Grid Minterm)
        {
            Dictionary<int, bool?> ret = new Dictionary<int, bool?>();
            var chbox = Minterm.FindChildren<CheckBox>();
            foreach (var ch in chbox)
            {
                ret.Add(Convert.ToInt32(ch.Content), ch.IsChecked);
            }
            return ret;
        }

        public static void SetMintermTableValues(Grid Minterm, List<LogicItem> items)
        {
            var chbox = Minterm.FindChildren<CheckBox>();
            foreach (var item in items)
            {
                var box = (from chb in chbox where chb.Content.ToString() == item.Index.ToString() select chb).FirstOrDefault();
                if (box != null) box.IsChecked = item.Checked;
            }
        }

        public static void ClearMintermtable(Grid Minterm)
        {
            var chbox = Minterm.FindChildren<CheckBox>();
            foreach (var ch in chbox)
            {
                ch.IsChecked = false;
            }
        }
    }

    public interface IMintermTable
    {
        List<LogicItem> Selected { get; set; }
        void ClearInput();
        void SwapVarnames();
        void SetAll(bool? value);
    }
}