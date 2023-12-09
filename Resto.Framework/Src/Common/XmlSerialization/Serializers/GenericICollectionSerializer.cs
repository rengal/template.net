using System;
using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class GenericICollectionSerializer<T> : ISerializer<ICollection<T>>
    {
        #region Fields
        [NotNull]
        private readonly ISerializer<List<T>> serializer;
        #endregion

        #region Ctor
        public GenericICollectionSerializer()
        {
            serializer = SerializersManager.GetSerializer<List<T>>();
        }
        #endregion

        #region Methods

        #region ISerializer
        public object DeepClone(object value, bool byValue, IDeserializationContext context)
        {
            throw new NotSupportedException();
        }

        public void WriteObjectToElement(string name, XmlWriter writer, Type type, object value, bool byValue, ISerializationContext context, SerializationMetadata metadata)
        {
            throw new NotSupportedException();
        }

        [NotNull]
        public object ReadObjectContentFromElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return serializer.ReadElementContent(reader, byValue, context, metadata);
        }

        public string ObjectToDebugString(object value, int depth)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region ISerializer<T>
        public ICollection<T> DeepClone(ICollection<T> value, bool byValue, IDeserializationContext context)
        {
            throw new NotSupportedException();
        }

        public void WriteElement(string name, XmlWriter writer, Type defaultType, ICollection<T> value, bool byValue, ISerializationContext context, SerializationMetadata metadata)
        {
            throw new NotSupportedException();
        }

        public void WriteElementContent(XmlWriter writer, ICollection<T> value, bool byValue, ISerializationContext context, SerializationMetadata metadata)
        {
            throw new NotSupportedException();
        }

        [CanBeNull]
        public ICollection<T> ReadElement([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            return serializer.ReadElement(reader, byValue, context, metadata);
        }

        [NotNull]
        public ICollection<T> ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            return serializer.ReadElementContent(reader, byValue, context, metadata);
        }

        public string ToDebugString(ICollection<T> value, int depth)
        {
            throw new NotSupportedException();
        }
        #endregion

        #endregion
    }
}