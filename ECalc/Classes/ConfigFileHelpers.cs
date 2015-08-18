using ECalc.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECalc.Classes
{
    public static class ConfigFileHelpers
    {
        public static void SerializeUsageStats()
        {
            if (FunctionList.UsageStats == null) return;
            StringBuilder sb = new StringBuilder();
            foreach (var entry in FunctionList.UsageStats)
            {
                sb.AppendFormat("{0};{1}\n", entry.Key, entry.Value);
            }
            Properties.Settings.Default.UsageStats = sb.ToString();
        }

        public static Dictionary<string, uint> DeSerializeUsageStats()
        {
            Dictionary<string, uint> ret = new Dictionary<string, uint>();
            string[] lines = Properties.Settings.Default.UsageStats.Split('\n');
            foreach (var line in lines)
            {
                string[] csv = line.Split(';');
                if (csv.Length > 1) ret.Add(csv[0], Convert.ToUInt32(csv[1]));
            }
            return ret;
        }
    }
}
