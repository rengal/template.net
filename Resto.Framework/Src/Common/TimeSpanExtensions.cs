using System;
using System.Collections.Generic;
using System.Globalization;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Properties;

// ReSharper disable CheckNamespace
namespace Resto.Framework.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Расширение класс <see cref="TimeSpan"/>
    /// </summary>
    public static class TimeSpanExtensions
    {
        public static TimeSpan TruncateToDays(this TimeSpan timeSpan)
        {
            return new TimeSpan(timeSpan.Days, 0, 0, 0);
        }

        public static TimeSpan TruncateToHours(this TimeSpan timeSpan)
        {
            return new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0);
        }

        public static TimeSpan TruncateToMinutes(this TimeSpan timeSpan)
        {
            return new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0);
        }

        public static TimeSpan TruncateToSeconds(this TimeSpan timeSpan)
        {
            return new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public static TimeSpan RoundToDays(this TimeSpan timeSpan)
        {
            return TimeSpan.FromDays(Math.Round(timeSpan.TotalDays, MidpointRounding.AwayFromZero));
        }

        public static TimeSpan RoundToHours(this TimeSpan timeSpan)
        {
            return TimeSpan.FromHours(Math.Round(timeSpan.TotalHours, MidpointRounding.AwayFromZero));
        }

        public static TimeSpan RoundToMinutes(this TimeSpan timeSpan)
        {
            return TimeSpan.FromMinutes(Math.Round(timeSpan.TotalMinutes, MidpointRounding.AwayFromZero));
        }

        public static TimeSpan RoundToSeconds(this TimeSpan timeSpan)
        {
            return TimeSpan.FromSeconds(Math.Round(timeSpan.TotalSeconds, MidpointRounding.AwayFromZero));
        }

        public static TimeSpan CeilToDays(this TimeSpan timeSpan)
        {
            return TimeSpan.FromDays(Math.Ceiling(timeSpan.TotalDays));
        }

        public static TimeSpan CeilToHours(this TimeSpan timeSpan)
        {
            return TimeSpan.FromHours(Math.Ceiling(timeSpan.TotalHours));
        }

        public static TimeSpan CeilToMinutes(this TimeSpan timeSpan)
        {
            return TimeSpan.FromMinutes(Math.Ceiling(timeSpan.TotalMinutes));
        }

        public static TimeSpan CeilToSeconds(this TimeSpan timeSpan)
        {
            return TimeSpan.FromSeconds(Math.Ceiling(timeSpan.TotalSeconds));
        }

        public static TimeSpan Multiply(this TimeSpan timeSpan, int multiplier)
        {
            return TimeSpan.FromTicks(timeSpan.Ticks * multiplier);
        }

        /// <summary>
        /// Представление интервала времени в виде строки "X г. Y мес Z д."
        /// </summary>
        /// <param name="value">Интервал времени</param>
        /// <param name="forceShowSeconds">true: показывать ли сколько секунд.</param>
        /// <returns>Строка вида "1 г. 3 мес. 2 нед. 3 д. 4 ч.", если value больше 1 дня, и "4 ч. 5 мин.", если value меньше дня.</returns>
        public static string ToFriendlyText(this TimeSpan value, bool forceShowSeconds = false)
        {
            const int daysInYear = 365;
            const int daysInMonth = 30;

            var values = new List<string>();

            // Количество лет
            var days = value.Days;
            if (days >= daysInYear)
            {
                var years = (days / daysInYear);
                values.Add(years + " " + Resources.YearShortText);
                days = (days % daysInYear);
            }

            // Количество месяцев
            if (days >= daysInMonth)
            {
                var months = (days / daysInMonth);
                values.Add(months + " " + Resources.MonthShortText);
                days = (days % daysInMonth);
            }

            // Количество недель
            var weeks = days / 7;
            if (weeks > 0)
            {
                values.Add(weeks + " " + Resources.WeekShortText);
                days -= weeks * 7;
            }

            // Количество дней
            if (days >= 1)
                values.Add(days + " " + Resources.DayShortText);

            // Количество часов
            if (value.Hours >= 1)
                values.Add(value.Hours + " " + Resources.HourShortText);

            // Количество минут
            if (days < 1 && value.Minutes >= 1)
                values.Add(value.Minutes + " " + Resources.MinuteShortText);

            // Количество секунд
            if (forceShowSeconds && days < 1 && (value.Seconds >= 1 || values.Count == 0))
                values.Add(value.Seconds + " " + Resources.SecondShortText);

            return string.Join(" ", values);
        }

        /// <summary>
        /// Возвращает строковое представление <see cref="TimeSpan"/> в соответствии с заданными настройками культуры.
        /// </summary>
        public static string TimeSpanToString(this TimeSpan value, [NotNull] CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            var hoursStr = ((int) value.TotalHours).ToString("0");
            return hoursStr + culture.DateTimeFormat.TimeSeparator + value.Minutes.ToString("00");
        }
    }
}