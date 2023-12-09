using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Вычислять ли ширину столбца таблицы автоматически по его содержимому.</summary>
    public class AutoWidthAttr : XAttribute
    {
        public AutoWidthAttr() : base(TagTable.AutowidthAttr, "") { }
    }
}
