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
    /// Xml-тэг logo может содержать внутри себя только число.
    /// </remarks>
    public sealed class TagLogo : TagBase
    {
        internal const string TagName = "logo";
        public static readonly TagLogo Instance;

        static TagLogo()
        {
            Instance = new TagLogo();
        }

        private TagLogo() : base(TagName) { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            if (node.ChildNodes.Count != 1 || node.ChildNodes[0].NodeType != XmlNodeType.Text)
                return;

            var logoFont = tape.Fonts.Logo;
            if (logoFont == null || !logoFont.IsSupported || logoFont.ConvertToLogo == null)
                return;
            var oldFont = tape.CurrentFont;
            tape.SetFont(logoFont);
            tape.AppendLineWithoutFormatting(new XElement(Instance.Name, node.ChildNodes[0].Value), logoFont.ConvertToLogo(node.ChildNodes[0].Value));

            tape.SetFont(oldFont);
        }
    }
}
