using ECalc.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECalc.Classes
{
    public static class ConfigFileHelpers
    {
        public static void SerializeFunctionUsageStats()
        {
            if (FunctionList.UsageStats == null) return;
            StringBuilder sb = new StringBuilder();
            foreach (var entry in FunctionList.UsageStats)
            {
                sb.AppendFormat("{0};{1}\n", entry.Key, entry.Value);
            }
            Properties.Settings.Default.FunctionUsageStats = sb.ToString();
        }

        public static Dictionary<string, uint> DeSerializeFunctionUsageStats()
        {
            Dictionary<string, uint> ret = new Dictionary<string, uint>();
            string[] lines = Properties.Settings.Default.FunctionUsageStats.Split('\n');
            foreach (var line in lines)
            {
                string[] csv = line.Split(';');
                if (csv.Length > 1) ret.Add(csv[0], Convert.ToUInt32(csv[1]));
            }
            return ret;
        }

        public static void SerializeConstantUsageStats()
        {
            if (FunctionList.UsageStats == null) return;
            StringBuilder sb = new StringBuilder();
            foreach (var entry in ConstantDB.UsageStats)
            {
                sb.AppendFormat("{0};{1}\n", entry.Key, entry.Value);
            }
            Properties.Settings.Default.ConstantUsageStats = sb.ToString();
        }

        public static Dictionary<string, uint> DeSerializeConstantUsageStats()
        {
            Dictionary<string, uint> ret = new Dictionary<string, uint>();
            string[] lines = Properties.Settings.Default.ConstantUsageStats.Split('\n');
            foreach (var line in lines)
            {
                string[] csv = line.Split(';');
                if (csv.Length > 1) ret.Add(csv[0], Convert.ToUInt32(csv[1]));
            }
            return ret;
        }
    }
}
