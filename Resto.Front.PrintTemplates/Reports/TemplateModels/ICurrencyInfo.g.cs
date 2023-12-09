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
    /// Информация об оплате в дополнительной валюте
    /// </summary>
    public interface ICurrencyInfo
    {
        /// <summary>
        /// Дополнительная валюта
        /// </summary>
        IAdditionalCurrency Currency { get; }

        /// <summary>
        /// Курс валюты
        /// </summary>
        decimal Rate { get; }

        /// <summary>
        /// Сумма в валюте
        /// </summary>
        decimal Sum { get; }

    }
}
