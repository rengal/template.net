using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers.Wrappers;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class ReadOnlyCollectionSerializer<T> : BaseReferenceTypeSerializer<ReadOnlyCollection<T>>
    {
        private readonly ISerializerWrapper<T> itemSerializer = SerializerWrapper.GetFor<T>();

        public override ReadOnlyCollection<T> DeepClone([CanBeNull] ReadOnlyCollection<T> value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (value == null)
                return null;

            var clone = new T[value.Count];

            for (var i = 0; i < value.Count; i++)
            {
                clone[i] = itemSerializer.DeepClone(value[i], false, context);
            }

            return clone.AsReadOnly();
        }

        public override void WriteElementContent(XmlWriter writer, ReadOnlyCollection<T> value, bool byValue, ISerializationContext context, SerializationMetadata metadata)
        {
            CollectionSerializationHelper.WriteElementContent(writer, value, byValue, context, metadata, itemSerializer);
        }

        public override ReadOnlyCollection<T> ReadElementContent(XmlReader reader, bool byValue, IDeserializationContext context, SerializationMetadata metadata)
        {
            return CollectionSerializationHelper.ReadElementContent<List<T>, T>(reader, byValue, context, metadata, itemSerializer).ToArray().AsReadOnly();
        }

        public override string ToDebugString(ReadOnlyCollection<T> value, int depth)
        {
            return CollectionSerializationHelper.ToDebugString(value, depth, value.Count, itemSerializer);
        }
    }
}
