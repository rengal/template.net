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
    /// Кассовая смена
    /// </summary>
    public interface ICafeSession
    {
        /// <summary>
        /// Номер смены
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Дата/время открытия смены
        /// </summary>
        DateTime OpenTime { get; }

        /// <summary>
        /// Сумма в кассе на начало смены
        /// </summary>
        decimal StartCash { get; }

        /// <summary>
        /// Расчётная сумма наличных в кассе (по версии iiko)
        /// </summary>
        decimal CalculatedBookCash { get; }

        /// <summary>
        /// Закрытые заказы
        /// </summary>
        [NotNull]
        IEnumerable<IOrder> ClosedOrders { get; }

        /// <summary>
        /// Сторнированные заказы
        /// </summary>
        [NotNull]
        IEnumerable<IOrder> StornedOrders { get; }

        /// <summary>
        /// Контрольные пересчёты
        /// </summary>
        [NotNull]
        IEnumerable<IRecalculateInfo> Recalculations { get; }

    }
}
