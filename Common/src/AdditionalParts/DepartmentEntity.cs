using Resto.Common.Properties;
using Resto.Framework.Data;
using Resto.UI.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common;

namespace Resto.Data
{
    public partial class DepartmentEntity : IComparable, IWithOperationalDaySettings
    {
        public static string ALL_DEPARTMENT_ENTITIES = Resources.DepartmentEntityAllDepartments;

        #region Properties

        /// <summary>
        /// Возвращает true, если текущий пользователь привязан к данному департамеенту.
        /// </summary>
        public bool IsAssignedToCurrentUser
        {
            get
            {
                User user = ServerSession.CurrentSession.GetCurrentUser();
                return user.AssignedToDepartments != null && user.AssignedToDepartments.Any(d => d.Id == Id)
                       || user.System;
            }
        }

        /// <summary>
        /// Возвращает true, если данное ТП лежит в списке ТП-ий где является ответственным текущий пользователь.
        /// </summary>
        public bool CurrentUserIsResponsible
        {
            get
            {
                User currentUser = ServerSession.CurrentSession.GetCurrentUser();

                return !Deleted && currentUser != null && (currentUser.ResponsibilityDepartments == null ||
                       currentUser.ResponsibilityDepartments.Contains(this));
            }
        }

        /// <summary>
        /// Возвращает true, если данное ТП лежит одновременно в списке ТП, где текущий пользователь является ответственным и
        /// в списке ТП, под которыми он работает в текущей сессии.
        /// </summary>
        /// <param name="includeDeleted">Если <c>true</c>, то принадлежность проверяется и для удалённого ТП,
        /// иначе для удалённого ТП возвращается <c>false</c>, даже если оно принадлежит обоим упомянутым множествам.</param>
        public bool IsWorkedForCurrentUser(bool includeDeleted)
        {
            return (includeDeleted || !Deleted) && MultiDepartments.Instance.GetWorkingDepartments(includeDeleted).Contains(this);
        }

        /// <summary>
        /// Проверяет существуют ли в текущем <see cref="DepartmentEntity"/> настройки подключения к ЕГАИС
        /// и доступны ли они из текущего режима
        /// </summary>
        public bool IsАvailableEgaisConnectionsSettings
        {
            get
            {
                return EgaisConnectionsSettings != null &&
                       EgaisConnectionsSettings.RmsManagedConnection == CompanySetup.IsRMS;
            }
        }

        /// <summary>
        /// Может ли данное ТП использоваться в версиях тех. карт.
        /// </summary>
        public bool CanHaveStoreSpecification => this is Department || this is Manufacture;

        #endregion Properties

        #region Public methods

        public bool HasStores()
        {
            foreach (Store store in EntityManager.INSTANCE.GetAllNotDeleted<Store>())
            {
                var curEntity = store.NpeParent;

                if (curEntity == null)
                {
                    continue;
                }
                while (true)
                {
                    if (curEntity.Id.Equals(Id))
                    {
                        return true;
                    }
                    if (curEntity.Parent == null)
                    {
                        break;
                    }
                    curEntity = curEntity.Parent;
                }
            }
            return false;
        }

        public List<Store> GetDepartmentStoresByHierarchy()
        {
            var stores = new List<Store>();
            foreach (Store store in EntityManager.INSTANCE.GetAllNotDeleted<Store>())
            {
                var curEntity = store.NpeParent;

                if (curEntity == null)
                {
                    continue;
                }
                while (true)
                {
                    if (curEntity.Id.Equals(Id))
                    {
                        stores.Add(store);
                        break;
                    }
                    if (curEntity.Parent == null)
                    {
                        break;
                    }
                    curEntity = curEntity.Parent;
                }
            }
            return stores;
        }

        public JurPerson GetJurPerson()
        {
            var parent = Parent;
            while (parent != null)
            {
                var jurPerson = parent as JurPerson;
                if (jurPerson != null)
                {
                    return jurPerson;
                }
                parent = parent.Parent;
            }

            return null;
        }
        #endregion Public methods

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var department = obj as DepartmentEntity;
            return department != null ? Comparer<string>.Default.Compare(NameLocal, department.NameLocal) : -1;
        }

        #endregion IComparable Members

        #region Object Members

        public override string ToString()
        {
            return NameLocal;
        }

        #endregion Object Members

        #region Schedule Methods

        public LocalTimeInterval GetWorkDayInterval(DateTime date)
        {
            if (scheduleInternal == null || scheduleInternal.WeekScheduleInfo == null || 
                scheduleInternal.WeekScheduleInfo.Days == null || scheduleInternal.CustomScheduleType == CustomScheduleType.ROUND_THE_CLOCK)
            {
                return new LocalTimeInterval(TimeSpan.FromDays(0), TimeSpan.FromDays(1));
            }

            if (scheduleInternal.CustomScheduleType == CustomScheduleType.SAME_EVERY_DAY)
            {
                return scheduleInternal.LocalTimeInterval;
            }

            var result = new LocalTimeInterval(TimeSpan.FromDays(0), TimeSpan.FromDays(1));
            var weekDay = WeekDays.FromDayOfWeek(date.DayOfWeek);
            List<LocalTimeInterval> weekDayIntervals;
            if (!scheduleInternal.WeekScheduleInfo.Days.TryGetValue(weekDay, out weekDayIntervals) || weekDayIntervals == null)
            {
                return result;
            }

            LocalTimeInterval unionIntervals = null;
            foreach (var interval in weekDayIntervals)
            {
                unionIntervals = GetSumIntervals(unionIntervals, interval);
            }

            if (unionIntervals != null)
            {
                result.Begin = unionIntervals.Begin;
                result.End = unionIntervals.Begin > unionIntervals.End ? unionIntervals.End.Add(TimeSpan.FromDays(1)) : unionIntervals.End;
            }

            return result;
        }

        /// <summary>
        /// Возвращает суммарный интервал. В отличии от объединения возвращает максимальный интервал с учетом первого и второго.
        /// 
        /// Пример 1:
        /// first:  10:00 - 17:00
        /// second: 15:00 - 02:00
        /// result: 10:00 - 02:00
        /// 
        /// Пример 2:
        /// first:  05:00 - 12:00
        /// second: 15:00 - 03:00
        /// result: 05:00 - 03:00
        /// 
        /// </summary>
        private LocalTimeInterval GetSumIntervals(LocalTimeInterval first, LocalTimeInterval second)
        {
            if (first == null)
            {
                return second ?? null;
            }
            else if (second == null)
            {
                return first;
            }

            var firstBegin = first.Begin;
            var firstEnd = firstBegin > first.End ? first.End.Add(TimeSpan.FromDays(1)) : first.End;

            var secondBegin = second.Begin;
            var secondEnd = secondBegin > second.End ? second.End.Add(TimeSpan.FromDays(1)) : second.End;

            var resultBegin = firstBegin < secondBegin ? firstBegin : secondBegin;
            var resultEnd = firstEnd > secondEnd ? firstEnd : secondEnd;

            if (resultEnd >= TimeSpan.FromDays(1))
            {
                resultEnd = resultEnd.Subtract(TimeSpan.FromDays(1));
            }
            return new LocalTimeInterval(resultBegin, resultEnd);
        }

        #endregion
    }    
}
