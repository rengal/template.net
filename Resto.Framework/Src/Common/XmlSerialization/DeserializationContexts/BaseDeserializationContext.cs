using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.DeserializationContexts
{
    public abstract class BaseDeserializationContext : IDeserializationContext
    {
        public XmlDeserializationStack DeserializationStack { get; } = new XmlDeserializationStack();

        private readonly IDictionary<Guid, Entity> childEntities = new Dictionary<Guid, Entity>(GuidComparer.Default);

        public abstract TRootEntityBaseType CreateRootEntity<TRootEntityBaseType>(Guid id, Type type) where TRootEntityBaseType : Entity, IRootEntity;

        public abstract TRootEntityBaseType GetRootEntity<TRootEntityBaseType>(Guid id, Type type) where TRootEntityBaseType : Entity, IRootEntity;

        public abstract void OnDeserialized([NotNull] IRootEntity entity);

        public bool ContainsChildEntity(Guid entityId) => childEntities.ContainsKey(entityId);

        public virtual Entity GetChildEntity(Guid entityId) => childEntities[entityId];

        public virtual void SetChildEntity(Guid entityId, Entity entity) => childEntities[entityId] = entity;

        public void ClearChildEntities() => childEntities.Clear();
        
        public abstract TEntity TryGetById<TRootEntityBaseType, TEntity>(Guid id) where TRootEntityBaseType : Entity, IRootEntity where TEntity : TRootEntityBaseType;
    }
}