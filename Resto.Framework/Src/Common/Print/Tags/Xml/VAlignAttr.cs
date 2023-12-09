using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Вертикальное выравнивание текста в ячейке или столбце таблицы.</summary>
    public sealed class VAlignAttr : XAttribute
    {
        private VAlignAttr(VAlignAttrValue value) : base(TagTable.ValignAttribute, value.GetName()) { }

        /// <summary>Выровнять текст сверху.</summary>
        public static VAlignAttr Top { get { return new VAlignAttr(VAlignAttrValue.Top); } }
        /// <summary>Выровнять текст по центру.</summary>
        public static VAlignAttr Center { get { return new VAlignAttr(VAlignAttrValue.Center); } }
        /// <summary>Выровнять текст снизу.</summary>
        public static VAlignAttr Bottom { get { return new VAlignAttr(VAlignAttrValue.Bottom); } }
    }
}
