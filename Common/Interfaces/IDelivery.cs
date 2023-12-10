using System;
using System.Collections.Generic;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий общие для front'а и back'а свойства доставки.
    /// </summary>
    public interface IDelivery
    {
        /// <summary>
        /// Количество персон в заказе.
        /// </summary>
        int GetPersonsCount();

        /// <summary>
        /// Список позиций заказа.
        /// </summary>
        IEnumerable<IDeliveryOrderItem> GetOrderItems(bool excludeNotDeleted = false);

        /// <summary>
        /// Разбивать ли заказ по персонам.
        /// </summary>
        bool GetSplitBetweenPersons();

        /// <summary>
        /// Имена гостей.
        /// </summary>
        IEnumerable<Guid> GetGuestsIds();
    }
}
