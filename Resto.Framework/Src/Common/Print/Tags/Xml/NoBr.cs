using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Символ неразрывного пробела.</summary>
    public class NoBr : XElement
    {
        public NoBr() : base(TagNobr.Instance.Name) { }
    }
}
