using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    partial class IncentiveProgram : IMultiDepartmentable<Department>
    {
        public ICollection<Department> DepartmentsCollection
        {
            get { return Departments; }
        }
    }
}
