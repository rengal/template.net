using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using Resto.Framework.Attributes.JetBrains;
using Microsoft.Win32.SafeHandles;

namespace Resto.Framework.Common
{
    public static class Win32KernelObjectsHelper
    {
        private const string GlobalNamespacePrefix = "Global\\";

        [NotNull]
        public static string GetKernelObjectName([NotNull] string name, bool visibleForAllTerminalSessions)
        {
            if (name.Contains("\\"))
            {
                throw new ArgumentException("name");
            }
            return visibleForAllTerminalSessions ? GlobalNamespacePrefix + name : name;
        }

        [DllImport("kernel32.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern SafeWaitHandle OpenMutex(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        public static bool MutexExists(string mutexName, bool findInAllSessions)
        {
            using (SafeHandle mutexHandle = OpenMutex(
                (uint)MutexRights.Synchronize,
                false,
                GetKernelObjectName(mutexName, findInAllSessions)))
            {
                return !mutexHandle.IsInvalid;
            }
        }

        public static bool MutexExists(string mutexName)
        {
            return MutexExists(mutexName, true);
        }

        public static MutexSecurity CreateFullAccessMutexSecurity()
        {
            // делаем для мьютекса полные права доступа для всех авторизованных пользователей
            var mutexSecurity = new MutexSecurity();

            var sidOwner = new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null);
            var sidAuthUser = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

            var rule = new MutexAccessRule(sidOwner, MutexRights.FullControl, AccessControlType.Allow);
            mutexSecurity.AddAccessRule(rule);

            rule = new MutexAccessRule(sidAuthUser, MutexRights.FullControl, AccessControlType.Allow);
            mutexSecurity.AddAccessRule(rule);

            return mutexSecurity;
        }

    }
}