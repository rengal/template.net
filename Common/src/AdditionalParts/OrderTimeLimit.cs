using System;
using System.Collections.Generic;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class OrderTimeLimit : IComparable<OrderTimeLimit>, IComparable, IEquatable<OrderTimeLimit>
    {

        #region IComparable<OrderTimeLimit> Members

        /// <summary>
        /// <para>Сравнивает два ограничения времени.</para>
        /// <para>Меньшим из них считается то, которое вступает в силу раньше.</para>
        /// </summary>
        /// <param name="otherLimit">Лимит времени, с которым производится сравнение</param>
        /// <returns>Меньше нуля, если текущий лимит при прочих равных вступит в силу раньше того, с которым производится сравнение.
        /// 0, если одновременно. Больше нуля, если позже.</returns>
        public int CompareTo(OrderTimeLimit otherLimit)
        {
            int result;

            if (otherLimit == null)
            {
                result = 1;
            }
            else
            {
                result = -Comparer<int>.Default.Compare(Days.GetValueOrFakeDefault(), otherLimit.Days.GetValueOrFakeDefault());
                if (result == 0)
                {
                    result = Comparer<TimeSpan>.Default.Compare(Time.GetValueOrFakeDefault().TimeOfDay, otherLimit.Time.GetValueOrFakeDefault().TimeOfDay);
                }
            }

            return result;
        }

        #endregion

        #region IComparable members

        /// <summary>
        /// Сравнивает данный лимит времени с другим объектом
        /// </summary>
        /// <param name="otherObj">Объект, с которым производится сравнение</param>
        public int CompareTo(object otherObj)
        {
            return CompareTo(otherObj as OrderTimeLimit);
        }

        #endregion

        public static bool operator <(OrderTimeLimit limit1, OrderTimeLimit limit2)
        {
            return limit1.CompareTo(limit2) < 0;
        }

        public static bool operator >(OrderTimeLimit limit1, OrderTimeLimit limit2)
        {
            return limit1.CompareTo(limit2) > 0;
        }

        public static bool operator <=(OrderTimeLimit limit1, OrderTimeLimit limit2)
        {
            return limit1.CompareTo(limit2) <= 0;
        }

        public static bool operator >=(OrderTimeLimit limit1, OrderTimeLimit limit2)
        {
            return limit1.CompareTo(limit2) >= 0;
        }

        public bool Equals(OrderTimeLimit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return time.Equals(other.time) && days == other.days;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrderTimeLimit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (time.GetHashCode() * 397) ^ days.GetHashCode();
            }
        }
    }
}