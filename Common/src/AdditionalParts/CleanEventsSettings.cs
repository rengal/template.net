using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    partial class CleanEventsSettings : IEquatable<CleanEventsSettings>
    {
        public bool Equals(CleanEventsSettings other)
        {
            if (other == null)
            {
                return false;
            }

            return
                EventsDaysToStore == other.EventsDaysToStore &&
                IsScheduleActive == other.IsScheduleActive &&
                DaysOfWeek.Count == other.DaysOfWeek.Count &&
                TimesOfDay.Count == other.TimesOfDay.Count &&
                DaysOfWeek.All(d => other.DaysOfWeek.Contains(d));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CleanEventsSettings);
        }

        public override int GetHashCode()
        {
            return new
            {
                EventsDaysToStore,
                IsScheduleActive,
                DaysOfWeek,
                TimesOfDay
            }.GetHashCode();
        }
    }
}