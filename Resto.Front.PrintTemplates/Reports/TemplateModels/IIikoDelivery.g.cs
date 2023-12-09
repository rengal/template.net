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
    public interface IIikoDelivery : ICommonGroup
    {
        /// <summary>
        /// Номер доставки
        /// </summary>
        string DeliveryNumber { get; }

        /// <summary>
        /// Оператор
        /// </summary>
        string DeliveryOperator { get; }

        /// <summary>
        /// Клиент
        /// </summary>
        string DeliveryCustomer { get; }

        /// <summary>
        /// Сумма доставки
        /// </summary>
        decimal? DeliverySum { get; }

    }
}
