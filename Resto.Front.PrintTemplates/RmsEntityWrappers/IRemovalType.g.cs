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
    /// Тип удаления позиции заказа
    /// </summary>
    public interface IRemovalType
    {
        /// <summary>
        /// Название типа удаления
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Счёт удаления
        /// </summary>
        [CanBeNull]
        IAccount Account { get; }

    }
}
