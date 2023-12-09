using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagPagecut : TagBase
    {
        public static readonly TagPagecut Instance;

        static TagPagecut()
        {
            Instance = new TagPagecut();
        }

        private TagPagecut() 
            : base("pagecut")
        {}

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            tape.Pagecut();
        }
    }
}