using System;
using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class GuidSerializer : BasePrimitiveValueTypeSerializer<Guid>
    {
        [NotNull]
        [Pure]
        protected override string ToStringCore(Guid value, bool _)
        {
            return GuidGenerator.FormatGuid(value);
        }

        [Pure]
        protected override Guid DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return GuidGenerator.ParseGuid(text);
        }
    }
}