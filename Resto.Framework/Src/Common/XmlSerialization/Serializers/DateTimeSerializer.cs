using System;
using System.Diagnostics;
using System.Globalization;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.Serializers
{
    internal sealed class DateTimeSerializer : BasePrimitiveValueTypeSerializer<DateTime>
    {
        private static readonly DateTimeFormatInfo WriteFormatInfo = new DateTimeFormatInfo { FullDateTimePattern = "yyyy-MM-dd'T'HH:mm:ss.fffzzz" };
        private static readonly DateTimeFormatInfo ReadFormatInfo = new DateTimeFormatInfo { FullDateTimePattern = "yyyy-MM-dd'T'HH:mm:ss.fff", ShortDatePattern = "yyyy-MM-dd" };

        [NotNull]
        [Pure]
        protected override string ToStringCore(DateTime value, bool forDebug)
        {
            return forDebug ? value.ToString(CultureInfo.CurrentCulture) : value.ToString("F", WriteFormatInfo);
        }

        [Pure]
        protected override DateTime DeserializeFromString([NotNull] string text)
        {
            Debug.Assert(text != null);

            // отрезаем временную зону
            if (text.Length == 29)
            {
                text = text.Substring(0, text.Length - 6);
            }

            Debug.Assert(text.Length == 10 || text.Length == 23,
                "Cannot parse local date", "Wrong length of date {0}: {1} instead of 10 or 23", text, text.Length);

            return DateTime.Parse(text, ReadFormatInfo, DateTimeStyles.AssumeLocal);
        }
    }
}