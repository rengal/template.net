using System.Collections.Generic;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    internal enum FormatterAttrValue
    {
        /// <summary>Обрезать текст, если он не помещается.</summary>
        Cut,
        /// <summary>Разбить текст на строки, перенося на пробельных символах (пробел, табуляция и др.).</summary>
        Split,
        /// <summary>Разбить текст на строки, перенося в любом месте.</summary>
        Wrap,
    }

    internal static class FormatterAttrValueExtensions
    {
        private static readonly Dictionary<FormatterAttrValue, string> Names = new Dictionary<FormatterAttrValue, string> {
            { FormatterAttrValue.Cut, "cut" },
            { FormatterAttrValue.Split, "split" },
            { FormatterAttrValue.Wrap, "wrap" },
        };

        public static string GetName(this FormatterAttrValue self)
        {
            return Names[self];
        }
    }
}
