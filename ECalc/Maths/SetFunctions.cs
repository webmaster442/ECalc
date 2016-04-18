using ECalc.IronPythonEngine;

namespace ECalc.Maths
{
    [Loadable]
    public static class SetFunctions
    {
        [Category("Sets")]
        public static IronPythonEngine.Types.Set Set(params double[] d)
        {
            return new IronPythonEngine.Types.Set(d);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Distinct(IronPythonEngine.Types.Set s1)
        {
            return IronPythonEngine.Types.Set.Distinct(s1);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Intersect(IronPythonEngine.Types.Set s1, IronPythonEngine.Types.Set s2)
        {
            return IronPythonEngine.Types.Set.Intersect(s1, s2);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Union(IronPythonEngine.Types.Set s1, IronPythonEngine.Types.Set s2)
        {
            return IronPythonEngine.Types.Set.Union(s1, s2);
        }

        [Category("Sets")]
        public static IronPythonEngine.Types.Set Except(IronPythonEngine.Types.Set s1, IronPythonEngine.Types.Set s2)
        {
            return IronPythonEngine.Types.Set.Except(s1, s2);
        }
    }
}
