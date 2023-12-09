// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.RmsEntityWrappers
{
    /// <summary>
    /// Тип оплаты
    /// </summary>
    public interface IPaymentType
    {
        /// <summary>
        /// Флаг удаления
        /// </summary>
        bool Deleted { get; }

        /// <summary>
        /// Флаг доступности типа оплаты во фронте
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Группа типа оплаты
        /// </summary>
        PaymentGroup Group { get; }

        /// <summary>
        /// Проводить оплату как скидку
        /// </summary>
        bool ProccessAsDiscount { get; }

        /// <summary>
        /// Нужно ли печатать чек
        /// </summary>
        bool PrintCheque { get; }

        /// <summary>
        /// Можно ли тип оплаты использовать для оплаты заказов
        /// </summary>
        bool ValidForOrders { get; }

        /// <summary>
        /// Можно ли вводить сумму больше суммы к оплате. Будет показываться окно о сдаче.
        /// </summary>
        bool CanDisplayChange { get; }

    }
}
