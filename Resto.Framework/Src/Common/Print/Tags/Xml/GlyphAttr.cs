using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>
    /// Атрибут начертания шрифтов
    /// </summary>
    public sealed class GlyphAttr : XAttribute
    {
        private GlyphAttr(string glyph) : base(glyph, TagFont.FontGlyphOn) { }

        /// <summary>Применение наклонного начертания.</summary>
        public static GlyphAttr Italic => new GlyphAttr(TagFont.ItalicFontAttribute);

        /// <summary>Применение жирного начертания.</summary>
        public static GlyphAttr Bold => new GlyphAttr(TagFont.BoldFontAttribute);

        /// <summary>Применение реверсивного начертания.</summary>
        public static GlyphAttr Reverse => new GlyphAttr(TagFont.ReverseFontAttribute);

        /// <summary>Применение подчеркнутого начертания.</summary>
        public static GlyphAttr Underline => new GlyphAttr(TagFont.UnderlineFontAttribute);
    }
}
