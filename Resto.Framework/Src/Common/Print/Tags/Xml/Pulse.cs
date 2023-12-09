using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>
    /// Тэг для подачи импульса
    /// </summary>
    public class Pulse : XElement
    {
        public Pulse()
            : base(TagPulse.Instance.Name)
        {
        }
    }
}