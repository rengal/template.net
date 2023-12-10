using System.Linq;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.UI.Common;

namespace Resto.Data
{
    public partial class RemovalType
    {
        public bool Writeoff
        {
            get { return account != null; }
        }

        /// <summary>
        /// Строковое представление списка подразделений, для которых предназначен тип оплаты.
        /// Возвращает только активные подразделения.
        /// </summary>
        public string DepartmentList
        {
            get
            {
                var workingDepartments = MultiDepartments.Instance.WorkingDepartments.ToList();
                return Departments != null
                    ? Departments
                        .Intersect(workingDepartments)
                        .Select(department => department.NameLocal)
                        .JoinWithComma()
                    : Department.ALL_DEPARTMENTS;
            }
        }

        public bool IsUsedInFlyerDiscount
        {
            get
            {
                return EntityManager.INSTANCE.GetAll<FlyerDiscountType>().FirstOrDefault(f => f.RemovalType == this) != null;
            }
        }

        public override string ToString()
        {
            return NameLocal;
        }
    }
}