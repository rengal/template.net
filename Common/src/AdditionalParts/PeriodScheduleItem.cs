using System;
using System.Collections.Generic;
using System.Linq;
using EnumerableExtensions;

namespace Resto.Data
{
    public partial class PeriodScheduleItem
    {
        public PeriodScheduleItem(TimeSpan begin, TimeSpan end, HashSet<WeekDays> daysOfWeek)
        {
            this.begin = begin;
            this.end = end;
            this.daysOfWeek = daysOfWeek;
        }
        
        /// <summary>
        /// Проверяет, активна ли часть недельного расписания на указанную дату
        /// </summary>
        public bool IsActiveAt(DateTime dateTime)
        {
            WeekDays day = WeekDays.FromDayOfWeek(dateTime.DayOfWeek);

            return DaysOfWeek.Contains(day) &&
                   Begin.CompareTo(dateTime.TimeOfDay) <= 0 &&
                   (dateTime.TimeOfDay.CompareTo(end) < 0 || End.Equals(TimeSpan.Zero));
        }

        public override int GetHashCode()
        {
            int daysOfWeekHash = 0;
            daysOfWeek.ForEach(d => daysOfWeekHash ^= d.GetHashCode());

            return new
                {
                    Begin,
                    End,
                    daysOfWeekHash
                }.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var periodScheduleItem = obj as PeriodScheduleItem;
            if (periodScheduleItem == null)
            {
                return false;
            }

            if (!Begin.Equals(periodScheduleItem.Begin) || !End.Equals(periodScheduleItem.End))
            {
                return false;
            }

            if (DaysOfWeek.Count != periodScheduleItem.DaysOfWeek.Count)
            {
                return false;
            }

            return DaysOfWeek
                .Select(d => d.ToDayOfWeek())
                .Intersect(periodScheduleItem.DaysOfWeek.Select(d => d.ToDayOfWeek()))
                .Count() == DaysOfWeek.Count;
        }
    }
}
