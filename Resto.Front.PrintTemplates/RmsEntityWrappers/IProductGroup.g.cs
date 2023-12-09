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
    /// Группа продуктов
    /// </summary>
    public interface IProductGroup
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Артикул
        /// </summary>
        [NotNull]
        string Article { get; }

    }
}
