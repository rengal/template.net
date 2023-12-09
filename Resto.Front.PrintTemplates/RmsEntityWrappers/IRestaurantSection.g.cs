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
    /// Отделение
    /// </summary>
    public interface IRestaurantSection
    {
        /// <summary>
        /// Название отделения
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Нужно ли печатать комментарий к элементу заказа (блюду) в чеке
        /// </summary>
        bool PrintProductItemCommentInCheque { get; }

        /// <summary>
        /// Нужно ли печатать штрихкод к элементу заказа (блюду) в сервисном чеке
        /// </summary>
        [Obsolete("Используйте свойство PrintKitchenBarcodeType вместо PrintBarcodeInServiceCheque")]
        bool PrintBarcodeInServiceCheque { get; }

        /// <summary>
        /// Тип печати штрихкода в сервисном чеке
        /// </summary>
        PrintKitchenBarcodeType PrintKitchenBarcodeType { get; }

        /// <summary>
        /// Нужно ли показывать гостей в даном отделении
        /// </summary>
        bool DisplayGuests { get; }

        /// <summary>
        /// Печатается ли в отделении сводный сервисный чек (true) или сервисный чек отделения (false)
        /// </summary>
        bool PrintSummaryServiceCheque { get; }

    }
}
