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
    /// Элемент заказа (повременная услуга)
    /// </summary>
    public interface ITimePayServiceItem : IOrderItem
    {
        /// <summary>
        /// Ограничение по времени
        /// </summary>
        TimeSpan? TimeLimit { get; }

        /// <summary>
        /// Стоимость услуги
        /// </summary>
        decimal TimePayServiceCost { get; }

        /// <summary>
        /// Позиции по тарифам повременной услуги
        /// </summary>
        [NotNull]
        IEnumerable<IRateScheduleEntry> RateScheduleEntries { get; }

    }
}
