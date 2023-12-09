// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    /// <summary>
    /// Транзакция оплаты заказа
    /// </summary>
    public interface IOrderPaymentTransaction
    {
        /// <summary>
        /// ID заказа, которому принадлежит оплата
        /// </summary>
        Guid OrderId { get; }

        /// <summary>
        /// Заказ, которому принадлежит оплата
        /// </summary>
        [CanBeNull]
        IOrder Order { get; }

        /// <summary>
        /// Кассир
        /// </summary>
        [NotNull]
        IUser Cashier { get; }

        /// <summary>
        /// Дата/время транзакции
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// Сумма транзакции
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Фискальная ли транзакция
        /// </summary>
        bool IsFiscal { get; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        [NotNull]
        IPaymentType PaymentType { get; }

        /// <summary>
        /// Тип чаевых
        /// </summary>
        [CanBeNull]
        IDonationType DonationType { get; }

        /// <summary>
        /// Тип транзакции
        /// </summary>
        OrderPaymentTransactionType TransactionType { get; }

        /// <summary>
        /// Номер чека
        /// </summary>
        int ChequeNumber { get; }

        /// <summary>
        /// Покупка через фронт
        /// </summary>
        bool IsPurchase { get; }

        /// <summary>
        /// Кем авторизована транзакция оплаты
        /// </summary>
        [CanBeNull]
        IAuthData AuthData { get; }

        /// <summary>
        /// Признак того, что транзакция оплаты заказа была произведена за счет официанта
        /// </summary>
        bool IsWaiterDebt { get; }

        /// <summary>
        /// Флаг доставочного заказа
        /// </summary>
        bool IsDeliveryOrder { get; }

        /// <summary>
        /// Информация об оплате в дополнительной валюте. null, если оплата в основной валюте
        /// </summary>
        [CanBeNull]
        ICurrencyInfo CurrencyInfo { get; }

    }
}
