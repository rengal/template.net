using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class RootEntitySerializer<T, TRootEntityBaseType> : BaseCompoundSerializer<T>, IRootEntityFactory<T>
        where T : TRootEntityBaseType
        where TRootEntityBaseType : Entity, IRootEntity
    {
        [CanBeNull]
        public override T DeepClone([CanBeNull] T value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (value == null)
                return null;

            if (!byValue)
                return (T)context.GetRootEntity<TRootEntityBaseType>(value.Id, typeof(T));


            var result = (T)context.CreateRootEntity<TRootEntityBaseType>(value.Id, typeof(T));
            try
            {
                DeepCopyFields(value, result, context);
            }
            finally
            {
                context.ClearChildEntities();
            }

            context.OnDeserialized(result);
            result.AfterDeserialization();
            return result;
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


            if (!byValue)
            {
                // проверяется, содержится ли объект в кэше
                var persistedEntity = value as PersistedEntity;
                if (persistedEntity != null)
                    PersistedEntityCheck(persistedEntity);

                writer.WriteString(GuidGenerator.FormatGuid(value.Id));
                return;
            }

            var id = value.Id;
            writer.WriteAttributeString("eid", GuidGenerator.FormatGuid(id));

            WriteFields(writer, value, context.CreateNew());
        }

        [NotNull]
        public override T ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            T result;
            if (!byValue)
            {
                var entityIdString = StringSerializer.Value.ReadElementContent(reader, false, context, SerializationMetadata.Empty);
                context.DeserializationStack.PushEntity<T>(entityIdString);
                var entityId = GuidGenerator.ParseGuid(entityIdString);

                result = (T)context.GetRootEntity<TRootEntityBaseType>(entityId, typeof(T));
                context.DeserializationStack.PopEntity();
                return result;
            }

            var idString = reader.GetAttribute("eid");
            if (idString == null)
            {
                throw new SerializerException("Attribute eid not specified for entity. Type: " + typeof(T));
            }
            context.DeserializationStack.PushEntity<T>(idString);
            var id = GuidGenerator.ParseGuid(idString);
            result = (T)context.CreateRootEntity<TRootEntityBaseType>(id, typeof(T));

            try
            {
                ReadFields(reader, result, context);
            }
            finally
            {
                context.ClearChildEntities();
            }

            context.OnDeserialized(result);
            result.AfterDeserialization();
            context.DeserializationStack.PopEntity();

            return result;

        }

        [NotNull]
        public T Create()
        {
            return CreateInstance();
        }
    }
}