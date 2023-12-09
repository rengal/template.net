using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    /// TODO это где нужно? и что делает?
    public sealed class TagFLine : TagBase
    {
        private const string SYMBOLS_ATTRIBUTE = "symbols";
        
        public static readonly TagFLine Instance;

        static TagFLine()
        {
            Instance = new TagFLine();
        }

        private TagFLine() : base("fline") { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            var oldFillSymbols = tape.FillSymbols;
            tape.NewParagraph();
            tape.FillSymbols = node.Attributes[SYMBOLS_ATTRIBUTE].Value;
            tape.NewParagraph();
            tape.FillSymbols = oldFillSymbols;
        }
    }
}
