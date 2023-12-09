using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Resto.Framework.Common.Print.Alignment;
using Resto.Framework.Common.Print.Tags.Xml;
using Resto.Framework.Common.Print.TextBlockFormatters;
using Resto.Framework.Common.Print.VirtualTape;
using Resto.Framework.Common.Print.VirtualTape.Fonts;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed partial class TagTable
    {
        internal const string AlignAttribute = "align";
        internal const string FormatterAttribute = "formatter";
        internal const string ValignAttribute = "valign";
        internal const string WidthAttribute = "width";
        private const string IsSpaceAttribute = "isSpace";

        private static readonly string ValignTop = VAlignAttrValue.Top.GetName();
        private static readonly string ValignCenter = VAlignAttrValue.Center.GetName();
        private static readonly string ValignBottom = VAlignAttrValue.Bottom.GetName();

        private static readonly Dictionary<string, string> ManualAttributes =
            new[] { WidthAttribute, AutowidthAttr, MinwidthAttr, MaxwidthAttr, IsSpaceAttribute }.ToDictionary(i => i);

        private sealed class TableColumnTape : Tape
        {
            private static readonly TableColumnTapeActions Actions;
            private readonly IDictionary<int, Font> LineFonts;
            //Т.к. FontPack содержит только три экземпляра шрифта, а каждая строка 
            //текста может содержать свою комбинацию начертаний, то вводится данное свойство.
            private readonly IDictionary<int, FontGlyphFlags> LineFontGlyphs;

            static TableColumnTape()
            {
                Actions = new TableColumnTapeActions
                {
                    { AlignAttribute, TagAlign.Center.Name, t => t.SetAlign(AlignCenter.Instance) },
                    { AlignAttribute, TagAlign.Justify.Name, t => t.SetAlign(AlignJustify.Instance) },
                    { AlignAttribute, TagAlign.Left.Name, t => t.SetAlign(AlignLeft.Instance) },
                    { AlignAttribute, TagAlign.Right.Name, t => t.SetAlign(AlignRight.Instance) },
                    { FormatterAttribute, TagFormatter.Cut.Name, t => t.TextBlockFormatter = new CutFormatter() },
                    { FormatterAttribute, TagFormatter.Split.Name, t => t.TextBlockFormatter = new SplitFormatter() },
                    { FormatterAttribute, TagFormatter.Wrap.Name, t => t.TextBlockFormatter = new WrapFormatter() },
                    { ValignAttribute, ValignTop, t => t.VAlign = VAlign.Top },
                    { ValignAttribute, ValignCenter, t => t.VAlign = VAlign.Center },
                    { ValignAttribute, ValignBottom, t => t.VAlign = VAlign.Bottom }
                };
            }

            public Font InitialFont { get; private set; }
            public FontGlyphFlags InitialFontGlyphFlags { get; private set; }
            private VAlign VAlign { get; set; }
            public int Width { get; set; }
            public WidthType WidthType { get; private set; }
            public FontPack OriginalFontsPack { get; set; }
            public bool IsSpaceColumn { get; private set; }


            private TableColumnTape(FontPack fonts, ICharWidthProvider charWidthProvider)
                : base(Copy(fonts), charWidthProvider, string.Empty)
            {
                LineFonts = new SortedDictionary<int, Font>();
                LineFontGlyphs = new SortedDictionary<int, FontGlyphFlags>();
                OriginalFontsPack = Copy(fonts);
            }

            private static FontPack Copy(FontPack fonts)
            {
                return new FontPack(fonts.Font0.Clone(), fonts.Font1.Clone(), fonts.Font2.Clone())
                {
                    BarCode = fonts.BarCode
                };
            }

            public void FormatVAlign(List<DocumentLine> lines, int maxLinesCount)
            {
                var lastFont = LineFonts[LineFonts.Count - 1];
                var lastFontGlyph = LineFontGlyphs[LineFontGlyphs.Count - 1];
                for (var i = LineFonts.Count; i < lines.Count; i++)
                    LineFonts[i] = lastFont;

                for (var i = LineFontGlyphs.Count; i < lines.Count; i++)
                    LineFontGlyphs[i] = lastFontGlyph;
                VAlign.Format(lines, LineFonts, LineFontGlyphs, maxLinesCount, this);
            }

            public override void SetFont(Font font)
            {
                base.SetFont(font);

                var lastFont = LineFonts[LineFonts.Count - 1];
                for (var i = LineFonts.Count; i < StoredLinesCount; i++)
                {
                    LineFonts[i] = lastFont;
                }
                LineFonts[StoredLinesCount] = font;
            }

            public override void SetFontGlyphFlags(FontGlyphFlags fontGlyphFlags)
            {
                base.SetFontGlyphFlags(fontGlyphFlags);

                var lastFontGlyph = LineFontGlyphs[LineFontGlyphs.Count - 1];
                for (var i = LineFontGlyphs.Count; i < StoredLinesCount; i++)
                {
                    LineFontGlyphs[i] = lastFontGlyph;
                }
                LineFontGlyphs[StoredLinesCount] = fontGlyphFlags;
            }

            public Font GetLineFont(int line)
            {
                return LineFonts[line];
            }

            public FontGlyphFlags GetLineFontGlyph(int line)
            {
                return LineFontGlyphs[line];
            }

            public static TableColumnTape GetInstance(XmlNode columnNode, ITape tape)
            {
                var ci = new TableColumnTape(tape.Fonts, tape.CharWidthProvider);

                InheritValues(ci, tape);
                foreach (var attr in columnNode.Attributes.Cast<XmlAttribute>().Where(a => !ManualAttributes.ContainsKey(a.Name)))
                {
                    Actions[attr.Name, attr.Value](ci);
                }
                SetWidth(ci, columnNode, tape.CurrentFont.Width);
                SetIsSpaceColumn(ci, columnNode);

                return ci;
            }

            public static TableColumnTape GetCopy(TableColumnTape source, ITape tape)
            {
                var copy = new TableColumnTape(source.Fonts, source.CharWidthProvider);
                InheritValues(copy, tape);
                return copy;
            }

            private static void InheritValues(TableColumnTape ci, ITape tape)
            {
                ci.SetAlign(tape.Align);
                ci.TextBlockFormatter = tape.TextBlockFormatter;
                ci.VAlign = VAlign.Top;
                ci.Width = 0;
                ci.WidthType = WidthType.Auto;
            }

            private static void SetIsSpaceColumn(TableColumnTape tableColumnTape, XmlNode columnNode)
            {
                tableColumnTape.IsSpaceColumn = columnNode.Attributes[IsSpaceAttribute] == null;
            }

            private static void SetWidth(TableColumnTape ci, XmlNode node, int totalWidth)
            {
                var widthAttr = node.Attributes[WidthAttribute];

                if (widthAttr == null) { return; }

                int width;
                if (int.TryParse(widthAttr.Value, out width))
                {
                    ci.Width = width;
                    ci.WidthType = WidthType.Exactly;
                }
                else if (widthAttr.Value.EndsWith("%") &&
                         int.TryParse(widthAttr.Value.Substring(0, widthAttr.Value.Length - 1), out width))
                {
                    ci.Width = (totalWidth * width) / 100;
                    ci.WidthType = WidthType.Percent;
                }
            }

            private static FontGlyphFlags GetFontGlyphFromCell(XmlNode cell)
            {
                return new FontGlyphFlags(
                    GetStringAttr(cell, BoldFontAttribute) == FontGlyphOn,     //cell font bold
                    GetStringAttr(cell, ItalicFontAttribute) == FontGlyphOn,   // cell font italic
                    GetStringAttr(cell, ReverseFontAttribute) == FontGlyphOn,  // cell font reverse
                    GetStringAttr(cell, UnderlineFontAttribute) == FontGlyphOn);// cell font underline
            }

            internal void ApplyInheritance(ITape parentTape, TableColumnTape sourceColumnTape, XmlNode firstCell, bool needToFreezeFonts)
            {
                // tape inheritance
                TextBlockFormatter = parentTape.TextBlockFormatter;
                SetAlign(parentTape.Align);

                // column inheritance
                if (sourceColumnTape != null)
                {
                    TextBlockFormatter = sourceColumnTape.TextBlockFormatter;
                    SetAlign(sourceColumnTape.Align);
                    VAlign = sourceColumnTape.VAlign;
                }
                
                // cell font override
                var rowFontRaw = firstCell?.Attributes[FontAttribute]?.Value;
                var currentFontGlyphFlags = GetFontGlyphFromCell(firstCell); 

                // шрифты в таблице "замораживаются", если это не таблица с одной колонкой на всю ширину
                var currentFont = GetFont(OriginalFontsPack, rowFontRaw) ?? GetFont(OriginalFontsPack, parentTape.CurrentFont.Id);
                Debug.Assert(currentFont != null);
                
                if (needToFreezeFonts)
                {
                    Fonts.Font0.Width = currentFont.Width;
                    Fonts.Font1.Width = currentFont.Width;
                    Fonts.Font2.Width = currentFont.Width;
                    Fonts.Font0.Esc = Fonts.Font1.Esc = Fonts.Font2.Esc = "";
                }
                else
                {
                    Fonts.Font0.Width = OriginalFontsPack.Font0.Width;
                    Fonts.Font0.Esc = OriginalFontsPack.Font0.Esc;
                    Fonts.Font1.Width = OriginalFontsPack.Font1.Width;
                    Fonts.Font1.Esc = OriginalFontsPack.Font1.Esc;
                    Fonts.Font2.Width = OriginalFontsPack.Font2.Width;
                    Fonts.Font2.Esc = OriginalFontsPack.Font2.Esc;
                }
                currentFont = GetFont(Fonts, currentFont.Id);
                Debug.Assert(currentFont != null);
                InitialFont = currentFont;
                InitialFontGlyphFlags = currentFontGlyphFlags;
                LineFonts[0] = currentFont;
                LineFontGlyphs[0] = currentFontGlyphFlags;
                SetFont(currentFont);
                SetFontGlyphFlags(currentFontGlyphFlags);
            }
        }
    }
}