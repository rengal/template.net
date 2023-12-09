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
    /// Тип скидки
    /// </summary>
    public interface IDiscountType
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Название для печати
        /// </summary>
        [NotNull]
        string PrintableName { get; }

        /// <summary>
        /// Печатать в пречеке позицию заказа, к которой была применена скидка (для флаеров)
        /// </summary>
        bool PrintProductItemInPrecheque { get; }

        /// <summary>
        /// Детализировать в пречеке (без учёта выборочно применённых скидок)
        /// </summary>
        bool PrintDetailedInPrecheque { get; }

        /// <summary>
        /// Скидка на сумму.
        /// </summary>
        bool DiscountBySum { get; }

        /// <summary>
        /// Признак того, что скидка iikoCard51.
        /// </summary>
        bool IsIikoCard51DiscountType { get; }

    }
}
