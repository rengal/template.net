using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    public class LeftPair : XElement
    {
        public LeftPair(string leftContent, string rightContent)
            : base("pair",
            new XAttribute("left", leftContent ?? string.Empty),
            new XAttribute("right", rightContent ?? string.Empty)) { }
    }
}
