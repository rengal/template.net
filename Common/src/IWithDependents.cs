using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    /// <summary>
    /// Интерфейс сущности, которая зависят от других сущностей.
    /// Используется для определения необходимости обновления сущности при обновлении сущностей,
    /// от которых она зависит.
    /// </summary>
    public interface IWithDependents
    {
        /// <summary>
        /// Зависимые объекты в виде словаря (id -> ревизия).
        /// </summary>
        [NotNull]
        IDictionary<Guid, int> GetDependents();
    }
}
