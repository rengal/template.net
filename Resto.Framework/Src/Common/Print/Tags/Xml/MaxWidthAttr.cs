using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Максимальная ширина столбца таблицы, если задан атрибут <see cref="AutoWidthAttr"/>.</summary>
    public class MaxWidthAttr : XAttribute
    {
        public MaxWidthAttr(int value) : base(TagTable.MaxwidthAttr, value) { }
    }
}
