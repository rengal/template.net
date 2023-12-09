using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Шрифт: QR-code.</summary>
    public class QRCode : XElement
    {
        public QRCode(string content, params XAttribute[] attributes) : base(TagQRCode.Instance.Name, content, attributes) { }
    }
}
