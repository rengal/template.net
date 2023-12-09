using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagNp : TagBase
    {
        public static readonly TagNp Instance;

        static TagNp()
        {
            Instance = new TagNp();
        }

        private TagNp() : base("np") { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            tape.NewParagraph();
        }
    }
}
