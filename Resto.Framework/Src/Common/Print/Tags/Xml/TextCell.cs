using System;
using System.Linq;
using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Ячейка с возможностью измерения ширины содержимого. Может содержать только строки.</summary>
    public class TextCell : Cell
    {
        public TextCell(string text, params XAttribute[] attributes) : base(TagTable.TextCellTag, text, attributes)
        {
            if (attributes.OfType<ColSpanAttr>().Any())
                throw new NotSupportedException("ColSpan on TextCell not supported");
        }
    }
}
