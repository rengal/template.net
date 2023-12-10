using System.Drawing;

namespace Resto.Common
{
    public sealed class XmlFont
    {
        public string FontFamily;
        public GraphicsUnit GraphicsUnit;
        public float Size;
        public FontStyle Style;

        public XmlFont()
        {
        }

        public XmlFont(Font f)
        {
            FontFamily = f.FontFamily.Name;
            GraphicsUnit = f.Unit;
            Size = f.Size;
            Style = f.Style;
        }

        public Font ToFont()
        {
            return new Font(FontFamily, Size, Style, GraphicsUnit);
        }

        public override string ToString()
        {
            return FontFamily + " " + Style + "," + Size + GraphicsUnit;
        }
    }
}
