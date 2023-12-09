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
    /// Позиция заказа (модификатор)
    /// </summary>
    public interface IModifierEntry : IOrderEntry
    {
        /// <summary>
        /// Простой модификатор
        /// </summary>
        [CanBeNull]
        ISimpleModifier SimpleModifier { get; }

        /// <summary>
        /// Дочерний модификатор
        /// </summary>
        [CanBeNull]
        IChildModifier ChildModifier { get; }

        /// <summary>
        /// Тип места приготовления
        /// </summary>
        [NotNull]
        ICookingPlaceType CookingPlaceType { get; }

        /// <summary>
        /// Место приготовления
        /// </summary>
        [NotNull]
        IRestaurantSection CookingPlace { get; }

    }
}
