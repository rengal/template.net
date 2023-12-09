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
    /// События работы с позициями заказа
    /// </summary>
    public interface IDishesGroup : ICommonGroup
    {
        /// <summary>
        /// Номер заказа
        /// </summary>
        int? OrderNum { get; }

        /// <summary>
        /// Сумма
        /// </summary>
        decimal? Sum { get; }

        /// <summary>
        /// Скидка/надбавка %
        /// </summary>
        int? Percent { get; }

        /// <summary>
        /// Официант
        /// </summary>
        [CanBeNull]
        IUser Waiter { get; }

        /// <summary>
        /// Список позиций заказа
        /// </summary>
        string Dishes { get; }

        /// <summary>
        /// Причина
        /// </summary>
        string Reason { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        string Comment { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [CanBeNull]
        IOrder Order { get; }

        /// <summary>
        /// Кол-во строк
        /// </summary>
        int? RowCount { get; }

    }
}
