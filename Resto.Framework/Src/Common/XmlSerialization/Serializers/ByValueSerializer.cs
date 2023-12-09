using System;
using System.Diagnostics;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class ByValueSerializer<T> : BaseSerializer<ByValue<T>> where T : CachedEntity
    {
        [CanBeNull]
        private readonly ISerializer<T> valueSerializer;

        public ByValueSerializer()
        {
            if (typeof(T).IsSealed)
                valueSerializer = SerializersManager.GetSerializer<T>();
        }

        [CanBeNull]
        public override ByValue<T> DeepClone([CanBeNull] ByValue<T> value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (value == null)
                return null;

            if (valueSerializer != null)
                return new ByValue<T>(valueSerializer.DeepClone(value.Value, true, context));

            if (value.Value.GetType() == typeof(T))
                return new ByValue<T>(SerializersManager.GetSerializer<T>().DeepClone(value.Value, true, context));

            var clone = SerializersManager.GetSerializer(value.Value.GetType()).DeepClone(value.Value, true, context);
            Debug.Assert(clone != null);

            return new ByValue<T>((T)clone);
        }

        public override void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, [CanBeNull] ByValue<T> value, bool byValue,
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
            if (defaultType != typeof(ByValue<T>))
                throw new ArgumentOutOfRangeException(nameof(defaultType), defaultType, "Invalid type");

            if (value == null)
            {
                writer.WriteStartElement(name);
                WriteNullValue(writer);
                writer.WriteEndElement();
                return;
            }

            if (value.Value.GetType() == typeof(T))
            {
                writer.WriteStartElement(name);
                WriteElementContent(writer, value, true, context, SerializationMetadata.Empty);
                writer.WriteEndElement();
                return;
            }

            SerializersManager
                .GetSerializer(value.Value.GetType())
                .WriteObjectToElement(name, writer, typeof(T), value.Value, true, context, SerializationMetadata.Empty);
        }

        public override void WriteElementContent([NotNull] XmlWriter writer, [NotNull] ByValue<T> value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (value.Value.GetType() != typeof(T))
                throw new ArgumentException(string.Format("Type of Value property in value ({0}) must be equal to serializer generic type ({1})", value.Value.GetType(), typeof(T)), nameof(value));

            (valueSerializer ?? SerializersManager.GetSerializer<T>()).WriteElementContent(writer, value.Value, true, context, SerializationMetadata.Empty);
        }

        [CanBeNull]
        public override ByValue<T> ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (IsNullValue(reader))
                return null;

            var type = typeof(T);

            var typeName = ReadTypeAttribute(reader);
            if (typeName == null || type.IsSealed)
            {
                return new ByValue<T>((valueSerializer ?? SerializersManager.GetSerializer<T>()).ReadElementContent(reader, true, context, SerializationMetadata.Empty));
            }

            var actualType = EntitiesRegistryBase.GetType(typeName);
            if (actualType.IsGenericTypeDefinition && type.IsInterface && type.IsGenericType)
            {
                actualType = actualType.MakeGenericType(type.GetGenericArguments());
            }
            else if (!type.IsAssignableFrom(actualType))
            {
                throw new SerializerException("Type " + actualType + " is not a subtype of " + type);
            }

            var converter = SerializersManager.GetSerializer(actualType);
            return new ByValue<T>((T)converter.ReadObjectContentFromElement(reader, true, context, SerializationMetadata.Empty));
        }

        [NotNull]
        public override ByValue<T> ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            var value = valueSerializer != null
                            ? valueSerializer.ReadElementContent(reader, true, context, SerializationMetadata.Empty)
                            : SerializersManager.GetSerializer<T>().ReadElementContent(reader, true, context, SerializationMetadata.Empty);

            return new ByValue<T>(value);
        }

        [NotNull]
        [Pure]
        public override string ToDebugString([NotNull] ByValue<T> value, int depth)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return valueSerializer != null 
                ? valueSerializer.ToDebugString(value.Value, depth) 
                : SerializersManager.GetSerializer(typeof(T)).ObjectToDebugString(value.Value, depth);
        }
    }
}