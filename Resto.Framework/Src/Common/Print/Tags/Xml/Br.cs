using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Перенос на новую строку.</summary>
    public class Br : XElement
    {
        public Br() : base(TagBr.Instance.Name) { }
    }
}
