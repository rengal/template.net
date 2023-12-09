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
    /// Комментарий к элементу заказа (блюду)
    /// </summary>
    public interface IProductItemComment
    {
        /// <summary>
        /// Текст комментария
        /// </summary>
        [NotNull]
        string Text { get; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        bool Deleted { get; }

    }
}
