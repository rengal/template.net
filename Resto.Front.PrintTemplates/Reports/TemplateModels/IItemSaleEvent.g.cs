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
    /// Событие продажи позиции заказа
    /// </summary>
    public interface IItemSaleEvent
    {
        /// <summary>
        /// Заказ, которому принадлежит позиция
        /// </summary>
        [CanBeNull]
        IOrder Order { get; }

        /// <summary>
        /// Ставка НДС
        /// </summary>
        decimal Vat { get; }

        /// <summary>
        /// Сумма НДС
        /// </summary>
        decimal VatSum { get; }

        /// <summary>
        /// Причина удаления блюда
        /// </summary>
        [CanBeNull]
        IRemovalType RemovalType { get; }

        /// <summary>
        /// Признак того, что блюдо было удалено из заказа со списанием: null — блюдо не было удалено, true — блюдо было удалено со списанием, false — блюдо было удалено без списания.
        /// </summary>
        bool? DeletedWithWriteoff { get; }

        /// <summary>
        /// Блюдо позиции заказа
        /// </summary>
        [NotNull]
        IProduct Dish { get; }

        /// <summary>
        /// Количество блюда
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Сумма по позиции заказа
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Сумма по позиции заказа с учетом скидки и наценки
        /// </summary>
        decimal SumAfterDiscount { get; }

        /// <summary>
        /// Официант заказа
        /// </summary>
        [CanBeNull]
        IUser Waiter { get; }

        /// <summary>
        /// Тип места приготовления блюда
        /// </summary>
        [CanBeNull]
        ICookingPlaceType CookingPlaceType { get; }

        /// <summary>
        /// ID заказа, которому принадлежит позиция
        /// </summary>
        Guid OrderId { get; }

    }
}
