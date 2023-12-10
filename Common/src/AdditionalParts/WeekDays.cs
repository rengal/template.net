using System;

namespace Resto.Data
{
    public partial class WeekDays : IEquatable<DayOfWeek>
    {
        public DayOfWeek ToDayOfWeek()
        {
            switch (_Value)
            {
                case "SUNDAY": return DayOfWeek.Sunday;
                case "MONDAY": return DayOfWeek.Monday;
                case "TUESDAY": return DayOfWeek.Tuesday;
                case "WEDNESDAY": return DayOfWeek.Wednesday;
                case "THURSDAY": return DayOfWeek.Thursday;
                case "FRIDAY": return DayOfWeek.Friday;
                case "SATURDAY": return DayOfWeek.Saturday;
                default: throw new ArgumentException("Undefined enum constant:" + _Value);
            }
        }

        public static DayOfWeek ToDayOfWeek(WeekDays day)
        {
            return day.ToDayOfWeek();
        }

        public static WeekDays FromDayOfWeek(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return SUNDAY;
                case DayOfWeek.Monday:
                    return MONDAY;
                case DayOfWeek.Tuesday:
                    return TUESDAY;
                case DayOfWeek.Wednesday:
                    return WEDNESDAY;
                case DayOfWeek.Thursday:
                    return THURSDAY;
                case DayOfWeek.Friday:
                    return FRIDAY;
                case DayOfWeek.Saturday:
                    return SATURDAY;
                default: throw new ArgumentException("Undefined enum constant.");
            }
        }

        public override int GetHashCode()
        {
            return _Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var weekDay = obj as WeekDays;
            if (weekDay != null)
            {
                return _Value.Equals(weekDay._Value);
            }

            var dayOfWeek = obj as DayOfWeek?;
            if (dayOfWeek != null)
            {
                return Equals(dayOfWeek.Value);
            }

            return false;
        }

        public bool Equals(DayOfWeek other)
        {
            return ToDayOfWeek().Equals(other);
        }
    }
}