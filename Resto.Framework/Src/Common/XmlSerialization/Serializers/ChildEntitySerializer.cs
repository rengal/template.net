using System;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.DeserializationContexts;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class ChildEntitySerializer<T> : BaseCompoundSerializer<T> where T : Entity
    {
        public ChildEntitySerializer()
        {
            if (typeof(PersistedEntity).IsAssignableFrom(typeof(T)))
                throw new SerializerException(string.Format("Type {0} cannot derive from PersistedEntity", typeof(T)));
        }

        [CanBeNull]
        public override T DeepClone([CanBeNull] T value, bool byValue, [NotNull] IDeserializationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (value == null)
                return null;

            if (context.ContainsChildEntity(value.Id))
                return (T)context.GetChildEntity(value.Id);

            var clone = CreateInstance();
            clone.Id = value.Id;
            context.SetChildEntity(value.Id, clone);
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


            var id = value.Id;
            writer.WriteAttributeString("eid", GuidGenerator.FormatGuid(id));

            if (context.IsChildEntityRegistered(id))
            {
                //Do not save child entity content twice
                return;
            }
            context.RegisterChildEntity(id);

            WriteFields(writer, value, context);
        }

        [NotNull]
        public override T ReadElementContent([NotNull] XmlReader reader, bool byValue, [NotNull] IDeserializationContext context, SerializationMetadata metadata)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            var idStr = reader.GetAttribute("eid");
            if (idStr == null)
                throw new SerializerException("Attribute eid not specified for entity. Type: " + typeof(T));
            context.DeserializationStack.PushEntity<T>(idStr);
            var id = GuidGenerator.ParseGuid(idStr);

            T result;
            if (context.ContainsChildEntity(id))
            {
                //Child entity was already parsed
                //Skipping xml content to next close tag
                if (!reader.IsEmptyElement)
                {
                    while (reader.Read())
                    {
                        var nodeType = reader.NodeType;
                        if (nodeType == XmlNodeType.EndElement)
                            break;

                        switch (nodeType)
                        {
                            case XmlNodeType.Text:
                            case XmlNodeType.CDATA:
                                throw new SerializerException("Unexpected content in reference field: " +
                                                              reader.Name + " id: " + id);
                            case XmlNodeType.Element:
                                throw new SerializerException("Unexpected element in reference field: " +
                                                              reader.Name + " id: " + id);
                        }
                    }
                }
                result = (T)context.GetChildEntity(id);
                context.DeserializationStack.PopEntity();
                return result;
            }

            //Creating child entity and saving it in context
            result = CreateInstance();
            result.Id = id;
            context.SetChildEntity(id, result);

            ReadFields(reader, result, context);
            context.DeserializationStack.PopEntity();
            return result;
        }
    }
}