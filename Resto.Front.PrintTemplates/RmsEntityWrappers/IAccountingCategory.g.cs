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
    /// Бухгалтерская категория
    /// </summary>
    public interface IAccountingCategory
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

    }
}
