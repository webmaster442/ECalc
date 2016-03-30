using ECalc.Engineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WPFLib.Extensions;

namespace ECalc.Controls.Special
{
    static class MintermTableHelpers
    {
        public static Dictionary<int, bool?> GetMintermTableValues(Grid Minterm)
        {
            var ret = new Dictionary<int, bool?>();
            var chbox = Minterm.FindChildren<CheckBox>();
            foreach (var ch in chbox)
            {
                ret.Add(Convert.ToInt32(ch.Content), ch.IsChecked);
            }
            return ret;
        }

        public static void SetMintermTableValues(Grid Minterm, IEnumerable<LogicItem> items)
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
        LogicItem[] GetSelected();
        void SetSelected(LogicItem[] vals);
        void ClearInput();
        void SwapVarnames();
        void SetAll(bool? value);
    }
}
