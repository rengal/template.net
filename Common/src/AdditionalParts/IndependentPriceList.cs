using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    /// <summary>
    /// Бэковское дополнение для работы с классом IndependentPriceList.
    /// </summary>
    public partial class IndependentPriceList
    {
        /// <summary>
        /// Фиктивный ключ записи внутреннего прайс-листа.
        /// Пока внутренний прайс-лист один, но может потребоваться разделение по подразделениям.
        /// </summary>
        private static readonly IndependentPriceListKey KEY_EMPTY = new IndependentPriceListKey(Guid.Empty);

        public IndependentPriceList(Guid id, DateTime? dateFrom, DateTime? dateTo)
            : base(id, dateFrom, dateTo)
        {
            key = KEY_EMPTY;
        }
    }
}