using System;
using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.Alignment;
using Resto.Framework.Common.Print.TextBlockFormatters;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagDoc : TagBase
    {
        internal const string LINE_TAG = "line";
        private const string ALIGN_ATTRIBUTE = "align";
        private const string FONT_ATTRIBUTE = "font";
        private const string FORMATTER_ATTRIBUTE = "formatter";
        private const string BELL_ATTRIBUTE = "bell";

        public static readonly TagDoc Instance;
        private static readonly ActionDictionary Actions;

        static TagDoc()
        {
            Instance = new TagDoc();
            Actions = new ActionDictionary
            {
                {ALIGN_ATTRIBUTE, TagAlign.Center.Name, tape => tape.SetAlign(AlignCenter.Instance)},              
                {ALIGN_ATTRIBUTE, TagAlign.Justify.Name, tape => tape.SetAlign(AlignJustify.Instance)},
                {ALIGN_ATTRIBUTE, TagAlign.Left.Name, tape => tape.SetAlign(AlignLeft.Instance)},
                {ALIGN_ATTRIBUTE, TagAlign.Right.Name, tape => tape.SetAlign(AlignRight.Instance)},
                {FONT_ATTRIBUTE, TagFont.F0.Name, tape => tape.SetFont(tape.Fonts.Font0)},
                {FONT_ATTRIBUTE, TagFont.F1.Name, tape => tape.SetFont(tape.Fonts.Font1)},
                {FONT_ATTRIBUTE, TagFont.F2.Name, tape => tape.SetFont(tape.Fonts.Font2)},
                {FORMATTER_ATTRIBUTE, TagFormatter.Cut.Name, tape => tape.TextBlockFormatter = new CutFormatter() },
                {FORMATTER_ATTRIBUTE, TagFormatter.Split.Name, tape => tape.TextBlockFormatter = new SplitFormatter() },
                {FORMATTER_ATTRIBUTE, TagFormatter.Wrap.Name, tape => tape.TextBlockFormatter = new WrapFormatter() },
                {BELL_ATTRIBUTE, string.Empty, tape => {}}
            };
        }

        private TagDoc() : base("doc") { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                Actions[attr.Name, attr.Value](tape);
            }
            base.Format(tape, node, tags);
        }

        private sealed class ActionDictionary : Dictionary<string, Dictionary<string, Action<ITape>>>
        {
            public void Add(string name, string value, Action<ITape> action)
            {
                if (!ContainsKey(name))
                {
                    Add(name, new Dictionary<string, Action<ITape>>());
                }
                this[name].Add(value, action);
            }

            public Action<ITape> this[string name, string value]
            {
                get { return this[name][value]; }
            }
        }
    }
}
