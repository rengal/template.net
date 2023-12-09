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
    /// Простой модификатор
    /// </summary>
    public interface ISimpleModifier
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
        /// Продукт
        /// </summary>
        [NotNull]
        IProduct Product { get; }

        /// <summary>
        /// Место приготовления
        /// </summary>
        [NotNull]
        IRestaurantSection CookingPlace { get; }

        /// <summary>
        /// Количество по умолчанию
        /// </summary>
        int DefaultAmount { get; }

        /// <summary>
        /// Количество модификатора, добавленного в заказ, не зависит от количества блюда
        /// </summary>
        bool AmountIndependentOfParentAmount { get; }

    }
}
