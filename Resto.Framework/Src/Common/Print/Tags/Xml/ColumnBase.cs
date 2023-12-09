using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    public abstract class ColumnBase : XElement
    {
        protected ColumnBase(XName name, params XAttribute[] attributes)
            : base(name, attributes)
        { }
    }
}
