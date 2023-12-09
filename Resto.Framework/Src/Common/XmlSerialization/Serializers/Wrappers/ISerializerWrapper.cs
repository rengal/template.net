using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;

namespace Resto.Framework.Common.XmlSerialization.Serializers.Wrappers
{
    internal interface ISerializerWrapper<T>
    {
        [NotNull, Pure]
        string ToDebugString(T value, int depth);

        void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [CanBeNull] T value, bool byValue, [NotNull] ISerializationContext context, SerializationMetadata metadata);

        void WriteElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type defaultType, [CanBeNull] T value, bool byValue, [NotNull] ISerializationContext context, SerializationMetadata metadata);

        [CanBeNull]
        T ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata);

        [CanBeNull]
        T ReadElement([NotNull] XmlReader reader, [NotNull] Type defaultType, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata);

        T DeepClone(T value, bool byValue, [NotNull] IDeserializationContext context);
    }
}