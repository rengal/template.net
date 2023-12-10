using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Properties;
using Resto.UI.Common;
using OfficeCommonResources = Resto.Common.Properties.Resources;

namespace Resto.Common
{
    /// <summary>
    /// Утилитный класс для работы с датой и временем.
    /// </summary>
    /// <seealso cref="DateTimeExtentions"/>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Минимальная дата, которую может «понять» сервер (см. <see cref="DateTimeServerConstants.MIN_DATE"/>).
        /// </summary>
        /// <seealso cref="DateTimeServerConstants"/>
        public static readonly DateTime MinDate = new DateTime(1900, 1, 1);

        /// <summary>
        /// Максимальная дата, которую может «понять» сервер (см. <see cref="DateTimeServerConstants"/>).
        /// </summary>
        /// <seealso cref="DateTimeServerConstants.MAX_DATE"/>
        public static readonly DateTime MaxDate = new DateTime(2300, 1, 1);

        /// <summary>
        /// Количество минут в сутках
        /// </summary>
        public const int MinutesPerDay = 24 * 60;

        /// <summary>
        /// Количество дней в месяце.
        /// </summary>
        /// <remarks>
        /// Для расчета каких-либо интервалов, не привязанных в дате (например: срок хранения продуктов).
        /// </remarks>
        public const int DefaultDaysInMonth = 30;

        /// <summary>
        /// Получить словарь с переводом дней недели.
        /// </summary>
        /// <param name="isShort"> Если необходимо получить словарь с короткими именами дней недели,
        /// передать в качестве параметра <c>true</c>.</param>
        public static IDictionary<DayOfWeek, string> GetDayNames(bool isShort = false)
        {
            var format = CultureInfo.CurrentUICulture.DateTimeFormat;
            return GetWeekDays().ToDictionary(
                dayOfWeek => dayOfWeek,
                dayOfWeek => isShort ? format.GetShortestDayName(dayOfWeek) : format.GetDayName(dayOfWeek));
        }

