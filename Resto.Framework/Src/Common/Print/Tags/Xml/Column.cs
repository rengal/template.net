using System;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>
    /// Разделительный столбец таблицы.
    /// Используется для формирования таблиц с различными интервалами между столбцами.
    /// </summary>
    /// <remarks>
    /// Для таблиц с одинаковыми интервалами используется <see cref="CellSpacingAttr"/>.
    /// При совместном использовании интервалы, указанные с помощью <see cref="SpaceColumn"/>, получат явно заданную ширину, 
    /// а остальные — <see cref="CellSpacingAttr"/> или ширину по умолчанию.
    /// </remarks>
    public sealed class SpaceColumn : ColumnBase
    {
        public SpaceColumn(int width)
            : base(TagTable.SpaceColumnTagName, new WidthAttr(width))
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width), width, "Space column width cannot be less than zero.");
        }
    }
}