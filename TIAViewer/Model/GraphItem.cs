using System.Collections.Generic;
using System.Xml;

namespace TIAViewer.Model
{
    public abstract class GraphItem
    {
        protected GraphItem(XmlNode xmlNode)
        {
            Type = ExtractType(xmlNode);
            Properties = ExtractProperties(xmlNode);
            Name = SetName();
        }


        public string Type { get; }

        public Dictionary<string, string> Properties { get; }

        public string Name { get; }


        private string ExtractType(XmlNode xmlNode)
        {
            return xmlNode.Attributes?["Type"].Value;
        }


        private Dictionary<string, string> ExtractProperties(XmlNode xmlNode)
        {
            var properties = new Dictionary<string, string>();

            var propertyNodes = xmlNode.SelectNodes("properties/property");
            if (propertyNodes == null) return properties;

            foreach (XmlNode propertyNode in propertyNodes)
            {
                var key = propertyNode.SelectSingleNode("key")?.InnerText;
                var value = propertyNode.SelectSingleNode("value")?.InnerText;

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    properties.Add(key, value);
                }
            }

            return properties;
        }

        private string SetName()
        {
            return Properties.ContainsKey("Name") ? Properties["Name"] : Properties["Id"];
        }
    }
}