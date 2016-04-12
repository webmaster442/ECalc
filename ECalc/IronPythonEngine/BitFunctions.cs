using System;
using System.Numerics;

namespace ECalc.IronPythonEngine
{
    enum BitFunction
    {
        AND, OR, NOT, XOR, SHL, SHR
    }

    static class BitOps
    {
        private static bool Floats(double d)
        {
            return d - Math.Truncate(d) != 0;
        }

        public static double DoFunction(double p1, double p2, BitFunction function)
        {
            if (Floats(p1) || Floats(p2))
            {
                throw new Exception("Binary operations only supported on integer types");
            }
            else
            {
                var n1 = new BigInteger(p1);
                var n2 = new BigInteger(p2);
                BigInteger result = 0;

                switch (function)
                {
                    case BitFunction.AND:
                        result = n1 & n2;
                        break;
                    case BitFunction.OR:
                        result = n1 | n2;
                        break;
                    case BitFunction.XOR:
                        result = n1 ^ n2;
                        break;
                    case BitFunction.NOT:
                        result = ~n1;
                        break;
                    case BitFunction.SHL:
                        result = n1 << (int)n2;
                        break;
                    case BitFunction.SHR:
                        result = n1 >> (int)n2;
                        break;
                }
                return (double)result;
            }
        }
    }
}
