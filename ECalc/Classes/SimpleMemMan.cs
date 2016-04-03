using ECalc.IronPythonEngine;
using System.Collections.Generic;
using System.Linq;

namespace ECalc.Classes
{
    internal class SimpleMemmMan : IMemManager
    {
        private readonly Dictionary<string, object> _mem;
        private int _TempCounter;

        public SimpleMemmMan()
        {
            _mem = new Dictionary<string, object>();
            _TempCounter = 1;
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
            
        }

        public void SetItem(string name, object value)
        {
            if (_mem.ContainsKey(name)) _mem[name] = value;
            else _mem.Add(name, value);
        }

        public void ClearTemp()
        {
            //Befejezni itt ezt a fost!
            for (int i = _TempCounter; i >= 1; i--)
            {
                _mem.Remove("$arg" + _TempCounter.ToString());
            }
            _TempCounter = 1;
        }

        public void PushTemp(object value)
        {
            _mem["$arg" + _TempCounter.ToString()] = value;
            _TempCounter++;
        }
    }
}
