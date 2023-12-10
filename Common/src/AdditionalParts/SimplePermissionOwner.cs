using System;
using Resto.Framework.Common;

namespace Resto.Data
{
    public abstract partial class SimplePermissionOwner
    {
        public string ChildName
        {
            get
            {
                if (this is User)
                {
                    return ((User)this).NameLocal;
                }
                if (this is Role)
                {
                    return ((Role)this).NameLocal;
                }
                return String.Empty;
            }
        }

        public override string ToString()
        {
            return ChildName;
        }

        private bool checkState(Permission permission, PermissionState state)
        {
            return Permissions != null && Permissions.ContainsKey(permission) &&
                   Permissions[permission] == state;
        }

        internal bool Allowed(Permission permission)
        {
            return administrator || checkState(permission, PermissionState.ALLOW);
        }

        internal bool AllowedWhereResponsible(Permission permission)
        {
            return
                administrator || checkState(permission, PermissionState.ALLOW) ||
                checkState(permission, PermissionState.WHERE_RESPONSIBLE);
        }

        internal bool Denied(Permission permission)
        {
            return !administrator && checkState(permission, PermissionState.DENY);
        }

        protected static void checkWhereResponsiblePermission(Permission permission)
        {
            if (!permission.ResponsiblePermission)
            {
                throw new RestoException(permission + " isn't responsible permission");
            }
        }
    }
}