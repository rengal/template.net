using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.Exceptions
{
    /// <summary>
    /// Исключение нарушения прав
    /// </summary>
    [Serializable]
    public sealed class PermissionViolationException : Exception
    {
        public PermissionViolationException([NotNull] Permission permissionViolated)
            : base($"User is violating permission {permissionViolated.Code}: {permissionViolated.LocalName}")
        {
            PermissionViolated = permissionViolated;
        }

        /// <summary>
        /// Нарушенное право
        /// </summary>
        public Permission PermissionViolated { get; }
    }
}
