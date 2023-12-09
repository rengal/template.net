using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    public abstract class BaseReferenceTypeSerializer<T> : BaseSerializer<T> where T : class
    {
        public sealed override void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, [CanBeNull] T value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            writer.WriteStartElement(name);
            if (value == null)
            {
                WriteNullValue(writer);
            }
            else
            {
                if (metadata.ForceTypeAttribute || defaultType != typeof(T))
                    WriteTypeAttribute(writer, typeof(T));

                WriteElementContent(writer, value, byValue, context, metadata);
            }
            writer.WriteEndElement();
        }

        [CanBeNull]
        public sealed override T ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            return IsNullValue(reader) ? null : ReadElementContent(reader, byValue, context, metadata);
        }
    }
}