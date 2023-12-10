using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    public partial class ServiceFee
    {
        /// <summary>
        /// Проверяет, присутствует ли в сервисном сборе хотя бы одна пара скидка-услуга,
        /// не доступная хотя бы одному общему ТП.
        /// </summary>
        public bool HasEntryWithVisibilityMismatch()
        {
            return ServiceByDiscountType.Any(mapEntry => !mapEntry.Value.IsVisibleForAnyDepartment(mapEntry.Key.Departments));
        }
    }
}
