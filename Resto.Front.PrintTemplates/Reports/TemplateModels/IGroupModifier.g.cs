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
    /// Групповой модификатор
    /// </summary>
    public interface IGroupModifier
    {
        /// <summary>
        /// Минимальное количество
        /// </summary>
        int MinimumAmount { get; }

        /// <summary>
        /// Максимальное количество
        /// </summary>
        int MaximumAmount { get; }

        /// <summary>
        /// Группа продуктов
        /// </summary>
        [NotNull]
        IProductGroup ProductGroup { get; }

        /// <summary>
        /// Дочерние модификаторы
        /// </summary>
        [NotNull]
        IEnumerable<IChildModifier> ChildModifiers { get; }

    }
}
