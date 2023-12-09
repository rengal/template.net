using System.Collections.Generic;
using System.Linq;

namespace Resto.Framework.Common.Print.Tags
{
    public static class TagSets
    {
        private static Dictionary<string, ITag> tagsDictionary;

        private static readonly ITag[] fullSet =
        {
            TagAlign.Center,
            TagAlign.Justify,
            TagAlign.Left,
            TagAlign.Right,
            TagBr.Instance,
            TagDoc.Instance,
            TagFill.Fill,
            TagNp.Instance,
            TagFont.F0,
            TagFont.F1,
            TagFont.F2,
            TagFormatter.Cut,
            TagFormatter.Split,
            TagFormatter.Wrap,
            TagTable.Instance,
            TagBarCode.Instance,
            TagPagecut.Instance,
            TagPulse.Instance,
            TagLogo.Instance,
            TagQRCode.Instance,
            TagImage.Instance,
        };

        public static Dictionary<string, ITag> Full
        {
            get { return tagsDictionary ?? (tagsDictionary = fullSet.ToDictionary(tag => tag.Name)); }
        }
    }
}