using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Шрифт: логотип.</summary>
    public class Logo : XElement
    {
        public Logo(string content) : base(TagLogo.Instance.Name, content) { }
    }
}
