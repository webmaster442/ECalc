using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.IronPythonEngine
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class CategoryAttribute: Attribute
    {
        public string Category
        {
            get;
            private set;
        }

        public CategoryAttribute(string cat)
        {
            Category = cat;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class LoadableAttribute: Attribute { }

    [Serializable]
    internal class FunctionInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Category { get; set; }
    }

}
