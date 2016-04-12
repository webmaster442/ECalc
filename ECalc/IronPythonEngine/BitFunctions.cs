using System;
using System.Numerics;

namespace ECalc.IronPythonEngine
{
    enum BitFunction
    {
        AND, OR, NOT, XOR
    }

    static class BitOps
    {
        private static bool Floats(double d)
        {
            return d - Math.Truncate(d) != 0;
        }

        private static byte[] DoFunction(byte[]array1, byte[] array2, BitFunction f)
        {
            byte[] ret = new byte[8];
            for (int i=0; i< ret.Length; i++)
            {
                switch (f)
                {
                    case BitFunction.AND:
                        ret[i] = (byte)(array1[i] & array2[i]);
                        break;
                    case BitFunction.OR:
                        ret[i] = (byte)(array1[i] | array2[i]);
                        break;
                    case BitFunction.NOT:
                        ret[i] = (byte)(~array1[i]);
                        break;
                    case BitFunction.XOR:
                        ret[i] = (byte)(array1[i] ^ array2[i]);
                        break;
                }
            }
            return ret;
        }

        public static double DoFunction(double p1, double p2, BitFunction function)
        {
            if (Floats(p1) || Floats(p2))
            {
                var array1 = BitConverter.GetBytes(p1);
                var array2 = BitConverter.GetBytes(p2);
                return BitConverter.ToDouble(DoFunction(array1, array2, function), 0);
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
                }
                return (double)result;
            }
        }
    }
}
