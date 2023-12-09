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
    /// Редактирование приготовления партии блюда
    /// </summary>
    public interface IProductCookEdit : ICommonGroup
    {
        /// <summary>
        /// Блюдо
        /// </summary>
        [NotNull]
        IProduct Product { get; }

        /// <summary>
        /// Старое количество
        /// </summary>
        decimal OldAmount { get; }

        /// <summary>
        /// Новое количество
        /// </summary>
        decimal NewAmount { get; }

        /// <summary>
        /// Кем приготовлено
        /// </summary>
        [NotNull]
        IUser CookedBy { get; }

        /// <summary>
        /// Время приготовления
        /// </summary>
        DateTime CookTime { get; }

    }
}
