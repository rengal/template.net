using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Шрифт: нулевой, мелкий, используется по умолчанию.</summary>
    public class F0 : XElement
    {
        public F0(object content) : base(TagFont.F0.Name, content) { }

        public F0(params object[] content) : base(TagFont.F0.Name, content) { }
    }
}
