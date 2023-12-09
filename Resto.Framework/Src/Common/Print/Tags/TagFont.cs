using System;
using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.Tags.Xml;
using Resto.Framework.Common.Print.VirtualTape;
using Resto.Framework.Common.Print.VirtualTape.Fonts;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagFont : TagBase
    {
        public readonly static TagFont F0;
        public readonly static TagFont F1;
        public readonly static TagFont F2;

        internal const string ItalicFontAttribute = "italic";
        internal const string BoldFontAttribute = "bold";
        internal const string ReverseFontAttribute = "reverse";
        internal const string UnderlineFontAttribute = "underline";
        internal const string FontGlyphOn = "on";
        internal const string FontGlyphOff = "off";

        private readonly Func<ITape, Font> GetFontMethod;

        static TagFont()
        {
            F0 = new TagFont(FontAttrValue.F0, tape => tape.Fonts.Font0);
            F1 = new TagFont(FontAttrValue.F1, tape => tape.Fonts.Font1);
            F2 = new TagFont(FontAttrValue.F2, tape => tape.Fonts.Font2);
        }

        private TagFont(FontAttrValue value, Func<ITape, Font> getFontMethod)
            : base(value.GetName())
        {
            GetFontMethod = getFontMethod;
        }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            var oldFont = tape.CurrentFont;
            var oldFontGlyph = tape.CurrentFontGlyphFlags;
            //cell font bold
            var rowFontRawBold = node?.Attributes[BoldFontAttribute] != null && node.Attributes[BoldFontAttribute].Value == FontGlyphOn;
            // cell font italic
            var rowFontRawItalic = node?.Attributes[ItalicFontAttribute] != null && node.Attributes[ItalicFontAttribute].Value == FontGlyphOn;
            // cell font reverse
            var rowFontRawReverse = node?.Attributes[ReverseFontAttribute] != null && node.Attributes[ReverseFontAttribute].Value == FontGlyphOn;
            // cell font underline
            var rowFontRawUnderline = node?.Attributes[UnderlineFontAttribute] != null && node.Attributes[UnderlineFontAttribute].Value == FontGlyphOn;
            var currentFontGlyph = new FontGlyphFlags(rowFontRawBold, rowFontRawItalic, rowFontRawReverse, rowFontRawUnderline);
            tape.SetFont(GetFontMethod(tape));
            tape.SetFontGlyphFlags(currentFontGlyph);
            base.Format(tape, node, tags);
            tape.SetFont(oldFont);
            tape.SetFontGlyphFlags(oldFontGlyph);
        }
    }
}
