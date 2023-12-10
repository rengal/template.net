using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    public partial class AbstractKeyedTimeTableItem
    {
        /// <summary>
        /// Возвращает <c>true</c>, если текущее время попадает в полуинтервал действия элемента.
        /// </summary>
        public bool IsActual
        {
            get
            {
                var now = DateTime.Now;
                return now >= (DateFrom ?? DateTimeServerConstants.MinDate) &&
                       now < (DateTo ?? DateTimeServerConstants.MaxDate);
            }
        }
    }
}
