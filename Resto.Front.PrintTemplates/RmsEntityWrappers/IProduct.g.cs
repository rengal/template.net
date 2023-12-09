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
    /// Продукт
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Полное название
        /// </summary>
        [CanBeNull]
        string FullName { get; }

        /// <summary>
        /// Полное название на иностранном языке
        /// </summary>
        [CanBeNull]
        string FullNameForeignLanguage { get; }

        /// <summary>
        /// Название для кухни
        /// </summary>
        [CanBeNull]
        string KitchenName { get; }

        /// <summary>
        /// Тип продукта
        /// </summary>
        ProductType Type { get; }

        /// <summary>
        /// Категория продукта
        /// </summary>
        [CanBeNull]
        IProductCategory Category { get; }

        /// <summary>
        /// Бухгалтерская категория
        /// </summary>
        [CanBeNull]
        IAccountingCategory AccountingCategory { get; }

        /// <summary>
        /// Пищевая ценность
        /// </summary>
        [CanBeNull]
        IFoodValue FoodValue { get; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        [NotNull]
        IMeasuringUnit MeasuringUnit { get; }

        /// <summary>
        /// Суммарный фактический выход на 1 норму закладки (вес 1 единицы измерения), кг
        /// </summary>
        [CanBeNull]
        decimal? UnitWeight { get; }

        /// <summary>
        /// Описание
        /// </summary>
        [CanBeNull]
        string Description { get; }

        /// <summary>
        /// Описание на иностранном языке
        /// </summary>
        [CanBeNull]
        string DescriptionForeignLanguage { get; }

        /// <summary>
        /// Срок годности
        /// </summary>
        TimeSpan ExpirationPeriod { get; }

        /// <summary>
        /// Цена
        /// </summary>
        decimal SalePrice { get; }

        /// <summary>
        /// НДС
        /// </summary>
        decimal Vat { get; }

        /// <summary>
        /// Повременная тарификация
        /// </summary>
        bool IsTimePayProduct { get; }

        /// <summary>
        /// Товар продаётся на вес
        /// </summary>
        bool UseBalanceForSell { get; }

        /// <summary>
        /// Артикул
        /// </summary>
        [NotNull]
        string Article { get; }

        /// <summary>
        /// Код быстрого набора
        /// </summary>
        [NotNull]
        string FastCode { get; }

        /// <summary>
        /// Родительская группа
        /// </summary>
        [CanBeNull]
        IProductGroup ProductGroup { get; }

        /// <summary>
        /// Нужно ли печатать блюдо в чеке (для модификаторов)
        /// </summary>
        bool PrechequePrintable { get; }

        /// <summary>
        /// Нужно ли печатать блюдо в чеке (для модификаторов)
        /// </summary>
        bool ChequePrintable { get; }

        /// <summary>
        /// Использовать время приготовления блюда из типа места приготовления
        /// </summary>
        bool UseDefaultCookingTime { get; }

        /// <summary>
        /// Время приготовления в нормальном режиме работы
        /// </summary>
        TimeSpan CookingTimeNormal { get; }

        /// <summary>
        /// Время приготовления в пиковом режиме работы
        /// </summary>
        TimeSpan CookingTimePeak { get; }

        /// <summary>
        /// Готовить вместе с основным блюдом (для модификаторов)
        /// </summary>
        bool CookWithMainDish { get; }

        /// <summary>
        /// Тип места приготовления
        /// </summary>
        [CanBeNull]
        ICookingPlaceType CookingPlaceType { get; }

        /// <summary>
        /// Пользовательские свойства продукта
        /// </summary>
        [NotNull]
        IEnumerable<IProductTag> ProductTags { get; }

        /// <summary>
        /// Аллергены
        /// </summary>
        [NotNull]
        IEnumerable<string> Allergens { get; }

    }
}
