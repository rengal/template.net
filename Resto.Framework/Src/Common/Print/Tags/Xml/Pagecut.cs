using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>
    /// Разрез бумаги
    /// </summary>
    public class Pagecut : XElement
    {
        public Pagecut() : base(TagPagecut.Instance.Name)
        {}
    }
}
