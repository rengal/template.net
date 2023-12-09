using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagNobr : TagBase
    {
        public static readonly TagNobr Instance;

        static TagNobr()
        {
            Instance = new TagNobr();
        }

        private TagNobr() : base("nobr") { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            tape.AppendTextBlock("\u00A0");
        }
    }
}
