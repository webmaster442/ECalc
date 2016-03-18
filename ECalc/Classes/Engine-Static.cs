using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Linq;

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
        /// Function list
        /// </summary>
        private static List<IFunction> _functions;

        /// <summary>
        /// User function list
        /// </summary>
        public static List<UserFuntion> UserFunctions
        {
            get;
            private set;
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

            _functions = new List<IFunction>();
            UserFunctions = new List<UserFuntion>();

            try
            {
                var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where
                        t.IsClass &&
                        t.Namespace == "ECalc.Maths" &&
                        t.GetInterfaces().Contains(typeof(IFunction))
                        select t;

                foreach (var item in q)
                {
                    var fnc = (IFunction)Activator.CreateInstance(item);
                    _functions.Add(fnc);
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorDialog(ex.Message);
            }
            ConfigFileHelpers.LoadUserFunctions();
        }
    }
}
