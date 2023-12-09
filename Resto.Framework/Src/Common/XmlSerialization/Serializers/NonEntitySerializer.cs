using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class NonEntitySerializer<T> : BaseCompoundSerializer<T> where T : class
    {
        public NonEntitySerializer()
        {
            if (typeof(Entity).IsAssignableFrom(typeof(T)))
                throw new SerializerException(string.Format("Type {0} cannot derive from Entity", typeof(T)));
        }

        [CanBeNull]
        public override T DeepClone([CanBeNull] T value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (value == null)
                return null;


            var clone = CreateInstance();
            DeepCopyFields(value, clone, context);
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


            WriteFields(writer, value, context);
        }

        [NotNull]
        public override T ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.DeserializationStack.PushEntity<T>(null);
            var result = CreateInstance();
            ReadFields(reader, result, context);
            context.DeserializationStack.PopEntity();

            return result;
        }
    }
}