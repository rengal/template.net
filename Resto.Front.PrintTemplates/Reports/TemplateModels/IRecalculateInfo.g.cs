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
    /// Информация по контрольному пересчёту
    /// </summary>
    public interface IRecalculateInfo
    {
        /// <summary>
        /// Фактический остаток
        /// </summary>
        decimal RealCashRest { get; }

        /// <summary>
        /// Остаток при предыдущем пересчёте
        /// </summary>
        decimal PriorCashRest { get; }

        /// <summary>
        /// Конечный остаток
        /// </summary>
        decimal FinalCashRest { get; }

        /// <summary>
        /// Разница между конечным и фактическим остатком
        /// </summary>
        decimal Difference { get; }

        /// <summary>
        /// Дата/время пересчёта
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// Пользователь, производивший пересчёт
        /// </summary>
        [NotNull]
        IUser User { get; }

        /// <summary>
        /// Аутентификационные данные пользователей, производивших пересчёт
        /// </summary>
        [NotNull]
        IEnumerable<IAuthData> Auths { get; }

    }
}
