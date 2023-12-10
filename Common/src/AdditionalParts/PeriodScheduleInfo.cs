using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

namespace Resto.Data
{
    partial class PeriodScheduleInfo
    {
        #region Fields

        [CanBeNull]
        private static IEqualityComparer<PeriodScheduleInfo> byIdEqualityComparer = null;

        #endregion

        #region Constructors

        public PeriodScheduleInfo(Guid? id, string name, List<PeriodScheduleItem> periods)
        {
            this.id = id;
            this.name = name;
            this.periods = periods;
        }

        #endregion

        #region Properties

        [NotNull]
        public static IEqualityComparer<PeriodScheduleInfo> ByIdEqualityComparer
        {
            get
            {
                if (byIdEqualityComparer == null)
                {
                    byIdEqualityComparer = new PeriodScheduleInfoByIdEqualityComparer();
                }

                return byIdEqualityComparer;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Проверяет, активно ли расписание на указанную дату
        /// </summary>
        public bool IsActiveAt(DateTime dateTime)
        {
            return Periods.Any(period => period.IsActiveAt(dateTime));
        }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }

        public override int GetHashCode()
        {
            int result = Id.GetValueOrFakeDefault().GetHashCode();
            result = result * 31 + (Name != null ? Name.GetHashCode() : 0);

            int periodsHashCode = 0;

            foreach (var period in Periods)
            {
                periodsHashCode = periodsHashCode ^ period.GetHashCode();
            }

            result = result * 31 + periodsHashCode;

            return result;
        }

        public override bool Equals(object obj)
        {
            var periodScheduleInfo = obj as PeriodScheduleInfo;
            if (periodScheduleInfo == null)
            {
                return false;
            }

            return Id.GetValueOrFakeDefault() == periodScheduleInfo.Id.GetValueOrFakeDefault() 
                && Name == periodScheduleInfo.Name 
                && Periods.Count == periodScheduleInfo.Periods.Count
                && Periods.Intersect(periodScheduleInfo.Periods).Count() == Periods.Count;
        }

        #endregion

        #region Nested types

        public class PeriodScheduleInfoByIdEqualityComparer : IEqualityComparer<PeriodScheduleInfo>
        {
            public bool Equals(PeriodScheduleInfo x, PeriodScheduleInfo y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return x.Id.GetValueOrFakeDefault().Equals(y.Id.GetValueOrFakeDefault());
            }

            public int GetHashCode(PeriodScheduleInfo obj)
            {
                return obj != null ? obj.Id.GetValueOrFakeDefault().GetHashCode() : 0;
            }
        }

        #endregion
    }
}
