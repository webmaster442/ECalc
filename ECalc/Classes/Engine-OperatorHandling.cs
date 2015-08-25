using System;
using System.Numerics;

namespace ECalc.Classes
{
    internal partial class Engine
    {
        /*  return type matrix:
    Type       CPLX      Fraction        Double
    CPX        CPLX      CPLX            CPLX
    Fraction   CPLX      Fraction        Fraction
    Double     CPLX      Fraction        Double  */
        /// <summary>
        /// Type matching operator handler function
        /// </summary>
        /// <param name="op1">operand1</param>
        /// <param name="op2">operand2</param>
        /// <param name="op">operation</param>
        /// <returns>result type</returns>
        public object HandleOperators(object op1, object op2, string op)
        {
            var t1 = op1.GetType().FullName;
            var t2 = op2.GetType().FullName;

            if (t1 == "System.Numerics.Complex" || t2 == "System.Numerics.Complex")
            {
                Complex a = new Complex();
                Complex b = new Complex();
                switch (t1)
                {
                    case "System.Numerics.Complex":
                        a = (Complex)op1;
                        break;
                    case "System.Double":
                        a = new Complex((double)op1, 0);
                        break;
                    case "ECalc.Classes.Fraction":
                        double d = ((Fraction)op1).ToDouble();
                        a = new Complex(d, 0);
                        break;
                }
                switch (t2)
                {
                    case "System.Numerics.Complex":
                        b = (Complex)op2;
                        break;
                    case "System.Double":
                        b = new Complex((double)op2, 0);
                        break;
                    case "ECalc.Classes.Fraction":
                        double d = ((Fraction)op2).ToDouble();
                        b = new Complex(d, 0);
                        break;
                }

                switch (op)
                {
                    case "+":
                        return a + b;
                    case "-":
                        return a - b;
                    case "÷":
                        return a / b;
                    case "X":
                        return a * b;
                    case "mod":
                        return new Complex(a.Real % b.Real, a.Imaginary % b.Imaginary);
                }
            }
            if (t1 == "ECalc.Classes.Fraction" || t2 == "ECalc.Classes.Fraction")
            {
                Fraction f1 = new Fraction();
                Fraction f2 = new Fraction();
                switch (t1)
                {
                    case "System.Double":
                        f1 = new Fraction((double)op1);
                        break;
                    case "ECalc.Classes.Fraction":
                        f1 = (Fraction)op1;
                        break;
                }
                switch (t2)
                {
                    case "System.Double":
                        f2 = new Fraction((double)op2);
                        break;
                    case "ECalc.Classes.Fraction":
                        f2 = (Fraction)op2;
                        break;
                }
                switch (op)
                {
                    case "+":
                        return f1 + f2;
                    case "-":
                        return f1 - f2;
                    case "÷":
                        return f1 / f2;
                    case "X":
                        return f1 * f2;
                    case "mod":
                        return new Fraction(f1.ToDouble() % f2.ToDouble());
                }
            }
            else
            {
                double n1 = (double)op1;
                double n2 = (double)op2;
                switch (op)
                {
                    case "+":
                        return n1 + n2;
                    case "-":
                        return n1 - n2;
                    case "÷":
                        return n1 / n2;
                    case "X":
                        return n1 * n2;
                    case "mod":
                        return n1 % n2;
                }
            }
            //default return
            return null;
        }

        private object HandleBinOperators(object op1, object op2, string op)
        {
            var t1 = op1.GetType().FullName;
            var t2 = op2.GetType().FullName;

            if (t1 != "System.Double" || t2 != "System.Double") throw new ArgumentException("Bit operators only supported on regular numbers");

            //used for checking & discarding floating point values
            double d1 = Math.Truncate((double)op1);
            double d2 = Math.Truncate((double)op2);

            //long & ulong value registers
            long sop1 = 0;
            long sop2 = 0;
            long sres = 0;
            ulong uop1 = 0;
            ulong uop2 = 0;
            ulong ures = 0;

            switch (Engine.BitEngineMode)
            {
                case BitEngineModes.Signed8bit:
                case BitEngineModes.Signed16bit:
                case BitEngineModes.Signed32bit:
                case BitEngineModes.Signed64bit:
                    if (d1 > long.MaxValue || d2 > long.MaxValue || d1 < long.MinValue || d2 < long.MinValue)
                        throw new ArgumentException("Input parameter is too large or too small for operations");
                    sop1 = Convert.ToInt64(d1);
                    sop2 = Convert.ToInt64(d2);

                    switch (op)
                    {
                        case "and":
                            sres = sop1 & sop2;
                            break;
                        case "or":
                            sres = sop1 | sop2;
                            break;
                        case "xor":
                            sres = sop1 ^ sop2;
                            break;
                        case "eq":
                            sres = ~(sop1 ^ sop2);
                            break;
                    }
                    break;
                case BitEngineModes.Unsigned8bit:
                case BitEngineModes.Unsigned16bit:
                case BitEngineModes.Unsigned32bit:
                case BitEngineModes.Unsigned64bit:
                    if (d1 > ulong.MaxValue || d2 > ulong.MaxValue || d1 < ulong.MinValue || d2 < ulong.MinValue)
                        throw new ArgumentException("Input parameter is too large or too small for operations");
                    uop1 = Convert.ToUInt64(d1);
                    uop2 = Convert.ToUInt64(d2);

                    switch (op)
                    {
                        case "and":
                            ures = uop1 & uop2;
                            break;
                        case "or":
                            ures = uop1 | uop2;
                            break;
                        case "xor":
                            ures = uop1 ^ uop2;
                            break;
                        case "eq":
                            ures = ~(uop1 ^ uop2);
                            break;
                    }
                    break;
            }

            switch (Engine.BitEngineMode)
            {
                case BitEngineModes.Signed8bit:
                    HadOwerFlow = (sres > sbyte.MaxValue || sres < sbyte.MinValue) ;
                    return (sres & 0x00000000000000FF);
                case BitEngineModes.Signed16bit:
                    HadOwerFlow = (sres > short.MaxValue || sres < short.MinValue);
                    return (sres & 0x000000000000FFFF);
                case BitEngineModes.Signed32bit:
                    HadOwerFlow = (sres > int.MaxValue || sres < int.MinValue);
                    return (sres & 0x00000000FFFFFFFF);
                case BitEngineModes.Signed64bit:
                    HadOwerFlow = (sres > long.MaxValue || sres < long.MinValue);
                    return sres;
                case BitEngineModes.Unsigned8bit:
                    HadOwerFlow = (ures > byte.MaxValue || ures < byte.MinValue);
                    return (ures & 0x00000000000000FF);
                case BitEngineModes.Unsigned16bit:
                    HadOwerFlow = (ures > ushort.MaxValue || ures < ushort.MinValue);
                    return (ures & 0x000000000000FFFF);
                case BitEngineModes.Unsigned32bit:
                    HadOwerFlow = (ures > uint.MaxValue || ures < uint.MinValue);
                    return (ures & 0x00000000FFFFFFFF);
                case BitEngineModes.Unsigned64bit:
                    HadOwerFlow = (ures > ulong.MaxValue || ures < ulong.MinValue);
                    return ures;
            }

            return null;
        }
    }
}
