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
    /// Вскрытые продукты ЕГАИС
    /// </summary>
    public interface IEgaisUnsealProduct
    {
        /// <summary>
        /// Наименование
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Объем
        /// </summary>
        decimal Capacity { get; }

        /// <summary>
        /// Алкокод
        /// </summary>
        string EgaisAlcCode { get; }

        /// <summary>
        /// Время вскрытия
        /// </summary>
        DateTime OpenTime { get; }

        /// <summary>
        /// Причина списания
        /// </summary>
        string WriteoffType { get; }

    }
}
