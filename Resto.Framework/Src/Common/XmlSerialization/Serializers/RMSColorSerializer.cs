using System.Diagnostics;
using System.Drawing;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class RMSColorSerializer : BasePrimitiveValueTypeSerializer<Color>
    {
        [NotNull]
        [Pure]
        protected override string ToStringCore(Color value, bool _)
        {
            return value.ToArgb().ToString();
        }

        [Pure]
        protected override Color DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return Color.FromArgb(int.Parse(text));
        }
    }
}