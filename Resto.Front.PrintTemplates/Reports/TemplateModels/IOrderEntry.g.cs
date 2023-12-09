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
    /// Позиция заказа
    /// </summary>
    public interface IOrderEntry
    {
        /// <summary>
        /// Блюдо/продукт/услуга
        /// </summary>
        [NotNull]
        IProduct Product { get; }

        /// <summary>
        /// Категория блюда, зафиксированная на момент добавления позиции в заказ
        /// </summary>
        [CanBeNull]
        IProductCategory ProductCategory { get; }

        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Цена за единицу
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Стоимость
        /// </summary>
        decimal Cost { get; }

        /// <summary>
        /// НДС включен в стоимость
        /// </summary>
        bool VatIncludedInPrice { get; }

        /// <summary>
        /// Ставка НДС
        /// </summary>
        decimal Vat { get; }

        /// <summary>
        /// Сумма НДС, не включенная в стоимость позиции заказа
        /// </summary>
        decimal ExcludedVat { get; }

        /// <summary>
        /// Сумма НДС, включенная в стоимость позиции заказа
        /// </summary>
        decimal IncludedVat { get; }

        /// <summary>
        /// Информация об удалении позиции заказа. null, если позиция не удалена.
        /// </summary>
        [CanBeNull]
        IOrderEntryDeletionInfo DeletionInfo { get; }

        /// <summary>
        /// Информация о том, является ли позиция заказа флаером. null, если позиция не флаер
        /// </summary>
        [CanBeNull]
        IOrderEntryDeletionInfo FlyerDeletionInfo { get; }

    }
}
