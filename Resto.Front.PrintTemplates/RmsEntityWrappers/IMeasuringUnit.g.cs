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
    /// Единица измерения
    /// </summary>
    public interface IMeasuringUnit
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Полное название
        /// </summary>
        [NotNull]
        string FullName { get; }

        /// <summary>
        /// Вид единицы измерения
        /// </summary>
        MeasuringUnitKind Kind { get; }

    }
}
