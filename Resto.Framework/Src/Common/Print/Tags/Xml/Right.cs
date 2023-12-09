using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Выровнять текст по правому краю.</summary>
    public class Right : XElement
    {
        public Right(object content) : base(TagAlign.Right.Name, content) { }

        public Right(params object[] content) : base(TagAlign.Right.Name, content) { }
    }
}
