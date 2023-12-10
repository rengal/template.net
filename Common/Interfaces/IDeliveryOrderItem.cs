using System;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий общие для front'а и back'а свойства позиции доставки
    /// </summary>
    public interface IDeliveryOrderItem
    {
        /// <summary>
        /// Id блюда
        /// </summary>
        Guid GetProductId();
        /// <summary>
        /// Количество
        /// </summary>
        decimal GetAmount();
        /// <summary>
        /// Id гостя
        /// </summary>
        Guid GetGuestId();
        /// <summary>
        /// Отпечатано ли блюдо
        /// </summary>
        bool IsItemPrinted();
    }
}
