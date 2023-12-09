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
    /// Тип оплаты "Настраиваемый"
    /// </summary>
    public interface IConfigurablePaymentType : IPaymentType
    {
        /// <summary>
        /// Базовый тип оплаты
        /// </summary>
        IPaymentType BasePaymentType { get; }

    }
}
