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
    /// Стол
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Номер стола
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Название стола
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Отделение, в котором находится стол
        /// </summary>
        [NotNull]
        IRestaurantSection Section { get; }

    }
}
