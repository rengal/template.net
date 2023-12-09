namespace Resto.Framework.Common.Print.VirtualTape.Fonts
{
    public class FontGlyphFlags
    {
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsReverse { get; set; }
        public bool IsUnderline { get; set; }

        public FontGlyphFlags(bool isBold, bool isItalic, bool isReverse, bool isUnderline)
        {
            IsBold = isBold;
            IsItalic = isItalic;
            IsReverse = isReverse;
            IsUnderline = isUnderline;
        }

        public FontGlyphFlags() { }
    }
}
