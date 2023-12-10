using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Описывает рекорд Грида.
    /// </summary>
    public interface IRestoGridRow
    {
        /// <summary>
        /// Если true, то рекорд пустой.
        /// </summary>
        bool IsEmptyRecord { get; }
    }
}
