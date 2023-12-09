using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    public class LineCell : Cell
    {
        public LineCell(string symbols) : base("linecell", new XAttribute(TagFill.SYMBOLS_ATTRIBUTE, symbols)) { }
    }
}
