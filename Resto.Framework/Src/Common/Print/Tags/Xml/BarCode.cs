using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Шрифт: штрихкод.</summary>
    public class BarCode : XElement
    {
        public BarCode(string content) : base(TagBarCode.Instance.Name, content) { }
    }
}
