using System;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class EmployeeAttendanceEntry
    {
        public EmployeeAttendanceEntry(Guid id, DateTime? dateFrom, DateTime? dateTo, User employee, DepartmentEntity department)
            : base(id, dateFrom, dateTo, employee, department, department)
        {
            // salaryDepartment заполняется сервером
        }

        public bool IsClosed
        {
            get
            {
                return (PersonalSessionStart != null && PersonalSessionEnd != null) || DateTo != null;
            }

        }

        /// <summary>
        /// Возвращает true, если была коррекция табельных данных.
        /// </summary>
        public bool IsCorrected
        {
            get
            {
                bool isCorrected = false;

                // Если фронтовые данные НЕ пусты и учетные данные НЕ совпадают (время явки бека отличается от фронта).
                if (personalSessionStart.HasValue && personalSessionEnd.HasValue &&
                    (personalSessionStart.Value != DateFrom.GetValueOrFakeDefault() || personalSessionEnd.Value != DateTo.GetValueOrFakeDefault()))
                {
                    isCorrected = true;
                }
                // Если фронтовые данные ПУСТЫ и тип явки в беке - «явка».
                else if (!personalSessionStart.HasValue && !personalSessionEnd.HasValue &&
                         AttendanceType != null && AttendanceType.Status.GetValueOrFakeDefault())
                {
                    isCorrected = true;
                }
                return isCorrected;
            }
        }

        /// <summary>
        /// true: Явка зарегистрирована в подразделении отличном от подразделения начисления оплат
        /// </summary>
        public bool IsOtherDepartment
        {
            get
            {
                if (SalaryDepartment == null)
                {
                    // Подразделение оплаты не назначено
                    return false;
                }

                return !Equals(SalaryDepartment, Department);
            }
        }

        public bool MoreOneDay
        {
            get
            {
                if (DateFrom == null)
                {
                    throw new Exception("Expected limited interval by DateFrom");
                }

                return (DateTo ?? DateTime.Now) > DateFrom.Value.AddDays(1);
            }
        }
    }
}