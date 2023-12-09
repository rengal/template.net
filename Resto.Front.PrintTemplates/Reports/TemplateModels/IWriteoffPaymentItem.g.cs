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
    /// Элемент оплаты заказа - списание
    /// </summary>
    public interface IWriteoffPaymentItem : IPaymentItem
    {
        /// <summary>
        /// Комментарий
        /// </summary>
        string Reason { get; }

    }
}
