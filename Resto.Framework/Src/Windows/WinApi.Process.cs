using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Resto.Framework.Attributes.JetBrains;
using Microsoft.Win32.SafeHandles;

namespace Resto.Framework.Windows
{
    public static partial class WinApi
    {
        private const int ErrorNoMoreFiles = 0x12;

        [Flags]
        public enum SnapshotFlags : uint
        {
            [PublicAPI]
            HeapList = 0x00000001,
            [PublicAPI]
            Process = 0x00000002,
            [PublicAPI]
            Thread = 0x00000004,
            [PublicAPI]
            Module = 0x00000008,
            [PublicAPI]
            Module32 = 0x00000010,
            [PublicAPI]
            All = (HeapList | Process | Thread | Module),
            [PublicAPI]
            Inherit = 0x80000000,
            [PublicAPI]
            NoHeaps = 0x40000000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessEntry32
        {
            [PublicAPI]
            public uint dwSize;
            [PublicAPI]
            public uint cntUsage;
            [PublicAPI]
            public uint th32ProcessID;
            [PublicAPI]
            public IntPtr th32DefaultHeapID;
            [PublicAPI]
            public uint th32ModuleID;
            [PublicAPI]
            public uint cntThreads;
            [PublicAPI]
            public uint th32ParentProcessID;
            [PublicAPI]
            public int pcPriClassBase;
            [PublicAPI]
            public uint dwFlags;
            [PublicAPI]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        };

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeSnapshotHandle CreateToolhelp32Snapshot(SnapshotFlags flags, uint id);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Process32First(SafeSnapshotHandle hSnapshot, ref ProcessEntry32 lppe);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Process32Next(SafeSnapshotHandle hSnapshot, ref ProcessEntry32 lppe);

        [PublicAPI]
        [SuppressUnmanagedCodeSecurity, HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
        private sealed class SafeSnapshotHandle : SafeHandleMinusOneIsInvalid
        {
            internal SafeSnapshotHandle()
                : base(true)
            { }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            internal SafeSnapshotHandle(IntPtr handle)
                : base(true)
            {
                SetHandle(handle);
            }

            protected override bool ReleaseHandle()
            {
                return CloseHandle(handle);
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            private static extern bool CloseHandle(IntPtr handle);
        }

        /// <exception cref="Win32Exception" />
        public static int ParentProcessId(this Process process)
        {
            return ParentProcessId(process.Id);
        }

        /// <exception cref="Win32Exception" />
        private static int ParentProcessId(int id)
        {
            var pe32 = new ProcessEntry32
            {
                dwSize = (uint)Marshal.SizeOf(typeof(ProcessEntry32))
            };
            using (var hSnapshot = CreateToolhelp32Snapshot(SnapshotFlags.Process, (uint)id))
            {
                if (hSnapshot.IsInvalid)
                    throw new Win32Exception();

                if (!Process32First(hSnapshot, ref pe32))
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    if (errorCode == ErrorNoMoreFiles)
                        return -1;
                    throw new Win32Exception(errorCode);
                }
                do
                {
                    if (pe32.th32ProcessID == (uint)id)
                        return (int)pe32.th32ParentProcessID;
                } while (Process32Next(hSnapshot, ref pe32));
            }
            return -1;
        }
    }
}
