using System;
using System.Diagnostics;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    public abstract class BasePrimitiveValueTypeSerializer<T> : BaseAtomSerializer<T> where T : struct 
    {
        protected sealed override void WriteElement([NotNull] XmlWriter writer, [NotNull] Type type, T value)
        {
            Debug.Assert(writer != null);
            Debug.Assert(type != null);


            if (type != typeof(T))
                WriteTypeAttribute(writer, typeof(T));

            WriteElementContent(writer, value);
        }

        protected sealed override void WriteElementContent([NotNull] XmlWriter writer, T value)
        {
            Debug.Assert(writer != null);

            writer.WriteString(ToString(value, false));
        }

        protected sealed override T ReadElement([NotNull] XmlReader reader)
        {
            Debug.Assert(reader != null);

            return IsNullValue(reader) ? default(T) : ReadElementContent(reader);
        }

        [NotNull]
        [Pure]
        protected sealed override string ToString(T value, bool forDebug)
        {
            return ToStringCore(value, forDebug);
        }

        [NotNull]
        [Pure]
        protected virtual string ToStringCore(T value, bool forDebug)
        {
            return value.ToString();
        }
    }
}