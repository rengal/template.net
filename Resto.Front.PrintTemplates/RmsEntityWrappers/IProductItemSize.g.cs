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
    /// Размер элемента заказа (блюда)
    /// </summary>
    public interface IProductItemSize
    {
        /// <summary>
        /// Наименование размера
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Краткое наименование размера, адаптированное для отображения на кухне
        /// </summary>
        [NotNull]
        string KitchenName { get; }

    }
}
