using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Principal;
using Resto.Framework.Attributes.JetBrains;
using log4net.Util;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using Resto.Framework.Windows;

namespace Resto.Framework.Common
{
    public class AuthenticationHelper
    {
        public static WindowsIdentity LogonUser(string userName, string domain, SecureString password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var unsecurePassword = IntPtr.Zero;
            try
            {
                if (password != null)
                {
                    unsecurePassword = Marshal.SecureStringToGlobalAllocUnicode(password);
                }

                IntPtr token;
                if (WinApi.LogonUser(userName, domain, unsecurePassword, WinApi.LogonType.Logon32LogonInteractive, WinApi.LogonProvider.Logon32ProviderDefault, out token))
                {
                    return new WindowsIdentity(token);
                }
            }
            finally
            {
                if (unsecurePassword != IntPtr.Zero)
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unsecurePassword);
                }
            }

            return null;
        }

        public static void CloseToken(WindowsIdentity identity)
        {
            if (identity == null || identity.Token == IntPtr.Zero) return;
            WinApi.CloseHandle(identity.Token);
            identity.Dispose();
        }

        public static bool IsUserCredentialsValid(WindowsIdentity identity)
        {
            return identity != null && identity.IsAuthenticated;
        }

        public static bool IsCurrentUser(WindowsIdentity identity)
        {
            var currentIdentity = WindowsIdentity.GetCurrent();
            if (currentIdentity == null || identity == null)
            {
                return false;
            }
            return currentIdentity.User.Equals(identity.User);
        }

        /// <summary>
        /// Проверяет, что заданный пользователь входит в группу локальных администраторов
        /// </summary>
        /// <param name="identity">объект <see cref="WindowsIdentity"/>, представляющий пользователя</param>
        /// <param name="checkElevatedPriviliges">разрешает проверку учетной записи с учетом повышенния привелегий</param>
        /// <returns><value>true</value> - пользователь входит вгруппу локальных админстраторов, иначе - <value>false</value></returns>
        /// <exception cref="Win32Exception">Ошибка при обращениии к Windows API</exception>
        /// <exception cref="SecurityException">У вызывающего кода нет достаточных прав</exception>
        public static bool IsUserInAdminRole([NotNull] WindowsIdentity identity, bool checkElevatedPriviliges = false)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            WinApi.SafeTokenHandle tokenToCheck = null;
            using (var safeToken = WinApi.SafeTokenHandle.FromToken(identity.Token))
            {
                if (Environment.OSVersion.Version.Major >= 6 && checkElevatedPriviliges)
                {
                    using (var elevationTypeInfo = safeToken.GetTokenInformation(WinApi.TokenInformationClass.TokenElevationType))
                    {
                        var elevationType = elevationTypeInfo.Read<WinApi.TokenElevationType>(0);

                        if (elevationType == WinApi.TokenElevationType.Limited)
                        {
                            using (
                                var linkedTokenInfo = safeToken.GetTokenInformation(WinApi.TokenInformationClass.TokenLinkedToken))
                            {
                                var pToken = linkedTokenInfo.Read<IntPtr>(0);
                                tokenToCheck = new WinApi.SafeTokenHandle(pToken);
                            }
                        }
                    }
                }

                // CheckTokenMembership requires an impersonation token. If we just got 
                // a linked token, it already is an impersonation token.  If we did not 
                // get a linked token, duplicate the original into an impersonation 
                // token for CheckTokenMembership.
                if (tokenToCheck == null)
                {
                    if (!WinApi.DuplicateToken(safeToken,
                        WinApi.SecurityImpersonationLevel.SecurityIdentification,
                        out tokenToCheck))
                    {
                        throw new Win32Exception();
                    }
                }
            }

            using (tokenToCheck)
            {
                var id = new WindowsIdentity(tokenToCheck.DangerousGetHandle());
                var principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);                
            }
        }

        /// <summary>
        /// Проверяет, что текущий пользователь входит в группу локальных администраторов
        /// </summary>
        /// <param name="checkElevatedPriviliges">разрешает проверку учетной записи с учетом повышенния привелегий</param>
        /// <returns><value>true</value> - пользователь входит вгруппу локальных админстраторов, иначе - <value>false</value></returns>
        /// <exception cref="Win32Exception">Ошибка при обращениии к Windows API</exception>
        /// <exception cref="SecurityException">У вызывающего кода нет достаточных прав</exception>
        public static bool IsCurrentUserInAdminRole(bool checkElevatedPriviliges = false)
        {
            return IsUserInAdminRole(WindowsIdentity.GetCurrent(), checkElevatedPriviliges);
        }
        
    }
}
