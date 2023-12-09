using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Разбить текст на строки, перенося на пробельных символах (пробел, табуляция и др.).</summary>
    public class Split : XElement
    {
        public Split(object content)
            : base(TagFormatter.Split.Name, content)
        {
        }

        public Split(params object[] content)
            : base(TagFormatter.Split.Name, content)
        {
        }
    }
}
