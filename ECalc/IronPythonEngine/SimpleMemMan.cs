using System;
using System.Collections.Generic;
using System.Linq;

namespace ECalc.IronPythonEngine
{
    internal class SimpleMemmMan : IMemManager
    {
        private readonly Dictionary<string, object> _mem;

        public SimpleMemmMan()
        {
            _mem = new Dictionary<string, object>();
        }

        public object GetItem(string name)
        {
            return _mem[name];
        }

        public string[] ListRegisters(string query)
        {
            return _mem.Keys.ToArray();
        }

        public void SetItem(object value)
        {
            throw new NotImplementedException();
        }

        public void SetItem(string name, object value)
        {
            if (_mem.ContainsKey(name)) _mem[name] = value;
            else _mem.Add(name, value);
        }
    }
}
