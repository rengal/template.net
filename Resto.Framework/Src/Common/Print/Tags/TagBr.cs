using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagBr : TagBase
    {
        public static readonly TagBr Instance;

        static TagBr()
        {
            Instance = new TagBr();
        }

        private TagBr() : base("br") { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            tape.NewLine();
        }
    }
}
