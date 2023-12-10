using System;
using Resto.Common.Extensions;

namespace Resto.Data
{
    partial class DateDepartmentConceptionStructure : IEquatable<DateDepartmentConceptionStructure>
    {
        private long? id;

        /// <summary>
        /// Id.
        /// </summary>
        public long Id
        {
            get { return id ?? (id = GetLongHashCode()).Value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((DateDepartmentConceptionStructure)obj);
        }

        public bool Equals(DateDepartmentConceptionStructure other)
        {
            if (other == null)
            {
                return false;
            }

            return ((Department == null && other.Department == null) || Department.Equals(other.Department)) &&
                    ((Conception == null && other.Conception == null) || Conception.Equals(other.Conception)) &&
                    Date.GetValueOrFakeDefault().Equals(other.Date.GetValueOrFakeDefault());
        }

        /// <summary>
        /// Сбрасывает id, нужно вызывать при изменении любого параметра участвующего в построении <see cref="GetLongHashCode"/>.
        /// </summary>
        public void ClearId()
        {
            id = null;
        }

        /// <summary>
        /// Возвращает long hashcode объекта.
        /// Использование такого варианта вместо int, уменьшает на порядок вероятность совпадения кодов.
        /// </summary>
        private long GetLongHashCode()
        {
            long hash1 = Department.GetHashCode();
            long hash2 = Date.GetValueOrFakeDefault().GetHashCode();
            long hash3 = (Conception != null ? Conception.GetHashCode() : 0);

            return ((hash1 << 16) + hash2) ^ hash3;
        }

        public override int GetHashCode()
        {
            return (Department != null ? Department.GetHashCode() : 0) ^ Date.GetValueOrFakeDefault().GetHashCode() ^ (Conception != null ? Conception.GetHashCode() : 0);
        }

        /// <summary>
        /// For debug purposes only
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} | {1} | {2}", Conception, Department, Date.ToString());
        }
    }
}
