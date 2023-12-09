using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Шрифт: картинка.</summary>
    class Image : XElement
    {
        public Image(string content) : base(TagImage.Instance.Name, content) { }
    }
}
