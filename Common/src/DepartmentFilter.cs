using System;
using System.Linq;

namespace Resto.Data
{
    /// <summary>
    /// Фильтр по подразделениям - может быть как включающий так и исключающий
    /// </summary>
    public partial class DepartmentFilter
    {
        /// <summary>
        /// Возвращает true если подразделение удовлетворяет фильтру
        /// </summary>
        public bool IsSatisfiedBy(DepartmentEntity department)
        {
            return excluding ^ departments.Contains(department);
        }

        /// <summary>
        /// Возвращает true если подразделение удовлетворяет фильтру
        /// </summary>
        public bool IsSatisfiedBy(Guid departmentId)
        {
            return excluding ^ departments.Any(d => d.Id == departmentId);
        }
    }
}
