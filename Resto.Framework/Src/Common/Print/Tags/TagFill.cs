using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagFill : TagBase
    {
        internal const string SYMBOLS_ATTRIBUTE = "symbols";

        public static readonly TagFill Fill = new TagFill("fill");

        private TagFill(string name) : base(name) { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            var oldFillSymbols = tape.FillSymbols;
            tape.FillSymbols = node.Attributes[SYMBOLS_ATTRIBUTE].Value;
            base.Format(tape, node, tags);
            tape.FillSymbols = oldFillSymbols;
        }
    }
}
