using System;

using Resto.Framework.Attributes.JetBrains;

using Resto.Framework.Common;

namespace Resto.Framework.Data
{
    /// <summary>
    /// Провайдер для доступа к root entities, унаследованных от типа <typeparamref name="TRootEntityBaseType"/>.
    /// </summary>
    /// <typeparam name="TRootEntityBaseType">Базовый тип root entities (например, <see cref="PersistedEntity"/>).</typeparam>
    public interface IRootEntitiesProvider<TRootEntityBaseType> where TRootEntityBaseType : Entity, IRootEntity
    {
        /// <summary>
        /// Получить root entity по <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Entity с указанным id.</returns>
        /// <exception cref="RestoException">
        /// Entity с id <paramref name="id"/> нет.
        /// </exception>
        [NotNull]
        TRootEntityBaseType Get(Guid id);

        /// <summary>
        /// Получить root entity по <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Entity с указанным id или <c>null</c>.</returns>
        [CanBeNull]
        TRootEntityBaseType TryGet(Guid id);
    }
}