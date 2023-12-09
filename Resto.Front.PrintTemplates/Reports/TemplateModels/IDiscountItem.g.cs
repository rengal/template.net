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
    /// Скидка, применённая к заказу
    /// </summary>
    public interface IDiscountItem
    {
        /// <summary>
        /// Тип скидки
        /// </summary>
        [NotNull]
        IDiscountType Type { get; }

        /// <summary>
        /// Признак категориальной скидки
        /// </summary>
        bool IsCategorized { get; }

        /// <summary>
        /// Способ добавления скидки
        /// </summary>
        DiscountSource Source { get; }

        /// <summary>
        /// Информация о скидочной карте
        /// </summary>
        [CanBeNull]
        IDiscountCardInfo CardInfo { get; }

        /// <summary>
        /// Число знаков номера скидочной карты, для печати на пречеке
        /// </summary>
        int? NumOfPrintedDigits { get; }

        /// <summary>
        /// Кем авторизовано добавление скидки
        /// </summary>
        [CanBeNull]
        IAuthData AuthData { get; }

        /// <summary>
        /// Суммы скидок по позициям заказа (ненулевые)
        /// </summary>
        [NotNull]
        IDictionary<IOrderEntry, decimal> DiscountSums { get; }

    }
}
