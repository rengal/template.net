using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Минимальная ширина столбца таблицы, если задан атрибут <see cref="AutoWidthAttr"/>.</summary>
    public class MinWidthAttr : XAttribute
    {
        public MinWidthAttr(int value) : base(TagTable.MinwidthAttr, value) { }
    }
}
