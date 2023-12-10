using System.Linq;
using EnumerableExtensions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;
using Resto.UI.Common;

namespace Resto.Data
{
    public partial class StoreSpecification
    {
        public void SetInverse(bool inverse)
        {
            // Ничего не делаем, если значение не поменялось
            if (Inverse == inverse)
            {
                return;
            }

            Inverse = inverse;

            // Инвертируем списки предприятий
            foreach (var departmentEntity in MultiDepartments.Instance.NotDeletedDepartments)
            {
                if (departmentEntity is Department || departmentEntity is Manufacture)
                {
                    if (Departments.Contains(departmentEntity))
                    {
                        Departments.Remove(departmentEntity);
                    }
                    else
                    {
                        Departments.Add(departmentEntity);
                    }
                }
            }
        }

        /// <summary>
        /// Копирование спецификации
        /// </summary>
        public StoreSpecification Copy()
        {
            var newSpecification = new StoreSpecification(Inverse);
            Departments.ForEach(dep => newSpecification.Departments.Add(dep));

            return newSpecification;
        }

        public static bool IsAssemblyChartItemVisible([CanBeNull] StoreSpecification storeSpecification, [CanBeNull] DepartmentEntity department, bool isAll,
            bool isMain)
        {
            if (isAll || storeSpecification == null)
            {
                return true;
            }

            if (isMain)
            {
                return storeSpecification.inverse;
            }

            return storeSpecification.IsSatisfiedBy(department);
        }

        public bool IsSatisfiedBy([CanBeNull] DepartmentEntity department)
        {
            return Inverse != Departments.Contains(department);
        }
    }
}