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
    /// Возврат товаров без оплаченного в данной смене заказа 
    /// </summary>
    public interface IReturnProductsChequePrint : IOrdersGroup
    {
        /// <summary>
        /// Комментарий
        /// </summary>
        string Comment { get; }

    }
}
