// This file was generated with T4.
// Do not edit it manually.



namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    /// <summary>
    /// Статус, в котором находится доставка
    /// </summary>
    public enum DeliveryStatus
    {
        /// <summary>
        /// Не подтверждена
        /// </summary>
        Unconfirmed,

        /// <summary>
        /// Новая доставка
        /// </summary>
        New,

        /// <summary>
        /// Ожидает отправки
        /// </summary>
        Waiting,

        /// <summary>
        /// В пути
        /// </summary>
        OnWay,

        /// <summary>
        /// Доставлена
        /// </summary>
        Delivered,

        /// <summary>
        /// Закрыта
        /// </summary>
        Closed,

        /// <summary>
        /// Отменена
        /// </summary>
        Cancelled,

    }
}
