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
    /// События работы с заказами
    /// </summary>
    public interface IOrdersGroup : ICommonGroup
    {
        /// <summary>
        /// Номер заказа
        /// </summary>
        int? OrderNum { get; }

        /// <summary>
        /// Сумма
        /// </summary>
        decimal? Sum { get; }

        /// <summary>
        /// Скидка/надбавка %
        /// </summary>
        int? Percent { get; }

        /// <summary>
        /// Официант
        /// </summary>
        [CanBeNull]
        IUser Waiter { get; }

        /// <summary>
        /// Сумма со скидкой
        /// </summary>
        decimal? OrderSumAfterDiscount { get; }

        /// <summary>
        /// Доп. авторизация
        /// </summary>
        IUser Auth { get; }

        /// <summary>
        /// К-во гостей
        /// </summary>
        int? NumGuests { get; }

    }
}
