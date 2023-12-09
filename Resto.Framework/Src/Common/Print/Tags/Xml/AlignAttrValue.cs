using System.Collections.Generic;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    internal enum AlignAttrValue
    {
        /// <summary>Выровнять текст по центру.</summary>
        Center,
        /// <summary>Выровнять текст по ширине.</summary>
        Justify,
        /// <summary>Выровнять текст по левому краю.</summary>
        Left,
        /// <summary>Выровнять текст по правому краю.</summary>
        Right,
    }

    internal static class AlignAttrValueExtensions
    {
        private static readonly Dictionary<AlignAttrValue, string> Names = new Dictionary<AlignAttrValue, string> {
            { AlignAttrValue.Center, "center" },
            { AlignAttrValue.Justify, "justify" },
            { AlignAttrValue.Left, "left" },
            { AlignAttrValue.Right, "right" },
        };

        public static string GetName(this AlignAttrValue self)
        {
            return Names[self];
        }
    }
}
