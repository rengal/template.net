using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Свойства, общие для объектов корпорации
    /// </summary>
    public interface ICorporatedEntityProps
    {
        /// <summary>
        /// Описание
        /// </summary>
        string CEDescription { get; set; }

        /// <summary>
        /// Родитель
        /// </summary>
        PersistedEntity CEParent { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        [NotNull]
        CorporatedEntityType CEType { get; }
    }
}