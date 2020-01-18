using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TIAViewer.Services
{
    public static class TiaFileService
    {
        public static List<XmlNode> ExtractGraphItems(string filepath)
        {
            var doc = new XmlDocument();
            doc.Load(filepath);

            XmlElement root = doc.DocumentElement;

            var xmlNodeList = new List<XmlNode>();
            xmlNodeList.AddRange(ExtractElementsByTagName(root, "node"));
            xmlNodeList.AddRange(ExtractElementsByTagName(root, "edge"));

            return xmlNodeList;
        }

        private static IEnumerable<XmlNode> ExtractElementsByTagName(XmlElement root, string name)
        {
            XmlNodeList xmlNodeList = root?.GetElementsByTagName(name);
            return xmlNodeList != null ? xmlNodeList.Cast<XmlNode>() : new List<XmlNode>();
        }
    }
}