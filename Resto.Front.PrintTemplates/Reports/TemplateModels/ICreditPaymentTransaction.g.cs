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
    /// Транзакция оплаты заказа в кредит
    /// </summary>
    public interface ICreditPaymentTransaction : IOrderPaymentTransaction
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        IUser Counteragent { get; }

        /// <summary>
        /// Была ли прокатана карта контрагента
        /// </summary>
        bool CreditCounteragentCardSlided { get; }

    }
}
