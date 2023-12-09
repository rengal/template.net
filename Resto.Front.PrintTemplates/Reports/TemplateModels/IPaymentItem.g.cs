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
    /// Элемент оплаты заказа
    /// </summary>
    public interface IPaymentItem
    {
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        [NotNull]
        IPaymentType Type { get; }

        /// <summary>
        /// Тип чаевых
        /// </summary>
        [CanBeNull]
        IDonationType DonationType { get; }

        /// <summary>
        /// Является ли внешняя позиция оплаты заранее проведенной вовне
        /// </summary>
        bool IsProcessedExternally { get; }

        /// <summary>
        /// Информация об оплате в дополнительной валюте. null, если оплата в основной валюте
        /// </summary>
        [CanBeNull]
        ICurrencyInfo CurrencyInfo { get; }

    }
}
