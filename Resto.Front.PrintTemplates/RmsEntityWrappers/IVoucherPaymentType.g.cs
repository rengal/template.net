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
    /// Тип оплаты "Ваучер"
    /// </summary>
    public interface IVoucherPaymentType : INonCashPaymentType
    {
        /// <summary>
        /// Указывается ли номинал ваучера в единицах товара (true) или в денежных единицах (false)
        /// </summary>
        bool IsAmountNominal { get; }

    }
}
