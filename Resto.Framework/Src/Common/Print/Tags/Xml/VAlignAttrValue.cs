using System.Collections.Generic;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    internal enum VAlignAttrValue
    {
        /// <summary>Выровнять текст сверху.</summary>
        Top,
        /// <summary>Выровнять текст по центру.</summary>
        Center,
        /// <summary>Выровнять текст снизу.</summary>
        Bottom,
    }

    internal static class VAlignAttrValueExtensions
    {
        private static readonly Dictionary<VAlignAttrValue, string> Names = new Dictionary<VAlignAttrValue, string> {
            { VAlignAttrValue.Top, "top" },
            { VAlignAttrValue.Center, "center" },
            { VAlignAttrValue.Bottom, "bottom" },
        };

        public static string GetName(this VAlignAttrValue self)
        {
            return Names[self];
        }
    }
}
