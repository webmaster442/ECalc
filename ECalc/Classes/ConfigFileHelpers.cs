using ECalc.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ECalc.Classes
{
    public static class ConfigFileHelpers
    {
        public static void SerializeFunctionUsageStats()
        {
            if (UltimateFunctionList.UsageStats == null) return;
            var sb = new StringBuilder();
            foreach (var entry in UltimateFunctionList.UsageStats)
            {
                sb.AppendFormat("{0};{1}\n", entry.Key, entry.Value);
            }
            Properties.Settings.Default.FunctionUsageStats = sb.ToString();
        }

        public static Dictionary<string, uint> DeSerializeFunctionUsageStats()
        {
            var ret = new Dictionary<string, uint>();
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
            if (ConstantDB.UsageStats == null) return;
            var sb = new StringBuilder();
            foreach (var entry in ConstantDB.UsageStats)
            {
                sb.AppendFormat("{0};{1}\n", entry.Key, entry.Value);
            }
            Properties.Settings.Default.ConstantUsageStats = sb.ToString();
        }

        public static Dictionary<string, uint> DeSerializeConstantUsageStats()
        {
            var ret = new Dictionary<string, uint>();
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
