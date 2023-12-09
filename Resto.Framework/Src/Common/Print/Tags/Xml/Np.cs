using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Перенос на новую строку.</summary>
    public class Np : XElement
    {
        public Np() : base(TagNp.Instance.Name) { }
    }
}
