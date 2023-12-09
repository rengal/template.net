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
    /// Тип места приготовления
    /// </summary>
    public interface ICookingPlaceType
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Время приготовления в нормальном режиме работы
        /// </summary>
        TimeSpan CookingTimeNormal { get; }

        /// <summary>
        /// Время приготовления в пиковом режиме работы
        /// </summary>
        TimeSpan CookingTimePeak { get; }

    }
}
