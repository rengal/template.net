using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    public interface ISerializer
    {
        [CanBeNull]
        object DeepClone([CanBeNull] object value, bool byValue, [NotNull] IDeserializationContext context);

        void WriteObjectToElement([NotNull] string name, [NotNull] XmlWriter writer, [NotNull] Type type, [CanBeNull] object value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata);

        [NotNull]
        object ReadObjectContentFromElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata);

        [NotNull]
        [Pure]
        string ObjectToDebugString([NotNull] object value, int depth);
    }
}