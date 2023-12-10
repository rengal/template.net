using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    /// <summary>
    /// Интерфейс для любых сущностей, имеющих отношение к нескольким подразделениям.
    /// </summary>
    /// <typeparam name="TDepartment">Тип подразделений.</typeparam>
    public interface IMultiDepartmentable<TDepartment> where TDepartment : DepartmentEntity
    {
        ICollection<TDepartment> DepartmentsCollection { get; }
    }
}
