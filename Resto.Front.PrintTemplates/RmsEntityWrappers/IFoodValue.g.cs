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
    /// Пищевая ценность
    /// </summary>
    public interface IFoodValue
    {
        /// <summary>
        /// Жиры
        /// </summary>
        decimal Fat { get; }

        /// <summary>
        /// Белки
        /// </summary>
        decimal Protein { get; }

        /// <summary>
        /// Углеводы
        /// </summary>
        decimal Carbohydrate { get; }

        /// <summary>
        /// Калорийность
        /// </summary>
        decimal Caloricity { get; }

    }
}
