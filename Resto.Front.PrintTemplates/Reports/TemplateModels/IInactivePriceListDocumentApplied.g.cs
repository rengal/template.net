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
    /// Применение прейскуранта вне расписания
    /// </summary>
    public interface IInactivePriceListDocumentApplied : IDishesGroup
    {
        /// <summary>
        /// Наименование прейскуранта
        /// </summary>
        string InactivePriceListDocumentName { get; }

        /// <summary>
        /// Цена (старая)
        /// </summary>
        decimal? PriceBefore { get; }

    }
}
