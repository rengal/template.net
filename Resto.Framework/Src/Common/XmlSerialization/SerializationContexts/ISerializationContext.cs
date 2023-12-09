using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.SerializationContexts
{
    public interface ISerializationContext
    {
        bool IsChildEntityRegistered(Guid entityId);
        void RegisterChildEntity(Guid entityId);
        [NotNull]
        ISerializationContext CreateNew();
    }
}