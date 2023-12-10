﻿using System;
using System.Collections.Generic;
﻿using System.Linq;

namespace Resto.Data
{
    /// <summary>
    /// Должность сотрудника.
    /// </summary>
    public partial class Role : IComparable<Role>, IComparable
    {
        public bool HasPermission(Permission permission)
        {
            return Permissions.ContainsKey(permission) && Permissions[permission] == PermissionState.ALLOW;
        }

        public bool HasPermissionWhereResponsible(Permission permission)
        {
            return AllowedWhereResponsible(permission);
        }

        /// <summary>
        /// Возвращает true, если тип графика должности = "Оклад".
        /// </summary>
        public bool IsSteady
        {
            get { return scheduleType == RoleScheduleType.STEADY_SALARY; }
        }

        public int CompareTo(Role other)
        {
            if (other == null)
                return 1;
            if (ReferenceEquals(this, other) || Equals(other))
            {
                return 0;
            }

            int res = StringComparer.CurrentCulture.Compare(NameLocal, other.NameLocal);
            if (res == 0)
                res = Comparer<Guid>.Default.Compare(Id, other.Id);
            return res;
        }

        public int CompareTo(object obj)
        {
            var other = obj as Role;
            if (other == null)
                return 1;
            return CompareTo(other);
        }

        public override string ToString()
        {
            return NameLocal;
        }

        /// <summary>
        /// Для роли "Системный администратор" свойство Permissions возвращает пустое множество. Данный метод позволяет получить
        /// действительные значения прав (все права) для роли "Системный администратор"; для других ролей возвращается свойство Permissions
        /// </summary>
        /// <param name="allPermissions">Множество всех существующих в системе привилегий</param>
        public IDictionary<Permission, PermissionState> GetActualPermissions(IEnumerable<Permission> allPermissions)
        {
            return Administrator
                ? allPermissions.ToDictionary(item => item, item => PermissionState.ALLOW)
                : Permissions;
        }
    }
}
