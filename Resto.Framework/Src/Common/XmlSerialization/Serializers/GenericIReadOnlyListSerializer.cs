using System;
using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers.Wrappers;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class GenericIReadOnlyListSerializer<T> : BaseReferenceTypeSerializer<IReadOnlyList<T>>
    {
        private readonly ISerializerWrapper<T> itemSerializer = SerializerWrapper.GetFor<T>();

        public override IReadOnlyList<T> DeepClone([CanBeNull] IReadOnlyList<T> value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (value == null)
                return null;
            if (value.Count == 0)
                return Array.Empty<T>();

            var clone = new T[value.Count];
            for (var i = 0; i < value.Count; i++)
            {
                clone[i] = itemSerializer.DeepClone(value[i], false, context);
            }

            return clone;
        }

        public override void WriteElementContent(XmlWriter writer, IReadOnlyList<T> value, bool byValue, ISerializationContext context, SerializationMetadata metadata)
        {
            CollectionSerializationHelper.WriteElementContent(writer, value, byValue, context, metadata, itemSerializer);
        }

        public override IReadOnlyList<T> ReadElementContent(XmlReader reader, bool byValue, IDeserializationContext context, SerializationMetadata metadata)
        {
            var result = CollectionSerializationHelper.ReadElementContent<List<T>, T>(reader, byValue, context, metadata, itemSerializer);
            if (result.Count == 0)
                return Array.Empty<T>();

            return result;
        }

        public override string ToDebugString(IReadOnlyList<T> value, int depth)
        {
            return CollectionSerializationHelper.ToDebugString(value, depth, value.Count, itemSerializer);
        }
    }
}
