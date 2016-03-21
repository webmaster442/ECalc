using System;

namespace ECalc.Classes
{
    [Serializable]
    public class UserFuntion
    {
        /// <summary>
        /// User function name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// User function argument count
        /// </summary>
        public int ArgCount { get; set; }
        /// <summary>
        /// User function run commands
        /// </summary>
        public string Commands { get; set; }
    }
}
