using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Корень чека.</summary>
    public class Doc : XElement
    {
        public Doc(object content) : base(TagDoc.Instance.Name, content) { }
        public Doc(params object[] content) : base(TagDoc.Instance.Name, content) { }
    }
}
