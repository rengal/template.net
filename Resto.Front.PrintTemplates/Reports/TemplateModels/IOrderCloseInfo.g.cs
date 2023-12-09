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
    /// Информация о закрытии заказа
    /// </summary>
    public interface IOrderCloseInfo
    {
        /// <summary>
        /// Кассир
        /// </summary>
        [NotNull]
        IUser Cashier { get; }

        /// <summary>
        /// Сдача
        /// </summary>
        decimal Change { get; }

        /// <summary>
        /// Дата/время закрытия
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// Тип списания
        /// </summary>
        [CanBeNull]
        IPaymentType WriteoffType { get; }

        /// <summary>
        /// Элемент списания
        /// </summary>
        [CanBeNull]
        IWriteoffPaymentItem WriteoffItem { get; }

    }
}
