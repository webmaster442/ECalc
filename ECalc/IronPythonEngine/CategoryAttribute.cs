using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.IronPythonEngine
{
    [AttributeUsage(AttributeTargets.Method)]
    class CategoryAttribute: Attribute
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
}
