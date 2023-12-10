using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    /// <summary>
    /// Класс методов расширений для складов.
    /// </summary>
    public static class StoreExtentions
    {
        /// <summary>
        /// Возвращает список департаментов по привязанным складам.
        /// </summary>
        public static List<DepartmentEntity> AssignedDepartments(this IList<Store> records)
        {
            return records.Where(s => s != null).Select(s => s.GetDepartmentEntity()).Distinct().ToList();
        }

        /// <summary>
        /// Возвращает список складов перечисленных через запятую в виде строки.
        /// </summary>
        public static string AsString(this IEnumerable<Store> records)
        {
            return string.Join(", ", records.Select(s => s.NameLocal).ToArray());
        }
    }
}