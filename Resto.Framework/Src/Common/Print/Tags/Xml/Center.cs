using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Выровнять текст по центру.</summary>
    public class Center : XElement
    {
        public Center(object content) : base(TagAlign.Center.Name, content) { }

        public Center(params object[] content) : base(TagAlign.Center.Name, content) { }
    }
}
