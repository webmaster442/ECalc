using ECalc.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ECalc.Engineering
{
    static class Functions
    {
        public static bool isSegmentOn(Rectangle segment)
        {
            SolidColorBrush segmentfill = (SolidColorBrush)segment.Fill;
            if (segmentfill.Color == Colors.Black) return false;
            else return true;
        }

        public static Rectangle GetRectangle(Grid grid, int row, int column)
        {
            return grid.Children.Cast<Rectangle>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
        }

        public static Dictionary<int, bool?> GetMintermTableValues(Grid Minterm)
        {
            Dictionary<int, bool?> ret = new Dictionary<int, bool?>();
            var chbox = Helpers.FindChildren<CheckBox>(Minterm);
            foreach (var ch in chbox)
            {
                ret.Add(Convert.ToInt32(ch.Content), ch.IsChecked);
            }
            return ret;
        }

        public static void SetMintermTableValues(Grid Minterm, List<LogicItem> items)
        {
            var chbox = Helpers.FindChildren<CheckBox>(Minterm);
            foreach (var item in items)
            {
                var box = (from chb in chbox where chb.Content.ToString() == item.Index.ToString() select chb).FirstOrDefault();
                if (box != null) box.IsChecked = item.Checked;
            }
        }

        public static void ClearMintermtable(Grid Minterm)
        {
            var chbox = Helpers.FindChildren<CheckBox>(Minterm);
            foreach (var ch in chbox)
            {
                ch.IsChecked = false;
            }
        }
    }
}
