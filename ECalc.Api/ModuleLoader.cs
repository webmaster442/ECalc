using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Api
{
    /// <summary>
    /// Module loader class to load modules
    /// </summary>
    public class ModuleLoader
    {
        private readonly List<EcalcModule> _modules;

        /// <summary>
        /// Creates a new Instance of ModuleLoaer
        /// </summary>
        public ModuleLoader()
        {
            _modules = new List<EcalcModule>();
        }

        /// <summary>
        /// Loads Modules from an assembly file
        /// </summary>
        /// <param name="assembly">assembly to load from</param>
        public void LoadFromAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentException("assembly");
            try
            {
                var key = assembly.GetName().GetPublicKey();
                if (key.Length == 0) throw new Exception("Assembly doesn't have a strong name");

                var modules = from t in assembly.GetTypes()
                              where t.IsClass &&
                                    t.BaseType == typeof(EcalcModule)
                              select t;

                foreach (var module in modules)
                {
                    var mod = (EcalcModule)Activator.CreateInstance(module);
                    _modules.Add(mod);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads modules from a directory
        /// </summary>
        /// <param name="directory">Directory to load from</param>
        /// <param name="filter">filter string</param>
        public void LoadFromFiles(string directory, string filter)
        {
            try
            {
                var files = Directory.GetFiles(directory, filter);
                foreach (var file in files)
                {
                    var buffer = File.ReadAllBytes(file);
                    Assembly a = Assembly.Load(buffer);
                    LoadFromAssembly(a);
                    buffer = null;
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads modules from a specified namespace
        /// </summary>
        /// <param name="ns">Namespace to load from</param>
        public void LoadFromNameSpace(string ns)
        {
            try
            {
                var types = from t in Assembly.GetCallingAssembly().GetTypes()
                            where t.IsClass &&
                                  t.BaseType == typeof(EcalcModule) &&
                                  t.Namespace == ns
                            select t;

                foreach (var type in types)
                {
                    var mod = (EcalcModule)Activator.CreateInstance(type);
                    _modules.Add(mod);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Runs a module by it's name
        /// </summary>
        /// <param name="name">Module name to run</param>
        /// <returns>A module's user control</returns>
        public UserControl RunByName(string name)
        {
            var q = from module in _modules
                    where module.ModuleName == name
                    select module;
            var x = q.FirstOrDefault();
            if (x != null) return x.GetControl();
            else return null;
        }

        /// <summary>
        /// Get a list of unique categories
        /// </summary>
        public string[] Categories
        {
            get
            {
                var q = from module in _modules.AsParallel()
                        orderby module.ModuleCategory ascending
                        select module.ModuleCategory;

                var cats = new List<string>();
                cats.Add("All");
                cats.AddRange(q.Distinct());
                return cats.ToArray();
            }
        }

        /// <summary>
        /// Select modules of a category
        /// </summary>
        /// <param name="category">Category to filter. Null returns all</param>
        /// <returns>An array of modules that match the criteria</returns>
        public EcalcModule[] Select(string category = null)
        {
            if (string.IsNullOrEmpty(category) || category == "All")
            {
                var sorted = from i in _modules
                             orderby i.ModuleName ascending
                             select i;
                return sorted.ToArray();
            }
            else
            {
                var q = from i in _modules.AsParallel()
                        where i.ModuleCategory == category
                        orderby i.ModuleName ascending
                        select i;
                return q.ToArray();
            }
        }

        /// <summary>
        /// Drops all loaded modules from memory
        /// </summary>
        public void DropModules()
        {
            _modules.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
