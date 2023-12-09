using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers.Wrappers;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal abstract class BaseCollectionSerializer<T, TItem> : BaseReferenceTypeSerializer<T> where T : class, ICollection<TItem>, new()
    {
        [NotNull]
        private readonly ISerializerWrapper<TItem> itemSerializer;

        protected BaseCollectionSerializer()
        {
            itemSerializer = SerializerWrapper.GetFor<TItem>();
        }

        protected abstract bool ByValue { get; }

        [CanBeNull]
        public sealed override T DeepClone([CanBeNull] T value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (value == null)
                return null;

            var clone = new T();

            var cloneByValue = ByValue;
            foreach (var item in value)
            {
                var itemClone = itemSerializer.DeepClone(item, cloneByValue, context);
                clone.Add(itemClone);
            }

            return clone;
        }

        public sealed override void WriteElementContent([NotNull] XmlWriter writer, [NotNull] T value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            CollectionSerializationHelper.WriteElementContent(writer, value, ByValue, context, metadata, itemSerializer);
        }

        public sealed override T ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            return CollectionSerializationHelper.ReadElementContent<T, TItem>(reader, ByValue, context, metadata, itemSerializer);
        }

        public sealed override string ToDebugString([NotNull] T value, int depth)
        {
            return CollectionSerializationHelper.ToDebugString(value, depth, value.Count, itemSerializer);
        }
    }
}