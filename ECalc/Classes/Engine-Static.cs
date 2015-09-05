using System;
using System.Globalization;

namespace ECalc.Classes
{
    //static part of the engine class
    internal partial class Engine
    {
        /// <summary>
        /// Trigonometry Mode
        /// </summary>
        public static TrigMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// Last calculation result as object
        /// </summary>
        public static Object Ans
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the bit engine mode
        /// </summary>
        public static BitEngineModes BitEngineMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets wheather there was an oweflow in the BitEngine Calculations
        /// </summary>
        public static bool HadOwerFlow
        {
            get;
            private set;
        }

        /// <summary>
        /// Decimal sepperator char on the current culture
        /// </summary>
        public static string DecimalSeperator
        {
            get;
            private set;
        }

        /// <summary>
        /// Number group sepperator
        /// </summary>
        public static string NumberGroupSeparator
        {
            get;
            private set;
        }

        /// <summary>
        /// If enabled the double number output will be formatted using prefix values.
        /// </summary>
        public static bool PreferPrefixes
        {
            get;
            set;
        }

        /// <summary>
        /// Static ctor
        /// </summary>
        static Engine()
        {
            DecimalSeperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            NumberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            HadOwerFlow = false;
            BitEngineMode = BitEngineModes.Signed32bit;
            PreferPrefixes = false;
        }
    }
}
