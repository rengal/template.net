// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.RmsEntityWrappers
{
    /// <summary>
    /// Дополнительная валюта
    /// </summary>
    public interface IAdditionalCurrency
    {
        /// <summary>
        /// Стандартное название валюты (ISO)
        /// </summary>
        [NotNull]
        string IsoName { get; }

        /// <summary>
        /// Сокращённое название валюты
        /// </summary>
        [NotNull]
        string ShortName { get; }

        /// <summary>
        /// Сокращённое название валюты в графическом интерфейсе
        /// </summary>
        [NotNull]
        string ShortNameForGui { get; }

    }
}
