using System;
using System.Diagnostics;
using System.Globalization;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal static class FormatHelper
    {
        internal static readonly NumberFormatInfo NumberFormatInfo;

        static FormatHelper()
        {
            var ci = CultureInfo.InstalledUICulture;
            NumberFormatInfo = (NumberFormatInfo)ci.NumberFormat.Clone();
            NumberFormatInfo.NumberDecimalSeparator = ".";
        }
    }

    internal sealed class DoubleSerializer : BasePrimitiveValueTypeSerializer<double>
    {
        [NotNull]
        [Pure]
        protected override string ToStringCore(double value, bool _)
        {
            return Convert.ToString(value, FormatHelper.NumberFormatInfo);
        }

        [Pure]
        protected override double DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return double.Parse(text, FormatHelper.NumberFormatInfo);
        }
    }

    internal sealed class DecimalSerializer : BasePrimitiveValueTypeSerializer<decimal>
    {
        [NotNull]
        [Pure]
        protected override string ToStringCore(decimal value, bool _)
        {
            return Convert.ToString(value, FormatHelper.NumberFormatInfo);
        }

        [Pure]
        protected override decimal DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return new decimal(double.Parse(text, FormatHelper.NumberFormatInfo));
        }
    }

    internal sealed class ByteSerializer : BasePrimitiveValueTypeSerializer<Byte>
    {
        [Pure]
        protected override byte DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return Byte.Parse(text, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }
    }

    internal sealed class FloatSerializer : BasePrimitiveValueTypeSerializer<float>
    {
        [NotNull]
        [Pure]
        protected override string ToStringCore(float value, bool _)
        {
            return Convert.ToString(value, FormatHelper.NumberFormatInfo);
        }

        [Pure]
        protected override float DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return float.Parse(text, FormatHelper.NumberFormatInfo);
        }
    }

    internal sealed class IntSerializer : BasePrimitiveValueTypeSerializer<int>
    {
        [Pure]
        protected override int DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return int.Parse(text, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }
    }

    internal sealed class LongSerializer : BasePrimitiveValueTypeSerializer<long>
    {
        [Pure]
        protected override long DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            return Int64.Parse(text, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }
    }
}