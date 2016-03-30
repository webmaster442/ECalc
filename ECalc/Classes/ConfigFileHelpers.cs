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
            if (FunctionList2.UsageStats == null) return;
            var sb = new StringBuilder();
            foreach (var entry in FunctionList2.UsageStats)
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

        public static void SaveUserFunctions()
        {
            try
            {
                var xs = new XmlSerializer(typeof(UserFuntion[]));
                using (var stream = File.Create("userprograms.xml"))
                {
                    xs.Serialize(stream, Engine.UserFunctions.ToArray());
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }

        public static void LoadUserFunctions()
        {
            try
            {
                if (!File.Exists("userprograms.xml")) return;
                var xs = new XmlSerializer(typeof(UserFuntion[]));
                using (var stream = File.OpenRead("userprograms.xml"))
                {
                    var items = (UserFuntion[])xs.Deserialize(stream);
                    Engine.UserFunctions.Clear();
                    Engine.UserFunctions.AddRange(items);
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
        }
    }
}
