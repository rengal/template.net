using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class BooleanSerializer : BasePrimitiveValueTypeSerializer<bool>
    {
        [NotNull]
        [Pure]
        protected override string ToStringCore(bool value, bool _)
        {
            return value ? "true" : "false";
        }

        [Pure]
        protected override bool DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return bool.Parse(text);
        }
    }
}