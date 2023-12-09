using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    /// <summary>
    /// Интерфейс для сериализации/десериализации значений типа <typeparamref name="T" /> в Xml
    /// </summary>
    /// <typeparam name="T">Тип сериализуемых значений</typeparam>
    public interface ISerializer<T> : ISerializer
    {
        T DeepClone(T value, bool byValue, [NotNull] IDeserializationContext context);

        void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, T value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata);
        void WriteElementContent([NotNull] XmlWriter writer, T value, bool byValue, [NotNull] ISerializationContext context, SerializationMetadata metadata);

        T ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata);
        T ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata);

        [NotNull]
        [Pure]
        string ToDebugString(T value, int depth);
    }
}