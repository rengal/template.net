using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class EmployeesPlanningReport
    {
        #region Methods

        /// <summary>
        /// Проверяет интервал заданной смены на пересечение с другими сменами сотрудника и заявками на отгул
        /// </summary>
        /// <returns>true если смена ни скем не пересекается</returns>
        public bool IsFreeEmployeeScheduleItemInterval(EmployeeScheduleItem scheduleItem)
        {
            if (!IsEmployeeAvailable(scheduleItem.Employee, scheduleItem.DateFrom.GetValueOrFakeDefault(), scheduleItem.DateTo.GetValueOrFakeDefault()))
            {
                return false;
            }

            ICollection<EmployeeScheduleItem> scheduleItems;
            if (!employeeScheduleItems.TryGetValue(scheduleItem.Employee, out scheduleItems))
            {
                return true;
            }

            return !scheduleItems.Except(scheduleItem.AsSequence())
                                 .Any(item => item.DateFrom.GetValueOrFakeDefault() <= scheduleItem.DateFrom.GetValueOrFakeDefault() && item.DateTo.GetValueOrFakeDefault() >= scheduleItem.DateFrom.GetValueOrFakeDefault() ||
                                              item.DateFrom.GetValueOrFakeDefault() <= scheduleItem.DateTo.GetValueOrFakeDefault() && item.DateTo.GetValueOrFakeDefault() >= scheduleItem.DateTo.GetValueOrFakeDefault() ||
                                              item.DateFrom.GetValueOrFakeDefault() >= scheduleItem.DateFrom.GetValueOrFakeDefault() && item.DateTo.GetValueOrFakeDefault() <= scheduleItem.DateTo.GetValueOrFakeDefault());
        }

        /// <summary>
        /// Проверяет не прерывается ли интервал отгулами сотрудника
        /// </summary>
        /// <param name="employee">Сотрудник</param>
        /// <param name="dateFrom">Начало проверяемого интервала</param>
        /// <param name="dateTo">Конец проверяемого интервала</param>
        /// <returns>true - если проверяемый интервал не прерывается</returns>
        public bool IsEmployeeAvailable(User employee, DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom >= dateTo)
            {
                throw new ArgumentException();
            }
            
            Dictionary<DateTime, bool> intervals;
            if (!employeesTimeline.TryGetValue(employee, out intervals))
            {
                return true;
            }

            var firstPair = intervals.LastOrDefault(pair => pair.Key <= dateFrom);

            if (!firstPair.Value)
            {
                return false;
            }

            var lastPair = intervals.FirstOrDefault(pair => pair.Key >= dateTo);

            if (lastPair.Value)
            {
                return false;
            }

            return !intervals.Any(pair => pair.Key > firstPair.Key && pair.Key < lastPair.Key);
        }

        /// <summary>
        /// Возвращает интервалы отсутствия сотрудников
        /// </summary>
        public Dictionary<User, List<Interval<DateTime>>> GetEmployeeAbsenceIntervals()
        {
            var result = new Dictionary<User, List<Interval<DateTime>>>(employeesTimeline.Count);

            foreach (var pair in employeesTimeline)
            {
                var employeeAbsence = new List<Interval<DateTime>>();
                DateTime? start = null;
                foreach (var timePoint in pair.Value)
                {
                    if (!timePoint.Value && start == null)
                    {
                        start = timePoint.Key;
                    }
                    else if (start != null)
                    {
                        employeeAbsence.Add(new Interval<DateTime>(start.Value, timePoint.Key));
                        start = timePoint.Value ? null : (DateTime?)timePoint.Key;
                    }
                }

                if (employeeAbsence.Any())
                {
                    result.Add(pair.Key, employeeAbsence);
                }
            }

            return result;
        }

        #endregion
    }
}
