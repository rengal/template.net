using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Горизонтальное выравнивание текста в ячейке или столбце таблицы.</summary>
    public sealed class AlignAttr : XAttribute
    {
        private AlignAttr(AlignAttrValue value) : base(TagTable.AlignAttribute, value.GetName()) { }

        /// <summary>Выровнять текст по центру.</summary>
        public static AlignAttr Center { get { return new AlignAttr(AlignAttrValue.Center); } }
        /// <summary>Выровнять текст по ширине.</summary>
        public static AlignAttr Justify { get { return new AlignAttr(AlignAttrValue.Justify); } }
        /// <summary>Выровнять текст по левому краю.</summary>
        public static AlignAttr Left { get { return new AlignAttr(AlignAttrValue.Left); } }
        /// <summary>Выровнять текст по правому краю.</summary>
        public static AlignAttr Right { get { return new AlignAttr(AlignAttrValue.Right); } }
    }
}
