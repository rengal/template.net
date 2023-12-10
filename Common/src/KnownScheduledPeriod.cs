using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Предустановленный период для периодических платежей
    /// </summary>
    /// <remarks>
    /// Сервер умеет работать с достаточно произовльными периодами <see cref="ScheduledPeriod"/>
    /// (каждые N дней, каждые N месяцев и т.п.), но в бек-офисе пока используется набор
    /// предустановленных периодов.
    /// </remarks>
    public class KnownScheduledPeriod
    {
        /// <summary>
        /// Период для разового платежа (т.е. отсутствие периодичности).
        /// </summary>
        public static readonly KnownScheduledPeriod NoPeriod = new KnownScheduledPeriod(ScheduledPeriodType.SINGLE_TIME, 1, string.Empty);

        /// <summary>
        /// Предустановленные периоды
        /// </summary>
        public static readonly IReadOnlyCollection<KnownScheduledPeriod> KnownPeriods = new List<KnownScheduledPeriod>
        {
            new KnownScheduledPeriod(ScheduledPeriodType.EVERY_N_DAYS, 1, Resources.ScheduledPeriodOneDay),
            new KnownScheduledPeriod(ScheduledPeriodType.EVERY_N_DAYS, 3, Resources.ScheduledPeriodThreeDays),
            new KnownScheduledPeriod(ScheduledPeriodType.EVERY_N_DAYS, 7, Resources.ScheduledPeriodWeek),
            new KnownScheduledPeriod(ScheduledPeriodType.EVERY_N_DAYS, 10, Resources.ScheduledPeriodTenDays),
            new KnownScheduledPeriod(ScheduledPeriodType.EVERY_N_MONTHS, 1, Resources.ScheduledPeriodMonth),
            new KnownScheduledPeriod(ScheduledPeriodType.EVERY_N_MONTHS, 3, Resources.ScheduledPeriodQuater),
            new KnownScheduledPeriod(ScheduledPeriodType.EVERY_N_MONTHS, 12, Resources.ScheduledPeriodYear),
        };

        private KnownScheduledPeriod(ScheduledPeriodType periodType, int periodValue, string name)
        {
            PeriodType = periodType;
            PeriodValue = periodValue;
            Name = name;
        }

        /// <summary>
        /// Название предустановленного периода
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Тип периодичности
        /// </summary>
        public ScheduledPeriodType PeriodType { get; private set; }

        /// <summary>
        /// Значение периодичности
        /// </summary>
        public int PeriodValue { get; private set; }

        /// <summary>
        /// <c>true</c>, если данный предустановленный период является периодическим
        /// </summary>
        public bool IsPeriodic
        {
            get { return PeriodType != ScheduledPeriodType.SINGLE_TIME; }
        }

        /// <summary>
        /// Возвращает предустановленный период, соответствующий указанному поизвольному периоду.
        /// Если среди прудстановленных соответствие не найдено, выбрасывает исключение, т.к.
        /// произвольные периоды бек-офисом не поддерживаются.
        /// </summary>
        [NotNull]
        public static KnownScheduledPeriod Get([CanBeNull] ScheduledPeriod period)
        {
            if (period == null || period.Type == ScheduledPeriodType.SINGLE_TIME)
            {
                return NoPeriod;
            }

            var result = KnownPeriods
                .FirstOrDefault(kp => kp.PeriodType == period.Type && kp.PeriodValue == period.Value);
            if (result == null)
            {
                // Пока работаем только с предустановленными периодами;
                // произвольные запрещаем.
                throw new RestoException("Unknown payment period");
            }

            return result;
        }

        public override string ToString()
        {
            return Name;
        }

        #region Equality

        protected bool Equals(KnownScheduledPeriod other)
        {
            return Equals(PeriodType, other.PeriodType) && PeriodValue == other.PeriodValue;
        }

        public override bool Equals(object obj)
        {
            var period = obj as KnownScheduledPeriod;
            return period != null && Equals(period);
        }

        public override int GetHashCode()
        {
            return new { periodType = PeriodType, periodValue = PeriodValue }.GetHashCode();
        }

        #endregion
    }
}