using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Resto.Framework.WcfCommon.Internationalization
{
    public sealed class Preferences : IXmlSerializable
    {
        private XmlNode anyElement;

        public XmlNode AnyElement
        {
            get { return anyElement; }
            set { anyElement = value; }
        }
        
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var document = new XmlDocument();
            anyElement = document.ReadNode(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            anyElement.WriteTo(writer);
        }
    }
}
