using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.SerializationContexts
{
    public sealed class DefaultSerializationContext : ISerializationContext
    {
        private readonly bool supportDifferentEntitiesWithEmptyId;
        private readonly HashSet<Guid> childEntities = new HashSet<Guid>(GuidComparer.Default);

        public DefaultSerializationContext()
            : this(false)
        { }

        public DefaultSerializationContext(bool supportDifferentEntitiesWithEmptyId)
        {
            this.supportDifferentEntitiesWithEmptyId = supportDifferentEntitiesWithEmptyId;
        }

        public bool IsChildEntityRegistered(Guid entityId)
        {
            if (supportDifferentEntitiesWithEmptyId && entityId == Guid.Empty)
                return false;

            return childEntities.Contains(entityId);
        }

        public void RegisterChildEntity(Guid entityId)
        {
            if (supportDifferentEntitiesWithEmptyId && entityId == Guid.Empty)
                return;

            childEntities.Add(entityId);
        }

        [NotNull]
        public ISerializationContext CreateNew()
        {
            return new DefaultSerializationContext(supportDifferentEntitiesWithEmptyId);
        }
    }
}