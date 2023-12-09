using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Resto.Framework.Data
{
    public class SerializableTimeSpan : IXmlSerializable
    {
        private TimeSpan timeSpan;

        public static implicit operator SerializableTimeSpan(TimeSpan value)
        {
            return new SerializableTimeSpan { timeSpan = value };
        }

        public static implicit operator TimeSpan(SerializableTimeSpan value)
        {
            return value.timeSpan;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            timeSpan = TimeSpan.Parse(reader.ReadElementContentAsString());        
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteValue(timeSpan.ToString());
        }

        public override string ToString()
        {
            return timeSpan.ToString();
        }
    }

    public class SerializableTimeSpanConverter : TimeSpanConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var obj = base.ConvertFrom(context, culture, value);
            if (obj == null) return null;
            SerializableTimeSpan timeSpan = (TimeSpan)obj;
            return timeSpan;
        }
    }
}