        /// <summary>
        /// Получить номер дня недели, принятый "у нас".
        /// </summary>
        /// <param name="day">День недели.</param>
        /// <returns>Номер дня недели, принятый "у нас".</returns>
        public static int GetDayOfWeekNumber(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
            }
            return 0;
        }

        /// <summary>
        /// Преобразует значение ExpirationDate из миллисекунд
        /// </summary>
        /// <param name="milliseconds">ExpirationDate в миллисекундах</param>
        /// <returns>Кортеж с параметрами типа int - количество месяцев; TimeSpan - содержит дни, часы, минуты</returns>
        public static Tuple<int, TimeSpan> ExpirationDateFromMillisecondConverter(long milliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            var monthCount = (int) timeSpan.TotalDays / DefaultDaysInMonth;
            timeSpan = timeSpan.Subtract(new TimeSpan(DefaultDaysInMonth * monthCount, 0, 0, 0));

            return new Tuple<int, TimeSpan>(monthCount, timeSpan);
        }

        /// <summary>
        /// Возвращает числа слева и справа от разделителя.
        /// </summary>
        /// <param name="interval">Строковое представление интервала</param>
        /// <param name="separator">Разделитель</param>
        public static Pair<int, int> GetCustomIntervalValue([CanBeNull] string interval, char separator)
        {
            if (interval != null)
            {
                var strings = interval.Split(separator);
                if (strings.Length == 2)
                {
                    var first = int.Parse(strings[0]);
                    var second = int.Parse(strings[1]);
                    return new Pair<int, int>(first, second);
                }
            }

            return new Pair<int, int>(0, 0);
        }

        /// <summary>
        /// Преобразует номер дня недели (в серверном формате) в день недели
        /// </summary>
        /// <param name="weekDayNumber">Номер дня недели в серверном формарте</param>
        /// <returns>День недели</returns>
        public static DayOfWeek GetDayOfWeek(int weekDayNumber)
        {
            switch (weekDayNumber)
            {
                case 0:
                    return DayOfWeek.Monday;
                case 1:
                    return DayOfWeek.Tuesday;
                case 2:
                    return DayOfWeek.Wednesday;
                case 3:
                    return DayOfWeek.Thursday;
                case 4:
                    return DayOfWeek.Friday;
                case 5:
                    return DayOfWeek.Saturday;
                case 6:
                    return DayOfWeek.Sunday;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weekDayNumber), weekDayNumber, "weekDayNumber must be value between 0 and 6");
            }
        }

        /// <summary>
        /// Временный метод, для получения строки, описывающей количество дней.
        /// </summary>
        /// <param name="days">количество дней</param>
        public static string GetStringRepresentationOfDaysCount(int days)
        {
            if (days >= 10 && days <= 20)
            {
                return Resources.DayForm3;
            }
            var lastDigit = days % 10;
            if (lastDigit == 0
                || lastDigit > 4)
            {
                return Resources.DayForm3;
            }
            if (lastDigit == 1)
            {
                return Resources.DayForm1;
            }
            return Resources.DayForm2;
        }

        /// <summary>
        /// Получить дни недели в порядке, определяемом текущей локалью (в РФ неделя 
        /// начинается с понедельника, у наших западных партнёров - с воскресенья)
        /// </summary>
        public static IEnumerable<DayOfWeek> GetWeekDays()
        {
            var result = new List<DayOfWeek>();
            var firstDayFound = false;
            var prevGoingDays = new List<DayOfWeek>();
            foreach (var day in EnumUtils.GetValues<DayOfWeek>())
            {
                if (day == CultureInfo.CurrentUICulture.DateTimeFormat.FirstDayOfWeek)
                {
                    firstDayFound = true;
                }
                if (firstDayFound)
                {
                    result.Add(day);
                }
                else
                {
                    prevGoingDays.Add(day);
                }
            }
            result.AddRange(prevGoingDays);
            return result;
        }

        /// <summary>
        /// Округление минут до заданной точности в минутах, например, 
        /// если точность 15 минут, то 47 округляется до 45, а 53 до 60
        /// </summary>
        /// <param name="currentMinutes">Значение минут, которое требуется округлить</param>
        /// <param name="roundValue">Точность до которой округляются минуты</param>
        /// <returns>Значение, которое нужно прибавить к currentMinutes, чтобы получить округленное значение</returns>
        public static int CalculateMinutesToAddForRound(int currentMinutes, int roundValue)
        {
            if (currentMinutes < 0)
                throw new ArgumentOutOfRangeException(nameof(currentMinutes));

            if (roundValue <= 0)
                throw new ArgumentOutOfRangeException(nameof(roundValue));

            return (int)Math.Round((float)currentMinutes / roundValue, MidpointRounding.AwayFromZero) * roundValue - currentMinutes;
        }

        /// <summary>
        /// Округление текущего времени до заданной точности в минутах, 
        /// например, если точность 15 минут и текущее время 1:07, то в результате округления получим 1:00,
        /// а если 1:08, то 1:15
        /// </summary>
        /// <param name="current">Время, которое нужно округлить</param>
        /// <param name="minutesRoundValue">Точность в минутах</param>
        /// <returns>Округленное значение</returns>
        public static DateTime RoundToMinutes(DateTime current, int minutesRoundValue)
        {
            var addMinutes = CalculateMinutesToAddForRound(current.Minute, minutesRoundValue);
            return current.AddMinutes(addMinutes);
        }

        /// <summary>
        /// Округление промежутка времени до заданной точности в минутах, 
        /// например, если точность 15 минут и заданный промежуток 7 минут, то в результате округления получим 0 минут,
        /// а если 8 минут, то получим 15 минут
        /// </summary>
        /// <param name="current">Промежуток времени, который нужно округлить</param>
        /// <param name="minutesRoundValue">Точность в минутах</param>
        /// <returns>Округленное значение</returns>
        public static TimeSpan RoundToMinutes(TimeSpan current, int minutesRoundValue)
        {
            var addMinutes = CalculateMinutesToAddForRound((int)current.TotalMinutes, minutesRoundValue);
            return current.Add(new TimeSpan(0, 0, addMinutes, 0));
        }

        /// <summary>
        /// Получает первый день по номеру недели. Номер недели соответсвует ISO 8601:2000. 
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="weekOfYear">Номер недели.</param>
        /// <returns>Первый день недели.</returns>
        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            var firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        /// <summary>
        /// Нахождение минимального значения
        /// </summary>
        public static DateTime Min(DateTime d1, DateTime d2)
        {
            return d1 < d2 ? d1 : d2;
        }

        /// <summary>
        /// Нахождение максимального значения
        /// </summary>
        public static DateTime Max(DateTime d1, DateTime d2)
        {
            return d1 > d2 ? d1 : d2;
        }

        public static string FormatDateTime(DateTime time)
        {
            return time.ToString("g");
        }

        public static string FormatDate(DateTime time)
        {
            return time.ToString("d");
        }

        /// <summary>
        /// Возвращает дату/время со временем начала дня.
        /// Всегда приводит в меньшую сторону.
        /// </summary>
        /// <param name="date">Дата/время</param>
        public static DateTime BusinessStartDateTime(DateTime date)
        {
            return BusinessStartDateTime(date, CafeSetup.INSTANCE.BusinessDateSettings);
        }

        public static DateTime BusinessStartDateTime(DateTime date, BusinessDateSettings businessDateSettings)
        {
            int hours;
            int minutes;
            if (businessDateSettings.DayStartTime.Minutes <= MinutesPerDay)
            {
                hours = businessDateSettings.DayStartTime.Minutes / 60;
                minutes = businessDateSettings.DayStartTime.Minutes % 60;
            }
            else
            {
                hours = 0;
                minutes = 0;
            }

            if (date.TimeOfDay.TotalMinutes < businessDateSettings.DayStartTime.Minutes)
            {
                try
                {
                    date = date.AddDays(-1);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            return new DateTime(date.Year, date.Month, date.Day, hours, minutes, 0);
        }

        /// <summary>
        /// Исправляет дату, если она выходит за допустимый диапазон.
        /// </summary>
        /// <param name="dateTime">Дата.</param>
        /// <returns>Корректная дата.</returns>
        public static DateTime CorrectDateTime(DateTime dateTime)
        {
            if (dateTime < MinDate) return MinDate;
            return dateTime > MaxDate ? MaxDate : dateTime;
        }

        /// <summary>
        /// Разница (в днях) между двумя датами (речь именно про даты, а не про даты-время)
        /// </summary>
        public static int DaysBetween(DateTime from, DateTime to)
        {
            if (from.Kind != to.Kind)
            {
                throw new ArgumentException("'from' and 'to' dates must be of the same type");
            }

            switch (from.Kind)
            {
                case DateTimeKind.Unspecified:
                    // Поверхностный осмотр существующего кода говорит о том, что Unspecified-даты
                    // обычно расматриваются как локальные, поэтому кастуем их к Local
                    from = DateTime.SpecifyKind(from, DateTimeKind.Local);
                    to = DateTime.SpecifyKind(to, DateTimeKind.Local);
                    break;
                case DateTimeKind.Utc:
                    // UTC-даты нужно преобразовать к локальным
                    to = to.ToLocalTime();
                    from = from.ToLocalTime();
                    break;
                case DateTimeKind.Local:
                    // Для Local используем даты как есть 
                    break;
                default:
                    throw new UnsupportedEnumValueException<DateTimeKind>(from.Kind);
            }

            return (int)((RoundDate(to) - RoundDate(from)).TotalDays);
        }

        /// <summary>
        /// Округляет переданные дату-время вверх или вниз до ближайшего начала суток
        /// </summary>
        private static DateTime RoundDate(DateTime dateTime)
        {
            return dateTime.AddHours(12).Date;
        }

        /// <summary>
        /// Возвращает <c>true</c>, если <paramref name="date"/> меньше даты начала открытого периода.
        /// Иначе <c>false</c>
        /// </summary>
        public static bool LessThanPeriodStart(DateTime date)
        {
            return date < CafeSetup.INSTANCE.PeriodStartDate.GetValueOrDefault();
        }

        /// <summary>
        /// Возвращает дату учетного дня.
        /// </summary>
        /// <param name="departments">Подразделения</param>
        /// <param name="date">Дата, относительно которой считается учетный день.
        /// Если <c>null</c>, то подразумевается <see cref="DateTime.Now"/></param>
        public static DateTime GetOperationalDay([CanBeNull] IEnumerable<IWithOperationalDaySettings> departments, DateTime? date = null)
        {
            var dateTime = date ?? DateTime.Now;
            var dayInterval = GetOperationalDayInterval(dateTime.Date, false, departments?.ToArray());

            if (dayInterval.InInterval(dateTime))
            {
                return dayInterval.DateFrom.Date;
            }

            if (dayInterval.DateTo < dateTime)
            {
                return dayInterval.DateFrom.Date.AddDays(1);
            }

            return dayInterval.DateFrom.Date.AddDays(-1);
        }

        /// <summary>
        /// Возвращает интервал учетного дня.
        /// </summary>
        /// <param name="date">Дата оносительно которой рассчитывается интервал</param>
        /// <param name="considerDayCloseTime">Если <c>true</c> - датой окончания интервала является дата окончания учетного дня из настроек,
        /// Иначе - дата начала учетного дня +1 день</param>
        /// <param name="departments">Подразделения</param>
        /// <remarks>
        /// В зависимости от того используется Chain или RMS определяет откуда брать настройки учетного дня.
        /// Для Chain и RMS всегда из <see cref="CafeSetup"/> (по умолчанию 6:00 - 23:00) для одноресторанного Chain из настроек ТП.
        /// </remarks>
        [NotNull]
        public static DateInterval GetOperationalDayInterval(DateTime date, bool considerDayCloseTime = true,
            IReadOnlyCollection<IWithOperationalDaySettings> departments = null)
        {
            IWithOperationalDaySettings entity;
            if (departments != null && departments.Count == 1 ||
                MultiDepartments.Instance.IsChainSingleDepartmentMode)
            {
                entity = departments == null
                    ? MultiDepartments.Instance.ChainOrRmsSingleDepartment
                    : departments.Single();
            }
            else
            {
                entity = CafeSetup.INSTANCE;
            }

            return GetOperationalDayInterval(entity, date, considerDayCloseTime);
        }

        /// <summary>
        /// Возвращает интервал учетного дня.
        /// </summary>
        /// <param name="entity">Сущность с настройками учетного дня</param>
        /// <param name="date">Дата оносительно которой рассчитывается интервал</param>
        /// <param name="considerDayCloseTime">Если <c>true</c> - датой окончания интервала является дата окончания учетного дня из настроек,
        /// Иначе - дата начала учетного дня +1 день</param>
        [NotNull]
        public static DateInterval GetOperationalDayInterval(IWithOperationalDaySettings entity, DateTime date, bool considerDayCloseTime = true)
        {
            var startTime = entity.BusinessDateSettings.DayStartTime.Minutes;
            var startDate = date.Date.AddMinutes(startTime);
            if (date < startDate)
            {
                startDate = startDate.AddDays(-1);
            }

            DateTime endDate;
            if (considerDayCloseTime)
            {
                var endTime = entity.OperationalDaySettings.DayCloseTime.Minutes;
                endDate = (startTime >= endTime
                    ? startDate.Date.AddDays(1)
                    : startDate.Date).AddMinutes(endTime);
            }
            else
            {
                endDate = startDate.AddDays(1);
            }

            return new DateInterval(startDate, endDate);
        }
    }
}