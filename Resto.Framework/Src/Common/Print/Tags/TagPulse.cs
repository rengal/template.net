using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Tags
{
    /// <summary>
    /// Тэг, обрабатывающий команду подачи импульса на выход принтера
    /// </summary>
    public sealed class TagPulse : TagBase
    {
        internal const string TagName = "pulse";
        public static readonly TagPulse Instance;

        static TagPulse()
        {
            Instance = new TagPulse();
        }

        private TagPulse() : base(TagName) { }

        public override void Format(ITape tape, XmlNode node, Dictionary<string, ITag> tags)
        {
            var pulseFont = tape.Fonts.Pulse;
            if (pulseFont == null || pulseFont.ConvertToPulse == null)
                return;
            var oldFont = tape.CurrentFont;
            tape.SetFont(pulseFont);
            tape.AppendLine(new XElement(Instance.Name), string.Empty, pulseFont.ConvertToPulse());

            tape.SetFont(oldFont);
        }
    }
}
