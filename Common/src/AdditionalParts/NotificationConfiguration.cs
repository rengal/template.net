using System;

namespace Resto.Data
{
    public partial class NotificationConfiguration
    {
        public Int32? GetDaysWithoutSalesLimits(DepartmentEntity dep)
        {
            return daysWithoutSalesLimits.ContainsKey(dep) ? daysWithoutSalesLimits[dep] : (int?)null;
        }

        public void UpdateDaysWithoutSalesLimits(DepartmentEntity dep, int? days)
        {
            if (!days.HasValue)
            {
                daysWithoutSalesLimits.Remove(dep);
            }
            else
            {
                daysWithoutSalesLimits[dep] = days.Value;
            }
        }
    }
}