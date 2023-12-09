using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    /// <summary>
    /// Тэг, обрабатывающий логотип
    /// </summary>
    /// <remarks>
    /// Xml-тэг QRCode содержит текст, может содержать атрибуты.
    /// </remarks>
    public sealed class TagQRCode : TagBase
    {
        internal const string TagName = "qrcode";
        internal const string AttributeAlign = "align";
        internal const string AttributeSize = "size";
        internal const string AttributeCorrection = "correction";
        public static readonly TagQRCode Instance;

        static TagQRCode()
        {
            Instance = new TagQRCode();
        }

        private TagQRCode() : base(TagName) { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            if (node.ChildNodes.Count != 1 || node.ChildNodes[0].NodeType != XmlNodeType.Text)
                return;

            var qrCodeFont = tape.Fonts.QRCode;
            if (qrCodeFont == null || !qrCodeFont.IsSupported || qrCodeFont.ConvertToQRCode == null) 
                return;
            var oldFont = tape.CurrentFont;
            tape.SetFont(qrCodeFont);
            var align = node.Attributes != null && node.Attributes[AttributeAlign] != null ? node.Attributes[AttributeAlign].Value : null;
            var size = node.Attributes != null && node.Attributes[AttributeSize] != null ? node.Attributes[AttributeSize].Value : null;
            var correction = node.Attributes != null && node.Attributes[AttributeCorrection] != null ? node.Attributes[AttributeCorrection].Value : null;
            var element = new XElement(Instance.Name, node.ChildNodes[0].Value);
            if (node.Attributes != null)
            {
                if (node.Attributes[AttributeAlign] != null && !string.IsNullOrEmpty(node.Attributes[AttributeAlign].Value))
                    element.SetAttributeValue(AttributeAlign, node.Attributes[AttributeAlign].Value);
                if (node.Attributes[AttributeSize] != null && !string.IsNullOrEmpty(node.Attributes[AttributeSize].Value))
                    element.SetAttributeValue(AttributeSize, node.Attributes[AttributeSize].Value);
                if (node.Attributes[AttributeCorrection] != null && !string.IsNullOrEmpty(node.Attributes[AttributeCorrection].Value))
                    element.SetAttributeValue(AttributeCorrection, node.Attributes[AttributeCorrection].Value);
            }
            tape.AppendLineWithoutFormatting(element, qrCodeFont.ConvertToQRCode(node.ChildNodes[0].Value, align, size, correction));

            tape.SetFont(oldFont);
        }
    }
}
