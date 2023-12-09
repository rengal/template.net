using System;
using System.Diagnostics;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal abstract class BaseNullableAtomSerializer<T> : BaseAtomSerializer<T> where T : class
    {
        protected sealed override void WriteElement([NotNull] XmlWriter writer, [NotNull] Type type, [CanBeNull] T value)
        {
            Debug.Assert(writer != null);
            Debug.Assert(type != null);

            if (value == null)
            {
                WriteNullValue(writer);
            }
            else
            {
                if (type != typeof(T))
                    WriteTypeAttribute(writer, typeof(T));

                WriteElementContent(writer, value);
            }
        }

        protected sealed override void WriteElementContent([NotNull] XmlWriter writer, [NotNull] T value)
        {
            Debug.Assert(writer != null);
            Debug.Assert(value != null);

            writer.WriteString(ToString(value, false));
        }

        [CanBeNull]
        protected sealed override T ReadElement([NotNull] XmlReader reader)
        {
            Debug.Assert(reader != null);

            return IsNullValue(reader) ? null : ReadElementContent(reader);
        }
    }
}