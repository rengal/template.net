using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    /// <summary>
    /// Продолжительность отработанного времени в минутах для SalaryReportGrid.
    /// </summary>
    partial class DurationInMinutes : IEquatable<DurationInMinutes>, IComparable<DurationInMinutes>, IComparable
    {
        public long MinutesOrZero
        {
            get { return Minutes ?? 0L; }
        }

        public bool Equals(DurationInMinutes other)
        {
            return other != null && Equals(Minutes, other.Minutes);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DurationInMinutes);
        }

        public override int GetHashCode()
        {
            return Minutes.GetHashCode();
        }

        public int CompareTo(DurationInMinutes other)
        {
            if (other == null)
            {
                return 1;
            }
            if (other.Minutes == null)
            {
                return Minutes == null ? 0 : 1;
            }
            if (Minutes == null)
            {
                return -1;
            }
            return Minutes.Value.CompareTo(other.Minutes.Value);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as DurationInMinutes);
        }

        public override string ToString()
        {
            if (Minutes == null)
            {
                return string.Empty;
            }
            var ts = TimeSpan.FromMinutes(Minutes.Value);
            return string.Format("{0:0#}:{1:0#}", Math.Floor(ts.TotalHours), ts.Minutes);
        }

        public static DurationInMinutes operator +(DurationInMinutes d1, DurationInMinutes d2)
        {
            return Calc(d1, d2, (op1, op2) => op1 + op2);
        }

        public static DurationInMinutes operator -(DurationInMinutes d1, DurationInMinutes d2)
        {
            return Calc(d1, d2, (op1, op2) => op1 - op2);
        }

        public static DurationInMinutes operator *(DurationInMinutes d1, DurationInMinutes d2)
        {
            return Calc(d1, d2, (op1, op2) => op1 * op2);
        }

        public static DurationInMinutes operator /(DurationInMinutes d1, DurationInMinutes d2)
        {
            return Calc(d1, d2, (op1, op2) => op1 / op2);
        }

        [CanBeNull]
        private static DurationInMinutes Calc(DurationInMinutes d1, DurationInMinutes d2,
            Func<long, long, long> operatorFunc)
        {
            if (d1 == null && d2 == null)
            {
                return null;
            }
            if (d1 != null && d2 != null && d1.Minutes == null && d2.Minutes == null)
            {
                return new DurationInMinutes(null);
            }
            long operand1 = d1 == null ? 0L : d1.MinutesOrZero;
            long operand2 = d2 == null ? 0L : d2.MinutesOrZero;
            return new DurationInMinutes(operatorFunc(operand1, operand2));
        }
    }
}
