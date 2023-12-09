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
    /// Элемент заказа (блюдо)
    /// </summary>
    public interface IProductItem : IOrderItem
    {
        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        IProductItemComment Comment { get; }

        /// <summary>
        /// Курс
        /// </summary>
        int Course { get; }

        /// <summary>
        /// Время печати
        /// </summary>
        DateTime? PrintTime { get; }

        /// <summary>
        /// Время начала приготовления
        /// </summary>
        DateTime? CookingStartTime { get; }

        /// <summary>
        /// Время окончания приготовления
        /// </summary>
        DateTime? CookingFinishTime { get; }

        /// <summary>
        /// Время подачи
        /// </summary>
        DateTime? DeliverTime { get; }

        /// <summary>
        /// Номер подачи
        /// </summary>
        int ServeGroupNumber { get; }

        /// <summary>
        /// Тип места приготовления
        /// </summary>
        [NotNull]
        ICookingPlaceType CookingPlaceType { get; }

        /// <summary>
        /// Место приготовления
        /// </summary>
        [NotNull]
        IRestaurantSection CookingPlace { get; }

        /// <summary>
        /// У элемента заказа есть микс
        /// </summary>
        bool HasMix { get; }

        /// <summary>
        /// Микс удалён (только для элементов заказа с HasMix == true)
        /// </summary>
        bool MixDeleted { get; }

        /// <summary>
        /// Информация о компонентах составных блюд
        /// </summary>
        [CanBeNull]
        ICompoundOrderItemInfo CompoundsInfo { get; }

        /// <summary>
        /// Простые модификаторы, доступные для добавления
        /// </summary>
        [NotNull]
        IEnumerable<ISimpleModifier> SimpleModifiers { get; }

        /// <summary>
        /// Групповые модификаторы, доступные для добавления
        /// </summary>
        [NotNull]
        IEnumerable<IGroupModifier> GroupModifiers { get; }

        /// <summary>
        /// Модификаторы, добавленные в заказ
        /// </summary>
        [NotNull]
        IEnumerable<IModifierEntry> ModifierEntries { get; }

    }
}
