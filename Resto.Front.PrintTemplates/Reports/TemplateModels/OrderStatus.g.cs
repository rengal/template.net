// This file was generated with T4.
// Do not edit it manually.



namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    /// <summary>
    /// Статус, в котором находится заказ
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Новый заказ
        /// </summary>
        New,

        /// <summary>
        /// Заказ в процессе оплаты
        /// </summary>
        Bill,

        /// <summary>
        /// Заказ в процессе закрытия
        /// </summary>
        Closing,

        /// <summary>
        /// Закрытый заказ
        /// </summary>
        Closed,

        /// <summary>
        /// Заказ в процессе сторнирования
        /// </summary>
        Storning,

        /// <summary>
        /// Удаленный заказ
        /// </summary>
        Deleted,

    }
}
