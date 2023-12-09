using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers.Wrappers;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class DictionarySerializer<T, TKey, TValue> : BaseReferenceTypeSerializer<T>
        where T : class, IDictionary<TKey, TValue>, new()
    {
        private const string KeyTag = "k";
        private const string ValTag = "v";

        [NotNull]
        private readonly ISerializerWrapper<TKey> keySerializer = SerializerWrapper.GetFor<TKey>();
        [NotNull]
        private readonly ISerializerWrapper<TValue> valueSerializer = SerializerWrapper.GetFor<TValue>();

        [CanBeNull]
        public override T DeepClone([CanBeNull] T value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (value == null)
                return null;

            var clone = new T();

            foreach (var kvp in value)
            {
                var key = keySerializer.DeepClone(kvp.Key, false, context);
                var val = valueSerializer.DeepClone(kvp.Value, false, context);
                clone.Add(key, val);
            }

            return clone;
        }

        public override void WriteElementContent([NotNull] XmlWriter writer, [NotNull] T value, bool byValue,
            [NotNull] ISerializationContext context, SerializationMetadata metadata)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var defaultKeyType = metadata.DefaultDictionaryKeyType;
            var defaultValueType = metadata.DefaultDictionaryValueType;

            foreach (var kvp in value)
            {
                if (defaultKeyType != null)
                    keySerializer.WriteElement(KeyTag, writer, defaultKeyType, kvp.Key, false, context, SerializationMetadata.Empty);
                else
                    keySerializer.WriteElement(KeyTag, writer, kvp.Key, false, context, SerializationMetadata.Empty);

                if (defaultValueType != null)
                    valueSerializer.WriteElement(ValTag, writer, defaultValueType, kvp.Value, false, context, SerializationMetadata.Empty);
                else
                    valueSerializer.WriteElement(ValTag, writer, kvp.Value, false, context, SerializationMetadata.Empty);
            }
        }

        [NotNull]
        public override T ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var result = new T();

            if (reader.IsEmptyElement)
                return result;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name != KeyTag)
                            throw new SerializerException("Dictionary key element should be named <" + KeyTag + ">, not " + reader.Name);

                        var keyItem = metadata.DefaultDictionaryKeyType != null
                            ? keySerializer.ReadElement(reader, metadata.DefaultDictionaryKeyType, false, context, SerializationMetadata.Empty)
                            : keySerializer.ReadElement(reader, false, context, SerializationMetadata.Empty);
                        var valueItem = ReadValue(reader, context, metadata);
                        // ReSharper disable CompareNonConstrainedGenericWithNull
                        if (keyItem != null)
                        // ReSharper restore CompareNonConstrainedGenericWithNull
                        {
                            if (result.ContainsKey(keyItem))
                            {
                                LogWrapper.Log.WarnFormat(
                                    "Key already exists. Can not add item with key '{0}' and value '{1}'",
                                    keySerializer.ToDebugString(keyItem, 5),
                                    valueSerializer.ToDebugString(valueItem, 5));
                            }
                            else
                            {
                                result.Add(keyItem, valueItem);
                            }
                        }
                        else
                        {
                            LogWrapper.Log.Warn("Null key in dictionary: " + typeof(TKey));
                        }
                        break;

                    case XmlNodeType.EndElement:
                        return result;
                }
            }

            throw new SerializerException("Unexpected end of data");
        }

        private TValue ReadValue([NotNull] XmlReader reader, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            Debug.Assert(reader != null);
            Debug.Assert(context != null);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name != ValTag)
                            throw new SerializerException("Dictionary value element should be named <" + ValTag + ">, not " + reader.Name);

                        return metadata.DefaultDictionaryValueType != null
                            ? valueSerializer.ReadElement(reader, metadata.DefaultDictionaryValueType, false, context, SerializationMetadata.Empty)
                            : valueSerializer.ReadElement(reader, false, context, SerializationMetadata.Empty);

                    case XmlNodeType.EndElement:
                        throw new SerializerException("Cannot read dictionary value: unexpected close tag");
                }
            }

            throw new SerializerException("Unexpected end of data");
        }

        [NotNull]
        [Pure]
        public override string ToDebugString([NotNull] T value, int depth)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (depth == 0)
                return "{..." + value.Count + "}";

            var builder = new StringBuilder();

            var childDepth = depth - 1;

            foreach (var kvp in value)
            {
                if (builder.Length > 0)
                    builder.Append("; ");

                builder.Append(keySerializer.ToDebugString(kvp.Key, childDepth));
                builder.Append("=");
                builder.Append(valueSerializer.ToDebugString(kvp.Value, childDepth));
            }

            return "{" + builder + "}";
        }
    }
}