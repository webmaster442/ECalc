using System;
using System.Xml.Serialization;

namespace ECalc.Classes
{
    [Serializable]
    [XmlRoot("Function")]
    public class UserFuntion
    {
        /// <summary>
        /// User function name
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }
        /// <summary>
        /// User function argument count
        /// </summary>
        [XmlAttribute("Arguments")]
        public int ArgCount { get; set; }
        /// <summary>
        /// User function run commands
        /// </summary>
        [XmlText]
        public string Commands { get; set; }
    }
}
