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
    /// Транзакция оплаты заказа ваучерами
    /// </summary>
    public interface IVoucherPaymentTransaction : IOrderPaymentTransaction
    {
        /// <summary>
        /// Номинал
        /// </summary>
        decimal Nominal { get; }

        /// <summary>
        /// Количество использованных ваучеров
        /// </summary>
        int VouchersNum { get; }

    }
}
