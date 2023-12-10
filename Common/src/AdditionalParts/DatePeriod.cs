using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class DatePeriod
    {
        #region Fields

        private const int WeekLength = 7;

        /// <summary>
        /// Набор пар "DatePeriod - Делегат, возвращающий соответствующий DateInterval".
        /// </summary>
        private static IDictionary<DatePeriod, Func<DateInterval>> intervalDelegates;

        #endregion

        #region Properties

        public static DatePeriod Custom
        {
            get { return CUSTOM; }
        }

        public static DatePeriod CurrentPeriod
        {
            get { return OPEN_PERIOD; }
        }

        public static DatePeriod CurrentDay
        {
            get { return TODAY; }
        }

        public static DatePeriod CurrentWeek
        {
            get { return CURRENT_WEEK; }
        }

        public static DatePeriod CurrentMonth
        {
            get { return CURRENT_MONTH; }
        }

        public static DatePeriod CurrentYear
        {
            get { return CURRENT_YEAR; }
        }

        public static DatePeriod LastDay
        {
            get { return YESTERDAY; }
        }

        public static DatePeriod LastWeek
        {
            get { return LAST_WEEK; }
        }

        public static DatePeriod LastMonth
        {
            get { return LAST_MONTH; }
        }

        public static DatePeriod LastYear
        {
            get { return LAST_YEAR; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает интервал дат с dateFrom 00:00:00.000 по dateTo 00:00:00.000
        /// включая нижнюю границу и исключая верхнюю. Исключение верхней границы
        /// осуществляется вычетом одной миллисекунды, т.е. фактически возвращается:
        /// с dateFrom 00:00:00.000 по (dateTo - 1) 23:59:59.999
        /// </summary>
        private static DateInterval BetweenIncludingLower(DateTime dateFrom, DateTime dateTo)
        {
            return new DateInterval(dateFrom.Date, dateTo.Date.AddTicks(-1));
        }

        /// <summary>
        /// Инициализация карты <see cref="intervalDelegates"/>. Решил, что лучше делать ее непосредственно
        /// при первом обращении к свойству <see cref="Interval"/>, чем по месту объявления поля или в статическом
        /// конструкторе класса, т.к. для корректной инициализации нам нужно, чтобы сгенерированные поля енума
        /// (TODAY, YESTERDAY и т.д.) были гарантированно инициализированы к этому моменту.
        /// </summary>
        private static void InitIntervalDelegatesIfNeeded()
        {
            if (intervalDelegates != null)
            {
                return;
            }

            intervalDelegates = new Dictionary<DatePeriod, Func<DateInterval>>()
            {
                {
                    CurrentPeriod, () =>
                        {
                            var today = DateTime.Now.Date;
                            var tomorrow = today.AddDays(1);
                            var periodStartDate = CafeSetup.IsCafeSetupLoaded
                                                      ? CafeSetup.INSTANCE.PeriodStartDate.GetValueOrFakeDefault()
                                                      : today;
                            return BetweenIncludingLower(periodStartDate, tomorrow);
                        }
                },
                {
                    CurrentDay, () =>
                        {
                            var today = DateTime.Now.Date;
                            var tomorrow = today.AddDays(1);
                            return BetweenIncludingLower(today, tomorrow);
                        }
                },
                {
                    CurrentWeek, () =>
                        {
                            var today = DateTime.Now.Date;
                            DateTime startOfWeek;
                            if (CompanySetup.NullableCorporation == null)
                            {
                                // Контрол может использоваться в любом проекте интеграций, который ссылается на OfficeCommon,
                                // В этом случе используем стандартный день начала недели для текущей культуры.
                                var dayOfWeek = today.DayOfWeek == DayOfWeek.Sunday
                                    ? WeekLength
                                    : (int) today.DayOfWeek;
                                startOfWeek = today.SubstractDays(dayOfWeek - 1);
                            }
                            else
                            {
                                startOfWeek = CalculateStartOfWeek(today);
                            }
                            var endOfWeek = startOfWeek.AddDays(WeekLength);

                            return BetweenIncludingLower(startOfWeek, endOfWeek);
                        }
                },
                {
                    CurrentMonth, () =>
                        {
                            var today = DateTime.Now.Date;

                            var startOfMonth = new DateTime(today.Year, today.Month, 1);
                            var endOfMonth = startOfMonth.AddMonths(1);

                            return BetweenIncludingLower(startOfMonth, endOfMonth);
                        }
                },
                {
                    CurrentYear, () =>
                        {
                            var today = DateTime.Now.Date;

                            var startOfYear = new DateTime(today.Year, 1, 1);
                            var endOfYear = startOfYear.AddYears(1);

                            return BetweenIncludingLower(startOfYear, endOfYear);
                        }
                },
                {
                    LastDay, () =>
                        {
                            var today = DateTime.Now.Date;
                            return BetweenIncludingLower(today.SubstractDays(1), today);
                        }
                },
                {
                    LastWeek, () =>
                        {
                            var startOfCurrentWeek = CurrentWeek.Interval.From.GetValueOrFakeDefault();

                            var startOfLastWeek = startOfCurrentWeek.SubstractDays(WeekLength);
                            var endOfLastWeek = startOfLastWeek.AddDays(WeekLength);

                            return BetweenIncludingLower(startOfLastWeek, endOfLastWeek);
                        }
                },
                {
                    LastMonth, () =>
                        {
                            var today = DateTime.Now.Date;

                            var startOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

                            var startOfLastMonth = startOfCurrentMonth.AddMonths(-1);
                            var endOfLastMonth = startOfCurrentMonth;

                            return BetweenIncludingLower(startOfLastMonth, endOfLastMonth);
                        }
                },
                {
                    LastYear, () =>
                        {
                            var today = DateTime.Now.Date;

                            var startOfLastYear = new DateTime(today.Year - 1, 1, 1);
                            var endOfLastYear = startOfLastYear.AddYears(1);

                            return BetweenIncludingLower(startOfLastYear, endOfLastYear);
                        }
                }
            };
        }

        private static DateTime CalculateStartOfWeek(DateTime dt)
        {
            var startOfWeek = CompanySetup.Corporation.DateFormatSettings.FirstDayOfWeek.ToDayOfWeek();
            var diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Возвращает интервал дат, соответствующий данному периоду дат
        /// </summary>
        public DateInterval Interval
        {
            get
            {
                InitIntervalDelegatesIfNeeded();

                Func<DateInterval> func;
                if (!intervalDelegates.TryGetValue(this, out func))
                {
                    throw new NotSupportedException(string.Format("Incorrect DatePeriod = {0}", this));
                }

                return func();
            }
        }

        #endregion
    }
}
