using System;

namespace Resto.Data
{
    /// <summary>
    /// Бэковское дополнение для работы с классом <see cref="DateInfo"/>.
    /// </summary>    
    public partial class DateInfo : IComparable, IComparable<DateInfo>
    {
        public DateInfo(int year, int month, int day, bool nothing)
        {
            this.year = year;
            this.month = month - 1;
            this.day = day;
        }

        public DateInfo(DateTime date)
            : this(date.Year, date.Month, date.Day, true)
        {
        }

        public bool Equals(DateInfo obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.year == year && obj.month == month && obj.day == day;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(DateInfo)) return false;
            return Equals((DateInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = year;
                result = (result * 397) ^ month;
                result = (result * 397) ^ day;
                return result;
            }
        }

        public static bool operator ==(DateInfo left, DateInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DateInfo left, DateInfo right)
        {
            return !Equals(left, right);
        }

        public DateTime ToDateTime()
        {
            return new DateTime(year, month + 1, day);
        }

        public override string ToString()
        {
            return ToDateTime().ToString("d");
        }

        public int CompareTo(object obj)
        {
            return CompareTo((DateInfo)obj);
        }

        public int CompareTo(DateInfo other)
        {
            if (other == null)
            {
                return 1;
            }

            var date1 = ToDateTime();
            var date2 = other.ToDateTime();

            return date1.CompareTo(date2);
        }
    }
}
