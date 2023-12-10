using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    /// <summary>
    /// Интерфейс для позиций складских документов, у которых можно задать размер блюда.
    /// </summary>
    /// <remarks>
    /// Т.к. сам размер блюда не участвует в складском учете,
    /// размер можно задавать только при списании по ингредиентам.
    /// </remarks>
    /// <see cref="Resto.Common.ProductSizeConstants"/>
    partial interface ProductSizeDocumentItem
    {
        /// <summary>
        /// Размер блюда. Null, если блюдо не использует шкалу размеров.
        /// Кроме того, может оказаться null и у блюда со шкалой размеров,
        /// например, в результате импорта через API или при репликации со старого RMS.
        /// </summary>
        ProductSize ProductSize { get; set; }

        /// <summary>
        /// Коэффициент списания блюда (модификатора), зафиксированный в момент импорта с фронта.
        /// </summary>
        /// <remarks>
        /// Например, если "двойная" порция модификатора списывает только 1.8 обычной
        /// (то есть, в ProductSizeFactors задано (0.0 -> 1.0; 2.0 -> 0.9)),
        /// и заказаны три блюда с двойными порциями модификатора,
        /// в строке документа для этого модификатора запишется:
        /// <ul>
        /// <li>Amount = 1.0 * 6 = 6.0</li>
        /// <li>AmountFactor = 0.9</li>
        /// <li>списано будет = 0.9 * 6 = 5.4</li>
        /// </ul>
        /// </remarks>
        /// <returns>коэффициент; единица, если размер блюда не задан.</returns>
        decimal AmountFactor { get; set; }
    }
}
