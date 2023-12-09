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
    /// Пользовательское свойство продукта
    /// </summary>
    public interface IProductTag
    {
        /// <summary>
        /// Значение свойства
        /// </summary>
        [NotNull]
        string Value { get; }

        /// <summary>
        /// Группа, которому принадлежит свойство
        /// </summary>
        [NotNull]
        IProductTagGroup Group { get; }

    }
}
