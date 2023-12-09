using System.Globalization;
using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Ширина столбца таблицы.</summary>
    public sealed class WidthAttr : XAttribute
    {
        public WidthAttr(int value) : base(TagTable.WidthAttribute, value.ToString(CultureInfo.InvariantCulture)) { }
    }
}
