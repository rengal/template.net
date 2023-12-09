using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class StringSerializer : BaseNullableAtomSerializer<string>
    {
        [NotNull]
        [Pure]
        protected override string ToString([NotNull] string value, bool _)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return value;
        }

        [NotNull]
        [Pure]
        protected override string DeserializeFromString([NotNull] string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return text;
        }
    }
}