using System.Collections.Generic;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    internal enum FontAttrValue
    {
        /// <summary>Шрифт: нулевой, мелкий, используется по умолчанию.</summary>
        F0,
        /// <summary>Шрифт: первый, средний.</summary>
        F1,
        /// <summary>Шрифт: второй, крупный.</summary>
        F2
    }

    internal static class FontAttrValueExtensions
    {
        private static readonly Dictionary<FontAttrValue, string> Names = new Dictionary<FontAttrValue, string> {
            { FontAttrValue.F0, "f0" },
            { FontAttrValue.F1, "f1" },
            { FontAttrValue.F2, "f2" }
        };

        public static string GetName(this FontAttrValue self)
        {
            return Names[self];
        }
    }
}
