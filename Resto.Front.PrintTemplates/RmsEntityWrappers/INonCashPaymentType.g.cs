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
    /// Тип оплаты "Безналичный"
    /// </summary>
    public interface INonCashPaymentType : IPaymentType
    {
        /// <summary>
        /// Скидка, которой заменяется оплата
        /// </summary>
        [CanBeNull]
        IDiscountType ReplaceDiscount { get; }

    }
}
