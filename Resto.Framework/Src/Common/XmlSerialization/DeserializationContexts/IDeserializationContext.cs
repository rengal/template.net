using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Framework.Common.XmlSerialization.DeserializationContexts
{
    public interface IDeserializationContext
    {
        /// <summary>
        /// Путь к текущему десериализуемому элементу от корня (может содержать имена типов, имея полей и т.п.).
        /// Используется в отладочных целях, в случае ошибки при десериализации можно будет понять путь к проблемному значению.
        /// </summary>
        [NotNull]
        XmlDeserializationStack DeserializationStack { get; }

        [NotNull]
        TRootEntityBaseType CreateRootEntity<TRootEntityBaseType>(Guid id, [NotNull] Type type) where TRootEntityBaseType : Entity, IRootEntity;

        [NotNull]
        TRootEntityBaseType GetRootEntity<TRootEntityBaseType>(Guid id, [NotNull] Type type) where TRootEntityBaseType : Entity, IRootEntity;

        void OnDeserialized([NotNull] IRootEntity entity);

        bool ContainsChildEntity(Guid entityId);

        Entity GetChildEntity(Guid entityId);

        void SetChildEntity(Guid entityId, Entity entity);

        void ClearChildEntities();

        TEntity TryGetById<TRootEntityBaseType, TEntity>(Guid id) where TRootEntityBaseType : Entity, IRootEntity where TEntity : TRootEntityBaseType;
    }
}