using System;
using System.Collections.Generic;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий настройки автодобавления (общий для Front и BackOffice).
    /// </summary>
    public interface IAutoAdditionSettings
    {
        /// <summary>
        /// Коллекция настроек автодобавления.
        /// </summary>
        IEnumerable<IAutoAdditionSettingsItem> GetItems();

        /// <summary>
        /// Идентификаторы блюд, участвующих в автодобавлении.
        /// </summary>
        IEnumerable<Guid> AutoAdditionProductIds { get; }
    }
}
