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
    /// Заказ
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// Номер
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Номер чека
        /// </summary>
        [Obsolete("В заказе хранятся номера чеков, которые были напечатаны при закрытии заказа. Для обратной совместимости, здесь возвращается номер последнего чека. Вместо этого свойства необходимо использовать FiscalChequeNumbers.")]
        int? FiscalChequeNumber { get; }

        /// <summary>
        /// Дата/время открытия
        /// </summary>
        DateTime OpenTime { get; }

        /// <summary>
        /// Дата/время пречека
        /// </summary>
        DateTime? PrechequeTime { get; }

        /// <summary>
        /// Тип заказа
        /// </summary>
        [CanBeNull]
        IOrderType Type { get; }

        /// <summary>
        /// Стол
        /// </summary>
        [NotNull]
        ITable Table { get; }

        /// <summary>
        /// Официант
        /// </summary>
        [CanBeNull]
        IUser Waiter { get; }

        /// <summary>
        /// Номинальное количество гостей в заказе
        /// </summary>
        int InitialGuestsCount { get; }

        /// <summary>
        /// Информация о закрытии заказа
        /// </summary>
        [CanBeNull]
        IOrderCloseInfo CloseInfo { get; }

        /// <summary>
        /// Статус заказа
        /// </summary>
        OrderStatus Status { get; }

        /// <summary>
        /// Был ли заказ сторнирован
        /// </summary>
        bool IsStorned { get; }

        /// <summary>
        /// Количество удаленных неотпечатанных блюд
        /// </summary>
        decimal DeletedNewItemsAmount { get; }

        /// <summary>
        /// Сумма удаленных неотпечатанных блюд
        /// </summary>
        decimal DeletedNewItemsSum { get; }

        /// <summary>
        /// Номера чеков
        /// </summary>
        [NotNull]
        IEnumerable<int> FiscalChequeNumbers { get; }

        /// <summary>
        /// Гости
        /// </summary>
        [NotNull]
        IEnumerable<IGuest> Guests { get; }

        /// <summary>
        /// Скидки, применённые к заказу
        /// </summary>
        [NotNull]
        IEnumerable<IDiscountItem> DiscountItems { get; }

        /// <summary>
        /// Оплаты
        /// </summary>
        [NotNull]
        IEnumerable<IPaymentItem> PaymentItems { get; }

        /// <summary>
        /// Предварительные оплаты
        /// </summary>
        [NotNull]
        IEnumerable<IPaymentItem> PrePaymentItems { get; }

        /// <summary>
        /// Чаевые
        /// </summary>
        [NotNull]
        IEnumerable<IPaymentItem> Donations { get; }

        /// <summary>
        /// Общедоступные внешние данные, хранимые API-плагинами в заказе.
        /// </summary>
        [NotNull]
        IDictionary<string, string> ExternalData { get; }

    }
}
