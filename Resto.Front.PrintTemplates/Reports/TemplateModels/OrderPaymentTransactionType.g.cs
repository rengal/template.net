// This file was generated with T4.
// Do not edit it manually.



namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    /// <summary>
    /// Тип транзакции оплаты заказа
    /// </summary>
    public enum OrderPaymentTransactionType
    {
        /// <summary>
        /// Продажа за наличные
        /// </summary>
        Cash,

        /// <summary>
        /// Выручка по картам
        /// </summary>
        Card,

        /// <summary>
        /// Выручка в кредит
        /// </summary>
        Credit,

        /// <summary>
        /// Оплата заказа за счет заведения
        /// </summary>
        OnTheHouse,

        /// <summary>
        /// Продажа с предоплатой
        /// </summary>
        PrepayClosed,

        /// <summary>
        /// Внесение предоплаты
        /// </summary>
        Prepay,

        /// <summary>
        /// Возврат предоплаты
        /// </summary>
        PrepayReturn,

        /// <summary>
        /// Возврат продажи с предоплатой
        /// </summary>
        PrepayClosedReturn,

        /// <summary>
        /// Возврат денег покупателю
        /// </summary>
        RevenueReturn,

        /// <summary>
        /// Чаевые
        /// </summary>
        Donations,

    }
}
