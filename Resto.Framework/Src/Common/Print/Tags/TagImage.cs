using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    /// <summary>
    /// Тэг, обрабатывающий изображение
    /// </summary>
    /// <remarks>
    /// Xml-тэг image может содержать внутри себя только данные для передачи в порт и печати точечного изображения.
    /// </remarks>
    public sealed class TagImage : TagBase
    {
        internal const string TagName = "image";
        public static readonly TagImage Instance;

        static TagImage()
        {
            Instance = new TagImage();
        }

        private TagImage() : base(TagName) { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            if (node.ChildNodes.Count != 1 || node.ChildNodes[0].NodeType != XmlNodeType.Text)
                return;

            var imageFont = tape.Fonts.Image;
            if (imageFont?.ConvertToImage == null)
                return;
            var oldFont = tape.CurrentFont;
            tape.SetFont(imageFont);

            var align = node.Attributes?["align"]?.Value;
            var resizeMode = node.Attributes?["resizeMode"]?.Value;

            var xElement = new XElement(Instance.Name, node.ChildNodes[0].Value);
            if (!string.IsNullOrWhiteSpace(align))
                xElement.SetAttributeValue("align", align);
            if (!string.IsNullOrWhiteSpace(resizeMode))
                xElement.SetAttributeValue("resizeMode", resizeMode);
            tape.AppendLineWithoutFormatting(xElement, imageFont.ConvertToImage(node.ChildNodes[0].Value, align, resizeMode));

            tape.SetFont(oldFont);
        }
    }
}
