using System;
using System.Diagnostics;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class NullableValueTypeSerializer<T> : BaseSerializer<T?> where T : struct
    {
        [NotNull]
        private readonly ISerializer<T> serializer;

        public NullableValueTypeSerializer([NotNull] ISerializer<T> serializer)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            this.serializer = serializer;
        }

        #region Methods
        public override T? DeepClone(T? value, bool byValue, [NotNull] IDeserializationContext context)
        {
            return value;
        }

        public override void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, [CanBeNull] T? value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (defaultType == null)
                throw new ArgumentNullException(nameof(defaultType));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            
            writer.WriteStartElement(name);
            if (!value.HasValue)
            {
                WriteNullValue(writer);
            }
            else
            {
                Debug.Assert(defaultType == typeof(T?));

                serializer.WriteElementContent(writer, value.Value, byValue, context, SerializationMetadata.Empty);
            }
            writer.WriteEndElement();
        }

        public override void WriteElementContent([NotNull] XmlWriter writer, [NotNull] T? value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            
            serializer.WriteElementContent(writer, value.Value, byValue, context, SerializationMetadata.Empty);
        }

        [CanBeNull]
        public override T? ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            if (IsNullValue(reader))
                return null;

            return serializer.ReadElementContent(reader, byValue, context, SerializationMetadata.Empty);
        }

        [NotNull]
        public override T? ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            return serializer.ReadElementContent(reader, byValue, context, SerializationMetadata.Empty);
        }

        [NotNull]
        public override string ToDebugString([NotNull] T? value, int depth)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));


            return serializer.ToDebugString(value.Value, depth);
        }
        #endregion
    }
}