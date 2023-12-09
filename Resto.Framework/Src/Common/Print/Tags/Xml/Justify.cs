using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Выровнять текст по ширине.</summary>
    public class Justify : XElement
    {
        public Justify(object content) : base(TagAlign.Justify.Name, content) { }

        public Justify(params object[] content) : base(TagAlign.Justify.Name, content) { }
    }
}
