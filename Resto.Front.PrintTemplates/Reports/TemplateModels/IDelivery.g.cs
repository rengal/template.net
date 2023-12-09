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
    /// Доставка
    /// </summary>
    public interface IDelivery
    {
        /// <summary>
        /// Статус
        /// </summary>
        DeliveryStatus Status { get; }

        /// <summary>
        /// Номер
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        string Comment { get; }

        /// <summary>
        /// Курьер
        /// </summary>
        [CanBeNull]
        IUser Courier { get; }

        /// <summary>
        /// Время, к которому надо доставить
        /// </summary>
        DateTime DeliverTime { get; }

        /// <summary>
        /// Фактическое время доставки
        /// </summary>
        DateTime? ActualTime { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [NotNull]
        IOrder Order { get; }

        /// <summary>
        /// Причина отмены доставки
        /// </summary>
        [CanBeNull]
        string CancelCause { get; }

    }
}
