using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Шрифт в ячейке.</summary>
    public class FontAttr : XAttribute
    {
        private FontAttr(FontAttrValue value) : base(TagTable.FontAttribute, value.GetName()) { }

        /// <summary>Шрифт: нулевой, мелкий, используется по умолчанию.</summary>
        public static FontAttr F0 { get { return new FontAttr(FontAttrValue.F0); } }
        /// <summary>Шрифт: первый, средний.</summary>
        public static FontAttr F1 { get { return new FontAttr(FontAttrValue.F1); } }
        /// <summary>Шрифт: второй, крупный.</summary>
        public static FontAttr F2 { get { return new FontAttr(FontAttrValue.F2); } }
    }
}
