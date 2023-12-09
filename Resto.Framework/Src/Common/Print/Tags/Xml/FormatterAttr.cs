using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    public class FormatterAttr : XAttribute
    {
        private FormatterAttr(FormatterAttrValue value) : base(TagTable.FormatterAttribute, value.GetName()) { }

        public static FormatterAttr Cut { get { return new FormatterAttr(FormatterAttrValue.Cut); } }
        public static FormatterAttr Split { get { return new FormatterAttr(FormatterAttrValue.Split); } }
        public static FormatterAttr Wrap { get { return new FormatterAttr(FormatterAttrValue.Wrap); } }
    }
}
