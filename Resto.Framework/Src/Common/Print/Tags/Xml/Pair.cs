using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    public class Pair : XElement
    {
        public Pair(string leftContent, string rightContent) : this(leftContent, rightContent, false) { }

        public Pair(string leftContent, string rightContent, bool fitLeft)
            : base("pair",
            new XAttribute("left", leftContent ?? string.Empty),
            new XAttribute("right", rightContent ?? string.Empty),
            new XAttribute("fit", fitLeft ? "left" : "right")) { }
    }
}