using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed partial class TagTable
    {
        private static void PreProcessZeroColSpans(int regularColumnsCount, List<XmlNode> cells)
        {
            // TODO <ap> Подсчитывать количество незанятых ячеек до конца строки
            foreach (var cell in cells.Where(cell => GetIntAttr(cell, ColspanAttr, 1) == 0))
                cell.Attributes[ColspanAttr].Value = regularColumnsCount.ToString();
        }

        private static void PreProcessCellSpacing(XmlDocument doc, List<XmlNode> columns, List<XmlNode> cells, int regularColumnsCount, int cellSpacing)
        {
            columns.Set(AddSpaceColumns(doc, columns, cellSpacing).ToList());

            var newCells = new List<XmlNode>();
            for (var i = 0; i < cells.Count; )
            {
                var spanSum = 0;
                var lineLength = cells.Skip(i).TakeWhile(c => (spanSum += GetIntAttr(c, ColspanAttr, 1)) <= regularColumnsCount).Count();
                if (lineLength == 0)
                    throw new InvalidOperationException("Incorrect row spans.");
                newCells.AddRange(cells.Skip(i).Take(lineLength).SelectWithSeparator(ExpandCellSpan, (prev, next) => CopyEmptyCell(doc, prev)));
                i += lineLength;
            }
            cells.Set(newCells);
        }

        /// <summary>
        /// Заполняет интервалы между столбцами дополнительными фиктивными столбцами с указанной шириной <see cref="defaultCellSpace"/>, если таковые не заданы явно.
        /// </summary>
        [NotNull]
        private static IEnumerable<XmlNode> AddSpaceColumns([NotNull] XmlDocument doc, [NotNull] IEnumerable<XmlNode> columns, int defaultCellSpace)
        {
            var currentColumnIndex = 0;
            var previousColumnIsSpace = false;

            foreach (var column in columns)
            {
                var isSpaceColumn = column.Name == SpaceColumnTagName;
                if (isSpaceColumn && currentColumnIndex == 0)
                    throw new InvalidOperationException("Leftmost column cannot be space column. Space columns must separate regular columns.");
                if (isSpaceColumn && previousColumnIsSpace)
                    throw new InvalidOperationException(string.Format("Space columns cannot be consecutive ({0}, {1}).", currentColumnIndex - 1, currentColumnIndex));

                // добавляем фиктивный столбец
                if (isSpaceColumn || (currentColumnIndex > 0 && !previousColumnIsSpace))
                {
                    // если фиктивный столбец в разметке, его ширина задана явно, иначе используется ширина по умолчанию
                    var spaceColumnWidth = isSpaceColumn ? GetIntAttr(column, WidthAttribute, 0) : defaultCellSpace;
                    var spaceColumn = doc.CreateElement(ColumnTag);
                    var widthAttr = spaceColumn.Attributes.Append(doc.CreateAttribute(WidthAttribute));
                    widthAttr.Value = spaceColumnWidth.ToString();
                    spaceColumn.Attributes.Append(doc.CreateAttribute(IsSpaceAttribute));

                    currentColumnIndex++;
                    yield return spaceColumn;
                }

                // добавляем столбец с данными
                if (!isSpaceColumn)
                {
                    currentColumnIndex++;
                    yield return column;
                }

                previousColumnIsSpace = isSpaceColumn;
            }
        }

        private static XmlNode ExpandCellSpan(XmlNode cell)
        {
            var colSpanAttr = cell.Attributes[ColspanAttr];
            if (colSpanAttr != null)
                colSpanAttr.Value = (int.Parse(colSpanAttr.Value) * 2 - 1).ToString();
            return cell;
        }

        private static XmlNode CopyEmptyCell(XmlDocument doc, XmlNode cellPrev)
        {
            var cellNode = doc.CreateElement(CellTag);
            if (cellPrev.FirstChild != null && cellPrev.FirstChild.Name == TagFill.Fill.Name)
            {
                // <c><fill symbols=""><np/></fill></c>
                var lineNode = cellNode.AppendChild(doc.CreateElement(TagFill.Fill.Name));
                lineNode.AppendChild(doc.CreateElement(TagNp.Instance.Name));
                var symbolsAttr = lineNode.Attributes.Append(doc.CreateAttribute(TagFill.SYMBOLS_ATTRIBUTE));
                symbolsAttr.Value = cellPrev.FirstChild.Attributes[TagFill.SYMBOLS_ATTRIBUTE].Value;
            }
            return cellNode;
        }

        private static void PreProcessAutoWidthColumns(XmlDocument doc, List<XmlNode> columns, IEnumerable<XmlNode> cells)
        {
            var columnInfos = columns
                .Select(column => new AutoWidthColumnInfo { Node = column.Attributes[AutowidthAttr] != null ? column : null })
                .ToList();

            var span = 0;
            foreach (var cell in cells)
            {
                var columnInfo = columnInfos[span];
                if (columnInfo.Node != null && cell.Name == TextCellTag && cell.InnerText != null)
                    columnInfo.MaxCellWidth = Math.Max(columnInfo.MaxCellWidth, cell.InnerText.Length);
                span += GetIntAttr(cell, ColspanAttr, 1);
                if (span >= columns.Count)
                    span = 0;
            }

            foreach (var column in columnInfos.Where(c => c.Node != null))
            {
                var widthValue = Math.Min(
                    Math.Max(
                        column.MaxCellWidth,
                        GetIntAttr(column.Node, MinwidthAttr, 1)),
                    GetIntAttr(column.Node, MaxwidthAttr, int.MaxValue))
                    .ToString();
                var widthAttr = column.Node.Attributes[WidthAttribute] ?? column.Node.Attributes.Append(doc.CreateAttribute(WidthAttribute));
                widthAttr.Value = widthValue;
            }
        }

        private sealed class AutoWidthColumnInfo
        {
            public XmlNode Node { get; set; }
            public int MaxCellWidth { get; set; }
        }
    }
}
