using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Обрезать текст, если он не помещается.</summary>
    public class Cut : XElement
    {
        public Cut(object content) : base(TagFormatter.Cut.Name, content) { }

        public Cut(params object[] content) : base(TagFormatter.Cut.Name, content) { }
    }
}
