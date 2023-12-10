using System;
using System.Collections.Generic;
using Resto.Data;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий одну позицию настроек автодобавления блюд (общий для front'а и бэк'а)
    /// </summary>
    public interface IAutoAdditionSettingsItem
    {
        /// <summary>
        /// Добавляемое блюдо
        /// </summary>
        Guid ProductId { get; }

        /// <summary>
        /// Тип добавления (для блюд, для каждой персоны, на весь заказ)
        /// </summary>
        AutoAdditionType AutoAdditionType { get; }

        /// <summary>
        /// Количество
        /// </summary>
        decimal Quantity { get; }

        /// <summary>
        /// Блюда, для которых происходит автодобавление (не пусто только для типа добавления "для блюд")
        /// </summary>
        IList<Guid> InitiatorProductsId { get; }

        /// <summary>
        /// Признак удалённого элемента настроек автодобавления
        /// </summary>
        bool Deleted { get; }
    }
}