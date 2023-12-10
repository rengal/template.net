using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;
using Resto.Framework.Data;

namespace Resto.Common.Extensions
{
    /// <summary>
    /// Extension-методы, используемые для работы с объектами корпорации
    /// в настройках ТП и в настройках корпорации.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Возвращает тип объекта корпорации для данного <paramref name="entity"/>
        /// </summary>
        /// <remarks>
        /// <paramref name="entity"/> должен являться объектом корпорации, т.е. должен
        /// реализовывать интерфейс <see cref="ICorporatedEntityProps"/>
        /// </remarks>
        [NotNull]
        public static CorporatedEntityType CorporatedType([NotNull] this PersistedEntity entity)
        {
            return ((ICorporatedEntityProps)entity).CEType;
        }

        /// <summary>
        /// Для данного <paramref name="entity"/> проходит вверх по
        /// корпоративной иерархии и находит первого подходящего родителя,
        /// который удовлетворяет условию <paramref name="parentPredicate"/>.
        /// </summary>
        /// <returns>
        /// Возвращает найденного родителя или <c>null</c>, если не
        /// найден родитель, удовлетовряющий условию.
        /// </returns>
        /// <remarks>
        /// <paramref name="entity"/> должен являться объектом корпорации, т.е. должен
        /// реализовывать интерфейс <see cref="ICorporatedEntityProps"/>
        /// </remarks>
        [CanBeNull]
        public static PersistedEntity CorporatedParent([NotNull] this PersistedEntity entity,
            [NotNull] Func<PersistedEntity, bool> parentPredicate)
        {
            while (true)
            {
                if (entity == null)
                {
                    return null;
                }

                if (parentPredicate(entity))
                {
                    return entity;
                }

                entity = ((ICorporatedEntityProps)entity).CEParent;
            }
        }
    }
}