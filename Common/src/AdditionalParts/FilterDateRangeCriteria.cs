using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class FilterDateRangeCriteria
    {
        public DateTime DateFrom
        {
            get { return (DateTime)From; }
        }

        public DateTime DateTo
        {
            get { return (DateTime)To; }
        }

        public FilterDateRangeCriteria(IComparable dateFrom, IComparable dateTo)
            : base(dateFrom, dateTo)
        {
        }

        /// <summary>
        /// Конструктор, формирующий FilterDateRangeCriteria в том виде, в котором он нужен серверу
        /// (с dateFrom 00:00 по dateTo+1 00:00 исключая правую границу периода)
        /// </summary>
        /// <param name="datePeriod"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public FilterDateRangeCriteria(DatePeriod datePeriod, DateTime dateFrom, DateTime dateTo)
            : this(dateFrom.Date, dateTo.Date.AddDays(1), datePeriod ?? DatePeriod.Custom)
        {
            IncludeHigh = false;
        }
    }
}
