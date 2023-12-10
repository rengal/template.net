using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;
using Resto.Framework.Common;

namespace Resto.Common
{
    public static class CommonExtensions
    {
        public static int ToJavaDate(this DayOfWeek day)
        {
            return DateTimeUtils.GetDayOfWeekNumber(day);
        }

        [CanBeNull]
        public static RGBColor ToRGBColor(this Color color)
        {
            if (color == Color.Transparent)
                return null;
            return new RGBColor(color.R, color.G, color.B);
        }

        public static Color ToWindowsColor([CanBeNull] this RGBColor color)
        {
            if (color == null)
                return Color.Transparent;
            return GetKnownColor(Color.FromArgb(color.Red, color.Green, color.Blue));
        }

        private static Color GetKnownColor(Color color)
        {
            return
                (from KnownColor knownColor in Enum.GetValues(typeof(KnownColor))
                 select Color.FromKnownColor(knownColor) into namedColor
                 where !namedColor.IsSystemColor && namedColor.ToArgb() == color.ToArgb()
                 select namedColor)
                 .ContinueWith(color)
                 .First();
        }

        public static IEnumerable<T> GetAllRecursively<T>(this IEnumerable<T> list, Func<T, IEnumerable<T>> func)
        {
            foreach (var enumerable in list)
            {
                yield return enumerable;
                foreach (var innerEnumerable in func(enumerable).GetAllRecursively(func))
                {
                    yield return innerEnumerable;
                }
            }
        }

        public static string GetDescriptionAttr<T>(this T source)
        {
            var fi = source.GetType().GetField(source.ToString());

            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

                if (attributes.Length > 0) return attributes[0].Description;
            }

            return source.ToString();
        }
    }
}