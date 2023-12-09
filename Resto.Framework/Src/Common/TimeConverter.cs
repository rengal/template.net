using System;
using Resto.Framework.Properties;

namespace Resto.Framework.Common
{
    public static class TimeConverter
    {
        public static string TimeSpanFormat(TimeSpan time, bool withSuffix = true)
        {
            var result = string.Empty;
            if (time.Days != 0)
            {
                result += string.Format(GetReducedTimeSpanFormat(Resources.ReducedDay, withSuffix), time.Days);
            }
            if (time.Hours != 0)
            {
                result += string.Format(GetReducedTimeSpanFormat(Resources.ReducedHour, withSuffix), time.Hours);
            }
            if (time.Minutes != 0)
            {
                result += string.Format(GetReducedTimeSpanFormat(Resources.ReducedMinute, withSuffix), time.Minutes);
            }

            return !result.IsNullOrEmpty()
                ? result.Trim()
                : string.Format(GetReducedTimeSpanFormat(Resources.ReducedMinute, withSuffix), 0);
        }

        private static string GetReducedTimeSpanFormat(string reducedTimeSpan, bool withSuffix)
        {
            return withSuffix ? string.Format("{0}{1}", reducedTimeSpan, Resources.TimeSpanSuffix) : reducedTimeSpan;
        }
    }
}
