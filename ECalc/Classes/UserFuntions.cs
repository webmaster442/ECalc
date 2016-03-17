using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ECalc.Classes
{
    [Serializable]
    public class UserFuntion : IXmlSerializable
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute("Name");
            ArgCount = Convert.ToInt32(reader.GetAttribute("Arguments"));
            Commands = reader.ReadElementContentAsString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("Arguments", ArgCount.ToString());
            writer.WriteElementString("Code", Commands);
        }
    }
}
