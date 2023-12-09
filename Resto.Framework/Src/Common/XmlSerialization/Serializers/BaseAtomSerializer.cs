using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    public abstract class BaseAtomSerializer<T> : BaseSerializer<T>
    {
        #region Implementation of ISerializer<T>
        public sealed override T DeepClone(T value, bool byValue, [NotNull] IDeserializationContext context)
        {
            return value;
        }

        public sealed override void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, T value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            writer.WriteStartElement(name);
            WriteElement(writer, defaultType, value);
            writer.WriteEndElement();
        }

        public sealed override void WriteElementContent([NotNull] XmlWriter writer, T value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            WriteElementContent(writer, value);
        }

        public sealed override T ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            return ReadElement(reader);
        }

        public sealed override T ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            return ReadElementContent(reader);
        }

        [NotNull]
        public sealed override string ToDebugString(T value, int depth)
        {
            return ToString(value, true);
        }
        #endregion

        protected T ReadElementContent([NotNull] XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (reader.IsEmptyElement)
                return DeserializeFromString(string.Empty);

            var value = string.Empty;
            while (reader.Read())
            {
                var nodeType = reader.NodeType;
                if (nodeType == XmlNodeType.EndElement)
                    break;

                switch (nodeType)
                {
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Whitespace:
                        value += reader.Value;
                        break;
                    case XmlNodeType.Element:
                        throw new SerializerException(string.Format("Unexpected element '{0}' with value '{1}' in atom field of type '{2}'", reader.Name, reader.Value, typeof(T)));
                }
            }

            return DeserializeFromString(value);
        }

        [NotNull]
        [Pure]
        protected abstract string ToString(T value, bool forDebug);

        [Pure]
        protected abstract T DeserializeFromString([NotNull] string text);

        protected abstract void WriteElement([NotNull] XmlWriter writer, [NotNull] Type type, T value);
        protected abstract void WriteElementContent([NotNull] XmlWriter writer, T value);
        protected abstract T ReadElement([NotNull] XmlReader reader);
    }
}