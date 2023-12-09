using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.DeserializationContexts
{
    public sealed class DeserializationContext : BaseDeserializationContext
    {
        private readonly IEntitiesProvider entitiesProvider;
        /** Entities which were created during deserialization. */
        private readonly IDictionary<Guid, Entity> rootEntities = new Dictionary<Guid, Entity>(GuidComparer.Default);
        /** Entities which were created, but weren't initialized. */
        private readonly Dictionary<Guid, Entity> uninitializedEntities;
        private readonly bool supportDifferentEntitiesWithEmptyId;

        private bool allowEntitiesImplicitCreation = true;

        /// <summary>
        /// Simple mode, without <see cref="IEntitiesProvider"/>
        /// </summary>
        public DeserializationContext()
        {
        }

        public DeserializationContext([NotNull] IEntitiesProvider entitiesProvider, bool trackUninitializedEntities = false, bool supportDifferentEntitiesWithEmptyId = false)
        {
            this.entitiesProvider = entitiesProvider ?? throw new ArgumentNullException(nameof(entitiesProvider));
            if(trackUninitializedEntities)
                uninitializedEntities = new Dictionary<Guid, Entity>(GuidComparer.Default);
            this.supportDifferentEntitiesWithEmptyId = supportDifferentEntitiesWithEmptyId;
        }

        /// <exception cref="NotSupportedException">Exception to notify about "trackUninitializedEntities" constructor argument.</exception>
        [NotNull]
        public IReadOnlyCollection<Entity> UninitializedEntities
        {
            get
            {
                if (uninitializedEntities == null)
                    throw new NotSupportedException($"To use {nameof(UninitializedEntities)} you must set the \"trackUninitializedEntities\" in constructor to \"true\"");
                return uninitializedEntities.Values;
            }
        }

        public void DisableEntitiesImplicitCreation() => allowEntitiesImplicitCreation = false;

        public override TRootEntityBaseType CreateRootEntity<TRootEntityBaseType>(Guid id, [NotNull] Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (rootEntities.TryGetValue(id, out var existingEntity))
                return (TRootEntityBaseType)existingEntity;

            if(!allowEntitiesImplicitCreation)
                throw new SerializerException($"Can not find entity {type} with id {id}");

            var result = Serializer.CreateRootEntity<TRootEntityBaseType>(type);
            result.Id = id;
            rootEntities[id] = result;
            uninitializedEntities?.Add(id, result);
            return result;
        }

        public override TRootEntityBaseType GetRootEntity<TRootEntityBaseType>(Guid id, [NotNull] Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.IsInterface)
                throw new ArgumentException("Interfaces are not supported, use class type.");

            return entitiesProvider?.GetEntities<TRootEntityBaseType>().TryGet(id) ?? CreateRootEntity<TRootEntityBaseType>(id, type);
        }

        public override void SetChildEntity(Guid entityId, Entity entity)
        {
            if (supportDifferentEntitiesWithEmptyId && entityId == Guid.Empty)
                return;

            base.SetChildEntity(entityId, entity);
        }

        public override void OnDeserialized([NotNull] IRootEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            uninitializedEntities?.Remove(entity.Id);
        }

        [CanBeNull]
        public override TEntity TryGetById<TRootEntityBaseType, TEntity>(Guid id)
        {
            return rootEntities.TryGetValue(id, out var existingEntity)
                ? (TEntity)existingEntity
                : (TEntity)entitiesProvider?.GetEntities<TRootEntityBaseType>().TryGet(id);
        }
    }
}