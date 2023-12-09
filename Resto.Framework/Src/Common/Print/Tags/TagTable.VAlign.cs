using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Resto.Framework.Common.Print.VirtualTape;
using Resto.Framework.Common.Print.VirtualTape.Fonts;

namespace Resto.Framework.Common.Print.Tags
{
    public partial class TagTable {
        private sealed class VAlign
        {
            public static readonly VAlign Top = new VAlign((lines, lineFonts, lineFontGlyphs, maxLinesCount, c) =>
            {
                for (var i = lines.Count; i < maxLinesCount; i++)
                {
                    lines.Add(GetDocumentLine(c));
                    lineFonts[i] = c.InitialFont;
                }
                for(var i = lineFontGlyphs.Count; i < maxLinesCount; i++)
                    lineFontGlyphs[i] = c.InitialFontGlyphFlags;

            });

            public static readonly VAlign Center = new VAlign((lines, lineFonts, lineFontGlyphs, maxLinesCount, c) => 
            {
                var firstHalf = (maxLinesCount - lines.Count) / 2;

                for (var i = lineFonts.Count - 1; i >= 0; i--)
                    lineFonts[i + firstHalf] = lineFonts[i];

                for (var i = lineFontGlyphs.Count - 1; i >= 0; i--)
                    lineFontGlyphs[i + firstHalf] = lineFontGlyphs[i];

                for (var i = 0; i < firstHalf; i++)
                {
                    lines.Insert(0, GetDocumentLine(c));
                    lineFonts[i] = c.InitialFont;
                    lineFontGlyphs[i] = c.InitialFontGlyphFlags;
                }

                for (var i = firstHalf; i < maxLinesCount; i++)
                {
                    lines.Add(GetDocumentLine(c));
                    lineFonts[i] = c.InitialFont;
                    lineFontGlyphs[i] = c.InitialFontGlyphFlags;
                }           
            });

            public static readonly VAlign Bottom = new VAlign((lines, lineFonts, lineFontGlyphs, maxLinesCount, c) =>
            {
                var count = maxLinesCount - lines.Count;
                for (var i = lineFonts.Count - 1; i >= 0; i--)
                    lineFonts[i + count] = lineFonts[i];

                for (var i = lineFontGlyphs.Count - 1; i >= 0; i--)
                    lineFontGlyphs[i + count] = lineFontGlyphs[i];

                for (var i = 0; i < count; i++)
                {
                    lines.Insert(0, GetDocumentLine(c));
                    lineFonts[i] = c.InitialFont;
                    lineFontGlyphs[i] = c.InitialFontGlyphFlags;
                }
            });

            private static DocumentLine GetDocumentLine(TableColumnTape c)
            {
                var s = new string(' ', c.Width);
                var line = new DocumentLine(new XElement(c.InitialFont.Id, s), s, string.Empty);
                return line;
            }

            private Action<List<DocumentLine>, IDictionary<int, Font>, IDictionary<int, FontGlyphFlags>, int, TableColumnTape> FormatAction { get; set; }            
            
            private VAlign(Action<List<DocumentLine>, IDictionary<int, Font>, IDictionary<int, FontGlyphFlags>, int, TableColumnTape> formatAction)
            {
                FormatAction = formatAction;
            }

            public void Format(List<DocumentLine> lines, IDictionary<int, Font> lineFonts, IDictionary<int, FontGlyphFlags> fontGlyphs, int maxLinesCount, TableColumnTape column)
            {
                FormatAction(lines, lineFonts, fontGlyphs, maxLinesCount, column);
            }
        }
    }
}
