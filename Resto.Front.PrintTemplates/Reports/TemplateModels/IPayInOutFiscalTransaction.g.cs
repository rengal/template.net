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
    /// Транзакция фискального внесения/изъятия
    /// </summary>
    public interface IPayInOutFiscalTransaction
    {
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Флаг внесения/изъятия
        /// </summary>
        bool IsPayIn { get; }

        /// <summary>
        /// Тип транзакции внесения/изъятия
        /// </summary>
        PayInOutTransactionType TransactionType { get; }

    }
}
