using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace PowerWPF
{
    public static class Utilities
    {
        public static string GetScriptFileCode(string resName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var strResources = assembly.GetName().Name + ".g.resources";
            var rStream = assembly.GetManifestResourceStream(strResources);
            var resourceReader = new ResourceReader(rStream);
            var items = resourceReader.OfType<DictionaryEntry>();
            var stream = items.First(x => (x.Key as string) == resName.ToLower()).Value;
            
            var reader = new StreamReader((UnmanagedMemoryStream)stream);
            var output = reader.ReadToEnd();
            return output;
        }
    }
}
