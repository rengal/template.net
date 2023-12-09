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
    /// Система оплаты
    /// </summary>
    public interface IPaymentSystem
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Продукт пополнения
        /// </summary>
        [CanBeNull]
        IProduct ReplenishProduct { get; }

        /// <summary>
        /// Продукты активации
        /// </summary>
        [NotNull]
        IEnumerable<IProduct> ActivationProducts { get; }

    }
}
