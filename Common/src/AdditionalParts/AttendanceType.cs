using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class AttendanceType : IComparable<AttendanceType>
    {

        /// <summary>
        /// Тип явки можно удалять
        /// </summary>
        public bool Deletable
        {
            get { return !System.GetValueOrDefault(); }
        }

        /// <summary>
        /// true - тип в статусе "Явка", иначе "Неявка"
        /// </summary>
        public bool StatusIsAttendance
        {
            get { return Status.GetValueOrDefault(); }
            set { Status = value; }
        }

        public override string ToString()
        {
            return NameLocal + " (" + payKoeff * 100 + "%)";
        }

        #region IComparable<AttendanceType> Members

        public int CompareTo(AttendanceType other)
        {
            if (other == null)
            {
                return 1;
            }
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            int result = System.GetValueOrDefault().CompareTo(other.System.GetValueOrDefault()) * 100 + Decimal.Compare(PayKoeff.GetValueOrDefault(), other.PayKoeff.GetValueOrDefault());
            if (result == 0)
            {
                result = Comparer<Guid>.Default.Compare(Id, other.Id);
            }
            return result;
        }

        #endregion
    }
}