using System;
using System.Globalization;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Common
{
    /// <summary>
    /// Extension-методы для <see cref="DateTime"/>
    /// </summary>
    /// <seealso cref="DateTimeUtils"/>
    public static class DateTimeExtentions
    {
        /// <summary>
        /// Начало отсчёта для UNIX-time
        /// </summary>
        private static readonly DateTime unixBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Отбрасывает время у переданного <paramref name="dt"/>
        /// </summary>
        public static DateTime TruncateTime(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        /// <summary>
        /// Приведение к типу <see cref="DateInfo"/>.
        /// </summary>
        public static DateInfo ToDateInfo(this DateTime dt)
        {
            return new DateInfo(dt);
        }

        /// <summary>
        /// Сменить время у даты.
        /// </summary>
        [Pure]
        public static DateTime ChangeTime(this DateTime dt, TimeSpan time)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, time.Hours, time.Minutes, time.Seconds);
        }

        /// <summary>
        /// Возвращает true, если дата равна <see cref="DateTimeServerConstants.MAX_DATE"/>.
        /// </summary>
        public static bool IsServerMaxDate(this DateTime dt)
        {
            return dt == DateTimeServerConstants.MAX_DATE.DateTime;
        }

        /// <summary>
        /// Вычисление разницы между датами в месяцах
        /// </summary>
        public static int MonthDifference(this DateTime date1, DateTime date2)
        {
            return Math.Abs((date1.Month - date2.Month) + 12 * (date1.Year - date2.Year));
        }

        /// <summary>
        /// Вычисление разницы между датами в днях
        /// </summary>
        public static int DayDifference(this DateTime date1, DateTime date2)
        {
            return Convert.ToInt32((date1.Date - date2.Date).RoundToDays().TotalDays);
        }

        /// <summary>
        /// Проверяет, входит ли дата в заданный интервал
        /// </summary>
        public static bool InRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }

        /// <summary>
        /// Преобразовать DateTime в Unix-based формат (кол-во секунд с 1970.01.01)
        /// </summary>
        public static long ToUnixTime(this DateTime utcDateTime)
        {
            return (long)utcDateTime.Subtract(unixBaseTime).TotalSeconds;
        }

        /// <summary>
        /// Преобразовать Unix-время в DateTime
        /// </summary>
        public static DateTime FromUnixTime(this long unixTime)
        {
            return unixBaseTime.AddSeconds(unixTime).ToLocalTime();
        }

        /// <summary>
        /// Возраст (в годах) на текущую дату
        /// </summary>
        /// <param name="birthday">Дата рождения</param>
        /// <returns>Возраст</returns>
        public static int GetAgeForNow(this DateTime birthday)
        {
            var now = DateTime.Today;
            var age = now.Year - birthday.Year;
            if (birthday.AddYears(age) > now)
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        /// <summary>
        /// Является ли дата "високосной" (29 февраля)?
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>true/false</returns>
        public static bool IsLeapDate(this DateTime date)
        {
            return (date.Month == 2) && (date.Day == 29);
        }

        /// <summary>
        /// Получить дату для указанного года. Нормализация високосных дат (29.02) выполняется в сторону уменьшения (28.02)
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="year">Год</param>
        /// <returns>Дата у указанном году</returns>
        public static DateTime GetForYear(this DateTime date, int year)
        {
            if (IsLeapDate(date) && (!DateTime.IsLeapYear(year)))
            {
                date = date.AddDays(-1);
            }
            return new DateTime(year, date.Month, date.Day);
        }

        /// <summary>
        /// Возвращает последний день года (31 декабря)
        /// </summary>
        public static DateTime LastDayOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31);
        }

        /// <summary>
        /// Первый день недели (понедельник) для указанной даты
        /// </summary>
        public static DateTime FirstDayOfWeek(this DateTime day)
        {
            int delta = CultureInfo.CurrentUICulture.DateTimeFormat.FirstDayOfWeek - day.DayOfWeek;
            return day.AddDays(delta).Date;
        }

        /// <summary>
        /// Последний день недели (воскресенье) для указанной даты
        /// </summary>
        public static DateTime LastDayOfWeek(this DateTime day)
        {
            return day.FirstDayOfWeek().AddDays(7).AddSeconds(-1);
        }

        /// <summary>
        /// Первый день месяца
        /// </summary>
        public static DateTime FirstDayOfMonth(this DateTime day)
        {
            return new DateTime(day.Year, day.Month, 1);
        }

        /// <summary>
        /// Последний день месяца
        /// </summary>
        public static DateTime LastDayOfMonth(this DateTime day)
        {
            return new DateTime(day.Year, day.Month, 1).AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        /// Начало суток
        /// </summary>
        public static DateTime StartOfDay(this DateTime a)
        {
            return a.Date;
        }

        /// <summary>
        /// Конец суток (последняя миллисекунда)
        /// </summary>
        public static DateTime EndOfDay(this DateTime a)
        {
            return a.AddDays(1).Date.AddMilliseconds(-1.0);
        }

        /// <summary>
        /// <c>true</c>, если две даты относятся к одному и тому же месяцу
        /// </summary>
        public static bool MonthEquals(this DateTime a, DateTime b)
        {
            return a.Month == b.Month && a.Year == b.Year;
        }

        /// <summary>
        /// Округление даты-времени до минут (отбрасываются секунды)
        /// </summary>
        /// <param name="a">Дата-время</param>
        /// <param name="down">
        /// <c>true</c> - округлять вниз (просто отбростить секунды),
        /// <c>false</c> - округлять вверх (отбросить секунды и прибавить минуту)</param>
        public static DateTime RoundToMinutes(this DateTime a, bool down)
        {
            return down
                ? new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 0)
                : new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 0).AddMinutes(1);
        }

        /// <summary>
        /// Nullable-вариант метода <see cref="RoundToMinutes(System.DateTime,bool)"/>
        /// </summary>
        public static DateTime? RoundToMinutes(this DateTime? dateTime, bool down)
        {
            return dateTime.HasValue ? (DateTime?)dateTime.Value.RoundToMinutes(down) : null;
        }
        
        /// <summary>
        /// Предыдущий по отношению к <paramref name="a"/> момент времени.
        /// </summary>
        /// <remarks>
        /// Минимальный квант времени считаем равным 1мс, поэтому метод
        /// возвращает время на миллисекунду меньшее, чем указанное.
        /// </remarks>
        public static DateTime NudgeDown(this DateTime a)
        {
            return a.AddMilliseconds(-1);
        }

        /// <summary>
        /// Начало месяца, к которому относится указанная дата
        /// </summary>
        public static DateTime MonthBegin(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// Конец (последняя миллисекунда) месяца, к которому относится указанная дата
        /// </summary>
        public static DateTime MonthEnd(this DateTime dateTime)
        {
            return dateTime.MonthBegin().AddMonths(1).NudgeDown();
        }

        /// <summary>
        /// Начало месяца, к которому относится указанная дата
        /// </summary>
        public static DateTime MonthBegin(this DateTime dt, Calendar calendar)
        {
            return new DateTime(dt.Year, dt.Month, 1, calendar);
        }

        /// <summary>
        /// Возвращает true, если дата <paramref name="dt"/> попадает во временной интервал от <paramref name="from"/>
        /// до <paramref name="to"/>, причём оба неравенства нестрогие.
        /// </summary>
        /// <param name="dt">Дата</param>
        /// <param name="from">Нижняя граница интервала</param>
        /// <param name="to">Верхняя граница интервала</param>
        public static bool Between(this DateTime dt, DateTime from, DateTime to)
        {
            if (to < from)
            {
                throw new ArgumentException("'From' value should be less or equal than 'to' value.");
            }
            return dt >= from && dt <= to;
        }

        /// <summary>
        /// Обертка над библиотечным методом DateTime.AddDays для вычитания дней.
        /// Сделана для повышения читаемости кода: SubstractDays(n) эквивалентно AddDays(-n)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="days"></param>
        public static DateTime SubstractDays(this DateTime dateTime, int days)
        {
            return dateTime.AddDays(-days);
        }

        /// <summary>
        /// Преобразует количество часов, прошедшее c начала суток, к соответствующему
        /// значению DateTime.
        /// </summary>
        /// <remarks>
        /// Фактически возвращается только время, а не дата-время (дата в результирующем
        /// значении будет 01.01.0001). Этот метод используется для работы с контролами,
        /// которые используются для редактирования времени.
        /// </remarks>
        public static DateTime FromHours(this int hours)
        {
            return new DateTime(1, 1, 1, 0, 0, 0, 0).AddHours(hours);
        }

        /// <summary>
        /// Преобразует количество минут, прошедшее c начала суток, к соответствующему
        /// значению DateTime.
        /// </summary>
        /// <remarks>
        /// Фактически возвращается только время, а не дата-время (дата в результирующем
        /// значении будет 01.01.0001). Этот метод используется для работы с контролами,
        /// которые используются для редактирования времени.
        /// </remarks>
        public static DateTime FromMinutes(this int minutes)
        {
            return new DateTime(1, 1, 1, 0, 0, 0, 0).AddMinutes(minutes);
        }

        /// <summary>
        /// Преобразует количество секунд, прошедшее c начала суток, к соответствующему
        /// значению DateTime.
        /// </summary>
        /// <remarks>
        /// Фактически возвращается только время, а не дата-время (дата в результирующем
        /// значении будет 01.01.0001). Этот метод используется для работы с контролами,
        /// которые используются для редактирования времени.
        /// </remarks>
        public static DateTime FromSeconds(this int seconds)
        {
            return new DateTime(1, 1, 1, 0, 0, 0, 0).AddSeconds(seconds);
        }

        /// <summary>
        /// Преобразует время в переданном значении DateTime
        /// в количество минут, прошедших с начала суток.
        /// </summary>
        public static int ToMinutes(this DateTime time)
        {
            return time.Minute + time.Hour * 60;
        }

        /// <summary>
        /// Преобразует время в переданном значении DateTime
        /// в количество секунд, прошедших с начала суток.
        /// </summary>
        public static int ToSeconds(this DateTime time)
        {
            return time.Second + time.Minute * 60;
        }
    }
}
