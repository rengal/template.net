using System;
using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class TimeSpanSerializer : BasePrimitiveValueTypeSerializer<TimeSpan>
    {
        [Pure]
        protected override TimeSpan DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return TimeSpan.Parse(text);
        }
    }
}