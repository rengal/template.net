using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    public abstract class BaseSerializer<T> : Serializer, ISerializer<T>
    {
        #region Implementation of ISerializer
        public object DeepClone(object value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return DeepClone((T)value, byValue, context);
        }

        public void WriteObjectToElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type type, [CanBeNull] object value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            if (value == null)
            {
                WriteElement(name, writer, type, default(T), byValue, context, metadata);
                return;
            }

            T typedValue;
            try
            {
                typedValue = (T)value;
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException(string.Format("Type of value ({0}) must be equal to serializer generic type ({1})", value.GetType(), typeof(T)), nameof(value), e);
            }

            WriteElement(name, writer, type, typedValue, byValue, context, metadata);
        }

        [NotNull]
        public object ReadObjectContentFromElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return ReadElementContent(reader, byValue, context, metadata);
        }

        [NotNull]
        [Pure]
        public string ObjectToDebugString([NotNull] object value, int depth)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return ToDebugString((T)value, depth);
        }
        #endregion

        #region Implementation of ISerializer<T>
        public abstract T DeepClone(T value, bool byValue, [NotNull] IDeserializationContext context);

        public abstract void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, T value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata);
        public abstract void WriteElementContent([NotNull] XmlWriter writer, T value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata);
        public abstract T ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata);
        public abstract T ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata);

        [NotNull]
        [Pure]
        public abstract string ToDebugString(T value, int depth);
        #endregion
    }
}