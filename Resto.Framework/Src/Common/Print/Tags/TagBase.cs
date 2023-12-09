using System.Collections.Generic;
using System.Xml;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    public abstract class TagBase : ITag
    {
        private readonly string tagName;
        public string Name
        {
            get { return tagName; }
        }

        protected TagBase (string name)
        {
            tagName = name;
        }

        public virtual void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    tags[childNode.Name].Format(tape, childNode, tags);
                }
                else
                {
                    tape.AppendTextBlock(tape.CurrentFont != null && tape.CurrentFont.ReplaceUnsupportedChars != null
                                             ? tape.CurrentFont.ReplaceUnsupportedChars(childNode.Value)
                                             : childNode.Value);
                }
            }
        }
    }
}