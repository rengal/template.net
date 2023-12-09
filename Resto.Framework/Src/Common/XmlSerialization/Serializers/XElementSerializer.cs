using System;
using System.Xml.Linq;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class XElementSerializer : BaseNullableAtomSerializer<XElement>
    {
        [NotNull]
        [Pure]
        protected override string ToString(XElement value, bool _)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return value.ToString();
        }

        [NotNull]
        [Pure]
        protected override XElement DeserializeFromString(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return XElement.Parse(text);
        }
    }
}