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
    /// Телефон
    /// </summary>
    public interface IPhone
    {
        /// <summary>
        /// Номер
        /// </summary>
        [NotNull]
        string Number { get; }

        /// <summary>
        /// Флаг основного номера
        /// </summary>
        bool IsMain { get; }

    }
}
