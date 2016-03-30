using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ECalc.Classes
{
    internal class ConstantList: ObservableCollection<Constant>
    {
        internal ConstantCategory _category;

        public ConstantList()
        {
            Category = ConstantCategory.Mathematical;
        }

        public void Add(IEnumerable<Constant> range)
        {
            foreach (var item in range)
            {
                this.Add(item);
            }
        }

        public ConstantCategory Category
        {
            get { return _category; }
            set
            {
                _category = value;
                this.Clear();
                switch (value)
                {
                    case ConstantCategory.Mathematical:
                        Add(ConstantDB.Mathematical);
                        break;
                    case ConstantCategory.Universal:
                        Add(ConstantDB.Universal);
                        break;
                    case ConstantCategory.ElectroMagnetic:
                        Add(ConstantDB.ElectroMagnetic);
                        break;
                    case ConstantCategory.Atomic:
                        Add(ConstantDB.Atomic);
                        break;
                    case ConstantCategory.Favorites:
                        Add(ConstantDB.Favorites());
                        break;
                }
            }
        }
    }
}
