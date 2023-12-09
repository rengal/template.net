using System;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    public struct SerializationMetadata
    {
        public static SerializationMetadata Empty => new SerializationMetadata();

        [CanBeNull]
        public Type DefaultCollectionItemType { get; }

        [CanBeNull]
        public Type DefaultDictionaryKeyType { get; }

        [CanBeNull]
        public Type DefaultDictionaryValueType { get; }

        public bool ForceTypeAttribute { get; }

        public SerializationMetadata([CanBeNull] Type defaultCollectionItemType = null, [CanBeNull] Type defaultDictionaryKeyType = null, [CanBeNull] Type defaultDictionaryValueType = null, bool forceTypeAttribute = false)
        {
            DefaultCollectionItemType = defaultCollectionItemType;
            DefaultDictionaryKeyType = defaultDictionaryKeyType;
            DefaultDictionaryValueType = defaultDictionaryValueType;
            ForceTypeAttribute = forceTypeAttribute;
        }
    }
}