using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Resto.Framework.Windows
{
    public static partial class WinApi
    {
        // ReSharper disable InconsistentNaming
        internal const string KERNEL32 = "kernel32.dll";
        internal const string USER32 = "user32.dll";
        internal const string ADVAPI32 = "advapi32.dll";
        internal const string OLE32 = "ole32.dll";
        internal const string OLEAUT32 = "oleaut32.dll";
        internal const string SHFOLDER = "shfolder.dll";
        internal const string SHIM = "mscoree.dll";
        internal const string CRYPT32 = "crypt32.dll";
        internal const string SECUR32 = "secur32.dll";


        internal const uint DUPLICATE_CLOSE_SOURCE = 0x00000001;
        internal const uint DUPLICATE_SAME_ACCESS = 0x00000002;
        internal const uint DUPLICATE_SAME_ATTRIBUTES = 0x00000004;
        // ReSharper restore InconsistentNaming

        // Error codes from WinError.h
        internal enum WinError
        {
            // ReSharper disable InconsistentNaming
            ERROR_SUCCESS = 0x0,
            ERROR_INVALID_FUNCTION = 0x1,
            ERROR_FILE_NOT_FOUND = 0x2,
            ERROR_PATH_NOT_FOUND = 0x3,
            ERROR_ACCESS_DENIED = 0x5,
            ERROR_INVALID_HANDLE = 0x6,
            ERROR_NOT_ENOUGH_MEMORY = 0x8,
            ERROR_INVALID_DATA = 0xd,
            ERROR_INVALID_DRIVE = 0xf,
            ERROR_NO_MORE_FILES = 0x12,
            ERROR_NOT_READY = 0x15,
            ERROR_BAD_LENGTH = 0x18,
            ERROR_SHARING_VIOLATION = 0x20,
            ERROR_NOT_SUPPORTED = 0x32,
            ERROR_FILE_EXISTS = 0x50,
            ERROR_INVALID_PARAMETER = 0x57,
            ERROR_CALL_NOT_IMPLEMENTED = 0x78,
            ERROR_INSUFFICIENT_BUFFER = 0x7A,
            ERROR_INVALID_NAME = 0x7B,
            ERROR_BAD_PATHNAME = 0xA1,
            ERROR_ALREADY_EXISTS = 0xB7,
            ERROR_ENVVAR_NOT_FOUND = 0xCB,
            ERROR_FILENAME_EXCED_RANGE = 0xCE, // filename too long. 
            ERROR_NO_DATA = 0xE8,
            ERROR_PIPE_NOT_CONNECTED = 0xE9,
            ERROR_MORE_DATA = 0xEA,
            ERROR_DIRECTORY = 0x10B,
            ERROR_OPERATION_ABORTED = 0x3E3, // 995; For IO Cancellation
            ERROR_NO_TOKEN = 0x3f0,
            ERROR_DLL_INIT_FAILED = 0x45A,
            ERROR_NON_ACCOUNT_SID = 0x4E9,
            ERROR_NOT_ALL_ASSIGNED = 0x514,
            ERROR_UNKNOWN_REVISION = 0x519,
            ERROR_INVALID_OWNER = 0x51B,
            ERROR_INVALID_PRIMARY_GROUP = 0x51C,
            ERROR_NO_SUCH_PRIVILEGE = 0x521,
            ERROR_PRIVILEGE_NOT_HELD = 0x522,
            ERROR_NONE_MAPPED = 0x534,
            ERROR_INVALID_ACL = 0x538,
            ERROR_INVALID_SID = 0x539,
            ERROR_INVALID_SECURITY_DESCR = 0x53A,
            ERROR_BAD_IMPERSONATION_LEVEL = 0x542,
            ERROR_CANT_OPEN_ANONYMOUS = 0x543,
            ERROR_NO_SECURITY_ON_OBJECT = 0x546,
            ERROR_TRUSTED_RELATIONSHIP_FAILURE = 0x6FD,
            // ReSharper restore InconsistentNaming
        }

        /// <summary>
        /// The TOKEN_INFORMATION_CLASS enumeration type contains values that 
        /// specify the type of information being assigned to or retrieved from 
        /// an access token.
        /// </summary>
        internal enum TokenInformationClass : uint
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUiAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        /// <summary>
        /// The SECURITY_IMPERSONATION_LEVEL enumeration type contains values 
        /// that specify security impersonation levels. Security impersonation 
        /// levels govern the degree to which a server process can act on behalf 
        /// of a client process.
        /// </summary>
        public enum SecurityImpersonationLevel
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        /// <summary>
        /// The TOKEN_ELEVATION_TYPE enumeration indicates the elevation type of 
        /// token being queried by the GetTokenInformation function or set by 
        /// the SetTokenInformation function.
        /// </summary>
        internal enum TokenElevationType
        {
            Default = 1,
            Full,
            Limited
        }

        /// <summary>
        /// The function creates a new access token that duplicates one 
        /// already in existence.
        /// </summary>
        /// <param name="existingTokenHandle">
        /// A handle to an access token opened with TOKEN_DUPLICATE access.
        /// </param>
        /// <param name="impersonationLevel">
        /// Specifies a SECURITY_IMPERSONATION_LEVEL enumerated type that 
        /// supplies the impersonation level of the new token.
        /// </param>
        /// <param name="duplicateTokenHandle">
        /// Outputs a handle to the duplicate token. 
        /// </param>
        /// <returns></returns>
        [DllImport(ADVAPI32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateToken(
            SafeTokenHandle existingTokenHandle,
            SecurityImpersonationLevel impersonationLevel,
            out SafeTokenHandle duplicateTokenHandle);

        [DllImport(KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
        [ResourceExposure(ResourceScope.Machine)]
        internal static extern
        bool DuplicateHandle(
            [In] IntPtr hSourceProcessHandle,
            [In] IntPtr hSourceHandle,
            [In] IntPtr hTargetProcessHandle,
            [In, Out] ref SafeTokenHandle lpTargetHandle,
            [In] uint dwDesiredAccess,
            [In] bool bInheritHandle,
            [In] uint dwOptions);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport(KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
        [ResourceExposure(ResourceScope.Machine)]
        internal static extern
        bool DuplicateHandle(
            [In] IntPtr hSourceProcessHandle,
            [In] SafeTokenHandle hSourceHandle,
            [In] IntPtr hTargetProcessHandle,
            [In, Out] ref SafeTokenHandle lpTargetHandle,
            [In] uint dwDesiredAccess,
            [In] bool bInheritHandle,
            [In] uint dwOptions);


        [DllImport(ADVAPI32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern
        bool GetTokenInformation(
            [In] IntPtr tokenHandle,
            [In] TokenInformationClass tokenInformationClass,
            [In] SafeLocalAllocHandle tokenInformation,
            [In] uint tokenInformationLength,
            [Out] out uint returnLength);

        [DllImport(ADVAPI32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern
        bool GetTokenInformation(
            [In] SafeTokenHandle tokenHandle,
            [In] TokenInformationClass tokenInformationClass,
            [In] SafeLocalAllocHandle tokenInformation,
            [In] uint tokenInformationLength,
            [Out] out uint returnLength);

        [SecurityCritical]
        public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeTokenHandle()
                : base(true)
            {
            }

            internal SafeTokenHandle(IntPtr handle)
                : base(true)
            {
                SetHandle(handle);
            }

            public static SafeTokenHandle FromToken(IntPtr token)
            {
                if (token == IntPtr.Zero)
                {
                    throw new ArgumentException("token");
                }

                // Find out if the specified token is a valid.
                var dwLength = (uint) Marshal.SizeOf(typeof (uint));
                var result = GetTokenInformation(token, TokenInformationClass.TokenType,
                    SafeLocalAllocHandle.InvalidHandle, 0, out dwLength);
                var error = (WinError) Marshal.GetLastWin32Error();
                if (error == WinError.ERROR_INVALID_HANDLE)
                {
                    throw new Win32Exception();
                }

                using (var process = Process.GetCurrentProcess())
                {
                    var processHandle = process.Handle;
                    var safeToken = InvalidHandle;
                    if (!DuplicateHandle(processHandle,
                        token,
                        processHandle,
                        ref safeToken,
                        0,
                        true,
                        DUPLICATE_SAME_ACCESS))
                    {
                        throw new SecurityException(GetLastErrorMessage());
                    }

                    return safeToken;
                }
            }

            internal static SafeTokenHandle InvalidHandle
            {
                get { return new SafeTokenHandle(IntPtr.Zero); }
            }

            protected override bool ReleaseHandle()
            {
                return CloseHandle(handle);
            }
        }

        [SecurityCritical]
        public sealed class SafeLocalAllocHandle : SafeBuffer
        {
            private SafeLocalAllocHandle() : base(true)
            {
            }

            // 0 is an Invalid Handle
            internal SafeLocalAllocHandle(IntPtr handle)
                : base(true)
            {
                SetHandle(handle);
            }

            internal static SafeLocalAllocHandle InvalidHandle
            {
                get { return new SafeLocalAllocHandle(IntPtr.Zero); }
            }

            [SecurityCritical]
            protected override bool ReleaseHandle()
            {
                return LocalFree(handle) == IntPtr.Zero;
            }
        }
    }

    internal static class SafeTokenHandleExtensions
    {

        [SecurityCritical]
        internal static WinApi.SafeLocalAllocHandle GetTokenInformation(
            this WinApi.SafeTokenHandle tokenHandle,
            WinApi.TokenInformationClass tokenInformationClass)
        {
            var dwLength = (uint) Marshal.SizeOf(typeof (uint));

            WinApi.GetTokenInformation(tokenHandle,
                tokenInformationClass,
                WinApi.SafeLocalAllocHandle.InvalidHandle,
                0,
                out dwLength);

            var dwErrorCode = (WinApi.WinError) Marshal.GetLastWin32Error();
            switch (dwErrorCode)
            {
                case WinApi.WinError.ERROR_BAD_LENGTH:
                // special case for TokenSessionId. Falling through
                case WinApi.WinError.ERROR_INSUFFICIENT_BUFFER:
                    // ptrLength is an [In] param to LocalAlloc

                    var safeLocalAllocHandle =
                        new WinApi.SafeLocalAllocHandle(Marshal.AllocHGlobal((int) dwLength));
                    if (safeLocalAllocHandle.IsInvalid)
                        throw new OutOfMemoryException();

                    safeLocalAllocHandle.Initialize(dwLength);

                    var result = WinApi.GetTokenInformation(tokenHandle,
                        tokenInformationClass,
                        safeLocalAllocHandle,
                        dwLength,
                        out dwLength);
                    if (!result)
                    {
                        throw new SecurityException(WinApi.GetLastErrorMessage());
                    }

                    return safeLocalAllocHandle;

                case WinApi.WinError.ERROR_INVALID_HANDLE:
                    throw new ArgumentException(WinApi.GetLastErrorMessage());
                default:
                    throw new SecurityException(WinApi.GetLastErrorMessage());
            }
        }
    }
}