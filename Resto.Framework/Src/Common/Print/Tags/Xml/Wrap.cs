using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Разбить текст на строки, перенося в любом месте.</summary>
    public class Wrap : XElement
    {
        public Wrap(object content) : base(TagFormatter.Wrap.Name, content) { }

        public Wrap(params object[] content) : base(TagFormatter.Wrap.Name, content) { }
    }
}
