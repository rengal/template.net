using System;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    public interface IEntitiesProvider
    {
        /// <exception cref="NotSupportedException">
        /// Свойство не поддерживается (фронт).
        /// </exception>
        [NotNull]
        IBaseEntityManager EntityManager { get; }

        [NotNull]
        IRootEntitiesProvider<TRootEntityBaseType> GetEntities<TRootEntityBaseType>() where TRootEntityBaseType : Entity, IRootEntity;
    }
}