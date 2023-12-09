using System;
using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.Tags.Xml;
using Resto.Framework.Common.Print.TextBlockFormatters;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagFormatter : TagBase
    {
        public readonly static TagFormatter Cut;
        public readonly static TagFormatter Split;
        public readonly static TagFormatter Wrap;

        private readonly Func<ITextBlockFormatter> GetFormatter;

        static TagFormatter()
        {
            Cut = new TagFormatter(FormatterAttrValue.Cut, () => new CutFormatter());
            Split = new TagFormatter(FormatterAttrValue.Split, () => new SplitFormatter());
            Wrap = new TagFormatter(FormatterAttrValue.Wrap, () => new WrapFormatter());
        }

        private TagFormatter(FormatterAttrValue value, Func<ITextBlockFormatter> getFormatter)
            : base(value.GetName())
        {
            GetFormatter = getFormatter;
        }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            var oldFormatter = tape.TextBlockFormatter;
            var formatter = GetFormatter();
            tape.TextBlockFormatter = formatter;
            base.Format(tape, node, tags);
            tape.TextBlockFormatter = oldFormatter;
        }
    }
}
