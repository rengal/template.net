using System;
using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers.Wrappers;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class ArraySerializer<T> : BaseReferenceTypeSerializer<T[]>
    {
        private readonly ISerializerWrapper<T> itemSerializer = SerializerWrapper.GetFor<T>();

        public override T[] DeepClone(T[] value, bool byValue, IDeserializationContext context)
        {
            if (value == null)
                return null;

            var clone = new T[value.Length];

            for (var i = 0; i < value.Length; i++)
            {
                clone[i] = itemSerializer.DeepClone(value[i], false, context);
            }

            return clone;
        }

        public override void WriteElementContent(XmlWriter writer, T[] value, bool byValue, ISerializationContext context, SerializationMetadata metadata)
        {
            CollectionSerializationHelper.WriteElementContent(writer, value, byValue, context, metadata, itemSerializer);
        }

        public override T[] ReadElementContent(XmlReader reader, bool byValue, IDeserializationContext context, SerializationMetadata metadata)
        {
            var result = CollectionSerializationHelper.ReadElementContent<List<T>, T>(reader, byValue, context, metadata, itemSerializer);
            return result.Count == 0 ? Array.Empty<T>() : result.ToArray();
        }

        public override string ToDebugString(T[] value, int depth)
        {
            return CollectionSerializationHelper.ToDebugString(value, depth, value.Length, itemSerializer);
        }
    }
}
