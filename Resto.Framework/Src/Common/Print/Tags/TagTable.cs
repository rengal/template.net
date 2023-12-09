using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

using Resto.Framework.Common.Print.Tags.Xml;
using Resto.Framework.Common.Print.VirtualTape;
using Resto.Framework.Common.Print.VirtualTape.Fonts;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed partial class TagTable : TagBase
    {
        internal const string ColumnsTag = "columns";
        internal const string CellsTag = "cells";
        internal const string ColumnTag = "column";
        internal const string CellTag = "c";
        internal const string TextCellTag = "ct";
        internal const string FontAttribute = "font";
        internal const string ItalicFontAttribute = "italic";
        internal const string BoldFontAttribute = "bold";
        internal const string ReverseFontAttribute = "reverse";
        internal const string UnderlineFontAttribute = "underline";
        internal const string FontGlyphOn = "on";
        internal const string FontGlyphOff = "off";

        internal const string ColspanAttr = "colspan";
        internal const string CellspacingAttr = "cellspacing";
        internal const string MinwidthAttr = "minwidth";
        internal const string MaxwidthAttr = "maxwidth";
        internal const string AutowidthAttr = "autowidth";
        internal const string SpaceColumnTagName = "spacecolumn";

        public const char LeftToRightMark = '\u200E';

        public static readonly TagTable Instance;

        private TagTable() : base("table") { }

        static TagTable()
        {
            Instance = new TagTable();
        }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            var allColumns = ((XmlElement)node).GetElementsByTagName(ColumnsTag)[0].ChildNodes.OfType<XmlNode>().ToList();
            var regularColumnsCount = allColumns.Count(c => c.Name == ColumnTag);
            var cells = ((XmlElement)node).GetElementsByTagName(CellsTag)[0].ChildNodes.OfType<XmlNode>().ToList();

            if (cells.Count == 0)
                return;

            // Preprocess tags
            PreProcessZeroColSpans(regularColumnsCount, cells);
            var cellSpacing = GetIntAttr(node, CellspacingAttr, 1);
            if (cellSpacing != 0 || allColumns.Count != regularColumnsCount)
                PreProcessCellSpacing(node.OwnerDocument, allColumns, cells, regularColumnsCount, cellSpacing);
            PreProcessAutoWidthColumns(node.OwnerDocument, allColumns, cells);

            // Prepare formatting
            var columnTapes = GetColumns(allColumns, tape);
            CalculateAllWidths(columnTapes, tape);
            var rowSpanConfigs = new Dictionary<List<int>, List<TableColumnTape>>(new IntArrayComparer());
            // Format table
            for (var i = 0; i < cells.Count; )
            {
                var currentRowSpanConfig = GetRowSpanConfig(cells, i, columnTapes.Count);
                if (!rowSpanConfigs.ContainsKey(currentRowSpanConfig))
                {
                    rowSpanConfigs.Add(currentRowSpanConfig, CreateColumnsForRowSpanConfig(columnTapes, currentRowSpanConfig, tape));
                }

                var currentColumns = rowSpanConfigs[currentRowSpanConfig];
                ApplyInheritance(currentColumns, currentRowSpanConfig, tape, columnTapes, cells[i]);

                var cellsInRow = currentRowSpanConfig.Count;
                for (var j = 0; j < cellsInRow; j++)
                {
                    base.Format(currentColumns[j], cells[i + j], tags);
                }
                FillLines(tape, currentColumns, cellsInRow);
                i += cellsInRow;
            }
            tape.StartNewLineWithFontEsc();
        }

        private static int GetIntAttr(XmlNode node, string attrName, int defaultValue)
        {
            return node.Attributes[attrName] != null ? int.Parse(node.Attributes[attrName].Value) : defaultValue;
        }

        private static string GetStringAttr(XmlNode node, string attrName, string defaultValue = "")
        {
            return node.Attributes[attrName] != null ? node.Attributes[attrName].Value : defaultValue;
        }

        [CanBeNull]
        private static Font GetFont(FontPack fontPack, string font)
        {
            if (font == FontAttrValue.F0.GetName())
                return fontPack.Font0;
            if (font == FontAttrValue.F1.GetName())
                return fontPack.Font1;
            if (font == FontAttrValue.F2.GetName())
                return fontPack.Font2;
            return null;
        }

        private static List<TableColumnTape> CreateColumnsForRowSpanConfig(List<TableColumnTape> columns, List<int> rowSpanConfig, ITape tape)
        {
            var config = new List<TableColumnTape>();
            for (int i = 0, j = 0; i < columns.Count && j < rowSpanConfig.Count; i += rowSpanConfig[j++])
            {
                var sourceColumns = columns.Skip(i).Take(rowSpanConfig[j]).ToList();
                var resultColumn = TableColumnTape.GetCopy(sourceColumns.First(), tape);

                resultColumn.OriginalFontsPack.Font0.Width = sourceColumns.Sum(c => c.Fonts.Font0.Width);
                resultColumn.OriginalFontsPack.Font1.Width = sourceColumns.Sum(c => c.Fonts.Font1.Width);
                resultColumn.OriginalFontsPack.Font2.Width = sourceColumns.Sum(c => c.Fonts.Font2.Width);

                resultColumn.Width = sourceColumns.Sum(c => c.Width);
                resultColumn.SetAlign(sourceColumns.First().Align);
                config.Add(resultColumn);
            }
            return config;
        }

        private static List<int> GetRowSpanConfig(IEnumerable<XmlNode> cellsNode, int startIndex, int count)
        {
            var spanSum = 0;
            return cellsNode
                .Skip(startIndex)
                .TakeWhile(node => (spanSum += GetColSpanAttr(node)) <= count)
                .Select(GetColSpanAttr)
                .ToList();
        }

        private static int GetColSpanAttr(XmlNode node)
        {
            return node.Attributes[ColspanAttr] != null ? int.Parse(node.Attributes[ColspanAttr].Value) : 1;
        }

        private static void CalculateAllWidths(List<TableColumnTape> columns, ITape tape)
        {
            var totalWidth = tape.CurrentFont.Width;

            // Случай, когда все колонки уместить по ширине ленты невозможно
            // (например, ширина ленты — 20, количество колонок — 30),
            // обрабатывается выбросом исключения, т.к. пока непонятно, как форматировать такой текст для печати.
            if (totalWidth < columns.Count)
                throw new InvalidOperationException(string.Format("Can't allocate {0} columns in tape with width {1}", columns.Count, totalWidth));

            var columnsWithUndefinedWidth = columns
                .Where(ci => ci.WidthType == WidthType.Auto)
                .ToList();
            var columnsWithDefinedWidth = columns
                .Where(ci => ci.WidthType != WidthType.Auto)
                .ToList();
            var totalDefinedWidth = columnsWithDefinedWidth.Sum(ci => ci.Width);

            if (columnsWithUndefinedWidth.Count > 0)
            {
                DistributeWidthInUniform(columnsWithUndefinedWidth, totalWidth - totalDefinedWidth);
            }
            // если таблица получается уже чем лента и нет колонок с незданной шириной (WidthType.Auto), 
            // то растягиваем таблицу
            else if (totalDefinedWidth < totalWidth)
            {
                // растягиваем не разделительные колонки, если они есть, иначе все
                var columnsForIncrease = columns.Where(ci => !ci.IsSpaceColumn).ToList();
                if (!columnsForIncrease.Any())
                    columnsForIncrease = columns.ToList();
                // вычисляем суммарную ширину колонок, которые не будут растягиваться
                var totalElapsedWidth = columns.Where(c => !columnsForIncrease.Contains(c)).Sum(c => c.Width);
                var delta = totalWidth - totalElapsedWidth - columnsForIncrease.Sum(c => c.Width);
                // распределяем оставшуюся ширину пропорционально по выбранным колонкам
                DistributeWidthDeltaInProportion(columnsForIncrease, delta);
            }

            // Корректируем ширину колонок на случай, если есть колонки с шириной меньше 1.
            // Это всегда те колонки, ширина которых жёстко не задана (WidthType == WidthType.Auto).
            // Задаём их ширину равной 1, недостающее место отнимаем у колонок с жёстко заданной шириной,
            // пропорционально их ширине.
            // Такая ситуация может воникнуть, если ширина ленты меньше или равна суммарной
            // ширине колонок с жёстко заданной шириной (баг № 7922).
            if (columnsWithUndefinedWidth.Any(c => c.Width < 1))
            {
                var summaryWidthSizeToReduce = 0;
                foreach (var column in columnsWithUndefinedWidth.Where(c => c.Width < 1))
                {
                    summaryWidthSizeToReduce += 1 - column.Width;
                    column.Width = 1;
                }
                DistributeWidthDeltaInProportion(columnsWithDefinedWidth, -summaryWidthSizeToReduce);
            }

            UpdateAllFontWidth(columns, tape, ct => ct.OriginalFontsPack);
            UpdateAllFontWidth(columns, tape, ct => ct.Fonts);
        }

        private static void DistributeWidthInUniform([NotNull] List<TableColumnTape> columnsToDistribute, int widthToDistribute)
        {
            if (columnsToDistribute == null)
                throw new ArgumentNullException(nameof(columnsToDistribute));

            var width = Math.Max((widthToDistribute) / columnsToDistribute.Count, 0);
            var rest = Math.Max(widthToDistribute - columnsToDistribute.Count * width, 0);

            for (var i = 0; i < columnsToDistribute.Count - rest; i++)
                columnsToDistribute[i].Width = width;

            for (var i = columnsToDistribute.Count - rest; i < columnsToDistribute.Count; i++)
                columnsToDistribute[i].Width = width + 1;
        }

        /// <summary>
        /// Распределяет ширину <paramref name="widthSizeDelta"/> среди колонок, пропорционально их текущей ширине.
        /// </summary>
        /// <param name="columns">Колонки которым нужно изменить ширину.</param>
        /// <param name="widthSizeDelta">
        /// Ширина, которая должна быть распределена. 
        /// Если число больше нуля, ширины колонок будут увеличены на <paramref name="widthSizeDelta"/> и уменьшены если меньше нуля.
        /// </param>
        private static void DistributeWidthDeltaInProportion(ICollection<TableColumnTape> columns, int widthSizeDelta)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            if (columns.Count == 0)
                throw new ArgumentException("Columns list is empty", nameof(columns));

            if (widthSizeDelta == 0)
                return;

            var sign = Math.Sign(widthSizeDelta);
            var delta = Math.Abs(widthSizeDelta);

            var sortedColumns = columns.OrderByDescending(c => c.Width).ToArray();
            var valuesToChange = AllocateValueInProportion(delta, sortedColumns.Select(c => c.Width).ToArray());

            for (var i = 0; i < sortedColumns.Length; i++)
                sortedColumns[i].Width = sortedColumns[i].Width + sign * valuesToChange[i];
        }

        private static int[] AllocateValueInProportion(int value, ICollection<int> weights)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value should be non-negative");
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));
            if (weights.Count == 0)
                throw new ArgumentException("Weights list is empty", nameof(weights));

            var weightsSum = weights.Sum();

            var valueForUnitOfWeight = (double)value / weightsSum;
            var allocation = weights.Select(w => (int)(w * valueForUnitOfWeight)).ToArray();

            // Остаток от деления распределяем по единице, с начала списка распределённых значений
            for (var i = 0; i < value - allocation.Sum(); i++)
            {
                ++allocation[i];
            }

            return allocation;
        }

        private static void FillLines(ITape tape, List<TableColumnTape> columns, int columnsCount)
        {
            var lineBlocks = columns
                .Select(c => c.GetLines())
                    .ToList();

            var maxLinesCount = lineBlocks.Max(l => l.Count);

            for (var i = 0; i < columns.Count; i++)
            {
                columns[i].FormatVAlign(lineBlocks[i], maxLinesCount);
            }

            AppendLines(columns, lineBlocks, tape, maxLinesCount, columnsCount);
            columns.ForEach(c => c.Clean());
        }

        private static void AppendLines(IList<TableColumnTape> columns, IList<List<DocumentLine>> lineBlocks,
            ITape tape, int maxLinesCount, int columnsCount)
        {
            var buffer = new StringBuilder();
            var bufferNoEsc = new StringBuilder();
            var useLeftToRightMarkDefault = tape.RightToLeftSupported && tape.Fonts.Font0.Esc.IsNullOrEmpty() && tape.Fonts.Font1.Esc.IsNullOrEmpty() && tape.Fonts.Font2.Esc.IsNullOrEmpty();
            for (var i = 0; i < maxLinesCount; i++)
            {
                // добавляем смену шрифта только для самой внутренней таблицы
                var firstBlock = lineBlocks[0][i].Content;
                var tapeFont = tape.Fonts[columns[0].GetLineFont(i).Id];
                var tapeFontGlypg = tape.Fonts.FontGlyph;
                var tapeFontGlyphFlags = columns[0].GetLineFontGlyph(i);
                var useLeftToRightMark = useLeftToRightMarkDefault;
                var fontId = tapeFont.Id;
                if (!firstBlock.StartsWith(tape.Fonts.Font0.Esc) && !firstBlock.StartsWith(tape.Fonts.Font1.Esc) && !firstBlock.StartsWith(tape.Fonts.Font2.Esc))
                {
                    buffer.Append(tapeFont.Esc);
                    useLeftToRightMark = tape.RightToLeftSupported;
                }
                else
                {
                    fontId = lineBlocks[0][i].Element.Name.LocalName;
                }
                for (var j = 0; j < columnsCount; j++)
                {
                    if (useLeftToRightMark)
                        buffer.Append(LeftToRightMark);
                    if (tapeFontGlyphFlags.IsBold)
                        buffer.Append(tapeFontGlypg.BoldBeginEsc);
                    if (tapeFontGlyphFlags.IsItalic)
                        buffer.Append(tapeFontGlypg.ItalicBeginEsc);
                    if (tapeFontGlyphFlags.IsReverse)
                        buffer.Append(tapeFontGlypg.ReverseBeginEsc);
                    if (tapeFontGlyphFlags.IsUnderline)
                        buffer.Append(tapeFontGlypg.UnderlineBeginEsc);

                    buffer.Append(lineBlocks[j][i].Content);
                    bufferNoEsc.Append(lineBlocks[j][i].Element.Value);

                    if (tapeFontGlyphFlags.IsUnderline)
                        buffer.Append(tapeFontGlypg.UnderlineEndEsc);
                    if (tapeFontGlyphFlags.IsReverse)
                        buffer.Append(tapeFontGlypg.ReverseEndEsc);
                    if (tapeFontGlyphFlags.IsItalic)
                        buffer.Append(tapeFontGlypg.ItalicEndEsc);
                    if (tapeFontGlyphFlags.IsBold)
                        buffer.Append(tapeFontGlypg.BoldEndEsc);
                }

                for (var j = columnsCount; j < columns.Count; j++)
                {
                    if (useLeftToRightMark) buffer.Append(LeftToRightMark);
                    buffer.Append(new string(' ', columns[j].Width));
                    bufferNoEsc.Append(new string(' ', columns[j].Width));
                }
                tape.AppendLine(new XElement(fontId, bufferNoEsc.ToString()), buffer.ToString(), string.Empty);
                buffer.Clear();
                bufferNoEsc.Clear();
            }
        }

        private static List<TableColumnTape> GetColumns(IEnumerable<XmlNode> columns, ITape tape)
        {
            return columns.Select(columnNode => TableColumnTape.GetInstance(columnNode, tape)).ToList();
        }

        private static void UpdateAllFontWidth(List<TableColumnTape> columns,
            ITape tape, Func<TableColumnTape, FontPack> fontPackSelector)
        {
            UpdateFontWidth(columns, tape, fp => fp.Font0, fontPackSelector);
            UpdateFontWidth(columns, tape, fp => fp.Font1, fontPackSelector);
            UpdateFontWidth(columns, tape, fp => fp.Font2, fontPackSelector);
        }

        private static void UpdateFontWidth(List<TableColumnTape> columns,
            ITape tape, Func<FontPack, Font> fontSelector, Func<TableColumnTape, FontPack> fontPackSelector)
        {
            var totalFontWidth = fontSelector(tape.Fonts).Width;
            var totalWidth = tape.CurrentFont.Width;

            columns.ForEach(c => fontSelector(fontPackSelector(c)).Width = (c.Width * totalFontWidth) / totalWidth);

            var rest = totalFontWidth - columns.Sum(c => fontSelector(fontPackSelector(c)).Width);
            for (var i = columns.Count - rest; i < columns.Count; i++)
            {
                fontSelector(fontPackSelector(columns[i])).Width++;
            }
        }

        private void ApplyInheritance(IList<TableColumnTape> targetColumns, List<int> rowSpanConfig, ITape parentTape, List<TableColumnTape> sourceColumnTapes, XmlNode firstCell)
        {
            for (int i = 0, j = 0; i < sourceColumnTapes.Count && j < rowSpanConfig.Count; i += rowSpanConfig[j++])
            {
                var columnTape = targetColumns[j];
                var sourceColumnTape = rowSpanConfig[j] == 1 ? sourceColumnTapes[i] : null;

                var needToFreezeFonts = rowSpanConfig.Count > 1;
                columnTape.ApplyInheritance(parentTape, sourceColumnTape, firstCell, needToFreezeFonts);
            }
        }
    }
}