using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Шрифт: первый, средний.</summary>
    public class F1 : XElement
    {
        public F1(object content) : base(TagFont.F1.Name, content) { }

        public F1(params object[] content) : base(TagFont.F1.Name, content) { }
    }
}
