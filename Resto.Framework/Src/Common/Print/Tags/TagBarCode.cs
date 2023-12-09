using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    /// <summary>
    /// Тэг, обрабатывающий штрихкод
    /// </summary>
    /// <remarks>
    /// Xml-тэг barcode может содержать внутри себя только текст.
    /// </remarks>
    public sealed class TagBarCode : TagBase
    {
        internal const string TagName = "barcode";
        public static readonly TagBarCode Instance;

        static TagBarCode()
        {
            Instance = new TagBarCode();
        }

        private TagBarCode() : base(TagName) { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            if (node.ChildNodes.Count != 1 || node.ChildNodes[0].NodeType != XmlNodeType.Text)
                return;

            var barCodeFont = tape.Fonts.BarCode;
            if (barCodeFont is not { IsSupported: true } || barCodeFont.ConvertToBarcode == null) 
                return;
            var oldFont = tape.CurrentFont;
            tape.SetFont(barCodeFont);

            var align = node.Attributes?["align"]?.Value;
            var heightRatio = node.Attributes?["heightRatio"]?.Value;
            var hri = node.Attributes?["hri"]?.Value;

            var xElement = new XElement(Instance.Name, node.ChildNodes[0].Value);
            if (!string.IsNullOrWhiteSpace(align))
                xElement.SetAttributeValue("align", align);
            if (!string.IsNullOrWhiteSpace(heightRatio))
                xElement.SetAttributeValue("heightRatio", heightRatio);
            if (!string.IsNullOrWhiteSpace(hri))
                xElement.SetAttributeValue("hri", hri);

            tape.AppendLineWithoutFormatting(xElement, barCodeFont.ConvertToBarcode(node.ChildNodes[0].Value, align, heightRatio, hri));

            tape.SetFont(oldFont);
        }
    }
}
