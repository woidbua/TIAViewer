using System;
using System.Collections.Generic;
using System.Xml;
using TIAViewer.Enums;

namespace TIAViewer.Model
{
    public class GraphItem
    {
        public GraphItem(XmlNode xmlNode)
        {
            Element = ExtractElement(xmlNode);
            Type = ExtractType(xmlNode);
            Properties = ExtractProperties(xmlNode);
        }


        public GraphItemElement Element { get; }

        public string Type { get; }

        public Dictionary<string, string> Properties { get; }

        private GraphItemElement ExtractElement(XmlNode xmlNode)
        {
            string nodeName = char.ToUpper(xmlNode.Name[0]) + xmlNode.Name.Substring(1);
            return nodeName == Enum.GetName(typeof(GraphItemElement), GraphItemElement.Edge)
                ? GraphItemElement.Edge
                : GraphItemElement.Node;
        }


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
                string key = propertyNode.SelectSingleNode("key")?.InnerText;
                string value = propertyNode.SelectSingleNode("value")?.InnerText;

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    properties.Add(key, value);
                }
            }

            return properties;
        }
    }
}