using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers.Wrappers;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal static class CollectionSerializationHelper
    {
        private const string ItemTag = "i";

        public static void WriteElementContent<TCollection, TItem>([NotNull] XmlWriter writer, [NotNull] TCollection value, bool serializeByValue,
           [NotNull] ISerializationContext context, SerializationMetadata metadata, [NotNull] ISerializerWrapper<TItem> itemSerializer)
            where TCollection : IEnumerable<TItem>
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (itemSerializer == null)
                throw new ArgumentNullException(nameof(itemSerializer));

            // RMS-48020: требование выводить конкретный тип в xml относится более к элементам коллекций, чем к самим полям коллекций.
            var childMetadata = new SerializationMetadata(forceTypeAttribute: metadata.ForceTypeAttribute);

            if (metadata.DefaultCollectionItemType != null)
            {
                var defaultItemType = metadata.DefaultCollectionItemType;

                foreach (var item in value)
                    itemSerializer.WriteElement(ItemTag, writer, defaultItemType, item, serializeByValue, context, childMetadata);
            }
            else
            {
                foreach (var item in value)
                    itemSerializer.WriteElement(ItemTag, writer, item, serializeByValue, context, childMetadata);
            }
        }

        public static TCollection ReadElementContent<TCollection, TItem>([NotNull] XmlReader reader, bool deserializeByValue,
            [NotNull] IDeserializationContext context, SerializationMetadata metadata, [NotNull] ISerializerWrapper<TItem> itemSerializer)
            where TCollection : class, ICollection<TItem>, new()
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (itemSerializer == null)
                throw new ArgumentNullException(nameof(itemSerializer));

            var result = new TCollection();

            if (reader.IsEmptyElement)
                return result;

            var defaultItemType = metadata.DefaultCollectionItemType;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!ItemTag.Equals(reader.Name))
                            throw new SerializerException("Collection item element must be named <" + ItemTag + ">, not " + reader.Name);

                        var item = defaultItemType == null
                            ? itemSerializer.ReadElement(reader, deserializeByValue, context, SerializationMetadata.Empty)
                            : itemSerializer.ReadElement(reader, defaultItemType, deserializeByValue, context, SerializationMetadata.Empty);

                        result.Add(item);

                        break;

                    case XmlNodeType.EndElement:
                        return result;
                }
            }

            throw new SerializerException("Unexpected end of data");
        }

        public static string ToDebugString<TCollection, TItem>([NotNull] TCollection value, int depth, int count, [NotNull] ISerializerWrapper<TItem> itemSerializer)
            where TCollection : IEnumerable<TItem>
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (itemSerializer == null)
                throw new ArgumentNullException(nameof(itemSerializer));

            if (depth == 0)
                return "[..." + count + "]";

            var builder = new StringBuilder();

            var childDepth = depth - 1;
            foreach (var item in value)
            {
                if (builder.Length > 0)
                    builder.Append(", ");

                builder.Append(itemSerializer.ToDebugString(item, childDepth));
            }

            return "[" + builder + "]";
        }
    }
}
