using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Выровнять текст по левому краю.</summary>
    public class Left : XElement
    {
        public Left(object content) : base(TagAlign.Left.Name, content) { }

        public Left(params object[] content) : base(TagAlign.Left.Name, content) { }
    }
}
