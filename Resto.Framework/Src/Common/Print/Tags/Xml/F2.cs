using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Шрифт: второй, крупный.</summary>
    public class F2 : XElement
    {
        public F2(object content) : base(TagFont.F2.Name, content) { }

        public F2(params object[] content) : base(TagFont.F2.Name, content) { }
    }
}
