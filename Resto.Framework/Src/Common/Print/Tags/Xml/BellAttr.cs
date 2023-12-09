using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Вставляет при печати чека специальный символ — звонок на кухню.</summary>
    public class BellAttr : XAttribute
    {
        public BellAttr() : base(TagUtil.Bell.Name, "") { }
    }
}
