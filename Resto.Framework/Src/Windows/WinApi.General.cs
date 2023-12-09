using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using Microsoft.Win32.SafeHandles;
using Resto.Framework.Attributes.JetBrains;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Resto.Framework.Windows
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static partial class WinApi
    {
        #region Const

        // ReSharper disable InconsistentNaming

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        public enum Messages
        {
            WM_TIMER = 0x113,
            WM_KEYFIRST = 0x0100,
            WM_KEYDOWN = 0x0100,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
            WM_KEYUP = 0x0101,
            WM_CHAR = 0x0102,
            WM_KEYLAST = 0x0109,
            WM_INPUT = 0x00FF,
            WM_DEVICE_CHANGE = 0x0219,
        }

        public const int WH_KEYBOARD = 2;
        public const int WH_KEYBOARD_LL = 13;
        public const int PM_NOREMOVE = 0x0000;
        public const int PM_REMOVE = 0x0001;

        public const int LLKHF_INJECTED = 0x10;

        // ReSharper restore InconsistentNaming

        public const uint FileFlagOverlapped = 0x40000000;
        public const int ErrorIoPending = 997;
        public const uint WaitObject0 = 0x00000000;
        public const uint WaitTimeout = 0x00000102;
        private const uint FormatMessageAllocateBuffer = 0x00000100;
        private const uint FormatMessageIgnoreInserts = 0x00000200;
        private const uint FormatMessageFromSystem = 0x00001000;
        private const uint FormatMessageArgumentArray = 0x00002000;
        private const uint FormatMessageFromHModule = 0x00000800;
        private const uint FormatMessageFromString = 0x00000400;

        #endregion

        #region Struct

        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr handle;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point p;
        }

        /// <summary>
        /// Defines information for the raw input devices.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RawInputDevice
        {
            /// <summary>
            /// Top level collection Usage page for the raw input device.
            /// </summary>
            public ushort UsagePage;

            /// <summary>
            /// Top level collection Usage for the raw input device.
            /// </summary>
            public HidUsage Usage;

            /// <summary>
            /// Mode flag that specifies how to interpret the information provided by <see cref="UsagePage"/>UsagePage and <see cref="Usage"/>
            /// </summary>
            public RawInputDeviceFlags Flags;

            /// <summary>
            /// A handle to the target window.
            /// </summary>
            public IntPtr Target;
        }

        /// <summary>
        /// Contains information about a raw input device.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RawInputDeviceList
        {
            /// <summary>
            /// A handle to the raw input device.
            /// </summary>
            public IntPtr HeaderPtr;

            /// <summary>
            /// The type of device.
            /// </summary>
            public RawInputDeviceType Type;
        }

        /// <summary>
        /// The raw input data.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct RawData
        {
            /// <summary>
            /// If the data comes from a mouse, this is the raw input data.
            /// </summary>
            [FieldOffset(0)] public RawMouse Mouse;

            /// <summary>
            /// If the data comes from a keyboard, this is the raw input data.
            /// </summary>
            [FieldOffset(0)] public RawKeyboard Keyboard;

            /// <summary>
            /// If the data comes from an HID, this is the raw input data.
            /// </summary>
            [FieldOffset(0)] public RawHid Hid;
        }

        /// <summary>
        /// Contains the raw input from a device.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct NativeRawInput
        {
            /// <summary>
            /// Header information that is part of the raw input data.
            /// </summary>
            public RawInputHeader Header;

            /// <summary>
            /// The raw input data.
            /// </summary>
            public RawData Data;
        }

        /// <summary>
        /// Contains the header information that is part of the raw input data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RawInputHeader
        {
            /// <summary>
            /// The type of raw input.
            /// </summary>
            public RawInputDeviceType Type;

            /// <summary>
            /// The size, in bytes, of the entire input packet of data.
            /// </summary>
            public uint Size;

            /// <summary>
            /// A handle to the device generating the raw input data.
            /// </summary>
            public IntPtr Device;

            /// <summary>
            /// The value passed in the wParam parameter of the WM_INPUT message.
            /// </summary>
            public IntPtr wParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RawHid
        {
            public uint SizeHid;
            public uint Count;
            public byte RawData;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct RawMouse
        {
            [FieldOffset(0)] public ushort Flags;
            [FieldOffset(4)] public uint Buttons;
            [FieldOffset(4)] public ushort ButtonFlags;
            [FieldOffset(6)] public ushort ButtonData;
            [FieldOffset(8)] public uint RawButtons;
            [FieldOffset(12)] public int LastX;
            [FieldOffset(16)] public int LastY;
            [FieldOffset(20)] public uint ExtraInformation;
        }

        /// <summary>
        /// Contains information about the state of the keyboard.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RawKeyboard
        {
            /// <summary>
            /// The scan code from the key depression.
            /// </summary>
            public ushort MakeCode;

            /// <summary>
            /// Flags for scan code information.
            /// </summary>
            public RawInputScanCodeFlags Flags;

            /// <summary>
            /// Reserved; must be zero.
            /// </summary>
            public ushort Reserved;

            /// <summary>
            /// Windows message compatible virtual-key code.
            /// </summary>
            public VirtualKeyCode VKey;

            /// <summary>
            /// The corresponding window message
            /// </summary>
            public uint Message;

            /// <summary>
            /// The device-specific additional information for the event.
            /// </summary>
            public uint ExtraInformation;
        }

        #endregion

        #region Enum

        /// <summary>
        /// Mode flag that specifies how to interpret the information provided by usUsagePage and usUsage
        /// </summary>
        [Flags]
        public enum RawInputDeviceFlags : uint
        {
            /// <summary>
            /// Enables the caller to receive the input even when the caller is not in the foreground. 
            /// </summary>
            RidevInputsink = 0x00000100,
        }

        /// <summary>
        /// Flags for scan code information
        /// </summary>
        [Flags]
        public enum RawInputScanCodeFlags : ushort
        {
            /// <summary>
            /// The key is up.
            /// </summary>
            RiKeyBreak = 1,

            /// <summary>
            /// This is the left version of the key.
            /// </summary>
            RiKeyE0 = 2,

            /// <summary>
            /// Extended
            /// </summary>
            RiKeyE1 = 4,
        }

        /// <summary>
        /// Specifies what data will be returned 
        /// </summary>
        public enum RawInputDeviceInfo : uint
        {
            /// <summary>
            /// String that contains the device name will be returned
            /// </summary>
            RidiDeviceName = 0x20000007,
        }

        /// <summary>
        /// The command flag
        /// </summary>
        public enum RawInputCommand : uint
        {
            /// <summary>
            /// Get the raw data from the RAWINPUT structure.
            /// </summary>
            RidInput = 0x10000003,
        }

        /// <summary>
        /// The type of device.
        /// </summary>
        [Flags]
        public enum RawInputDeviceType
        {
            /// <summary>
            /// The device is a mouse.
            /// </summary>
            RimTypeMouse = 0,

            /// <summary>
            /// The device is a keyboard.
            /// </summary>
            RimTypeKeyboard = 1,

            /// <summary>
            /// The device is an HID that is not a keyboard and not a mouse.
            /// </summary>
            RimTypeHid = 2,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SendInputStruct
        {
            public uint Type;
            public SendInputUnion InputUnion;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct SendInputUnion
        {
            [FieldOffset(0)]
            public MouseInput MouseInput;

            [FieldOffset(0)]
            public KeyboardInput KeyboardInput;

            [FieldOffset(0)]
            public HardwareInput HardwareInput;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardInput
        {
            public VirtualKeyCode VirtualCode;
            public ushort ScanCode;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public uint Msg;
            public ushort wParamL;
            public ushort wParamH;
        }

        /// <summary>
        /// Device type for SendInput
        /// </summary>
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public enum SendInputType : uint
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        /// <summary>
        /// SendInput Flags
        /// </summary>
        [Flags]
        public enum SendInputFlags : uint
        {
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008,
        }

        public enum MapType : uint
        {
            VkToVsc = 0x00,
            VscToVk = 0x01,
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public enum Keys
        {

            VkShift = 0x10,
            VkControl = 0x11,
            VkReturn = 0x0D,
            VkOemPlus = 0xBB,
            VkOemComma = 0xBC,
            VkOemPeriod = 0xBE,
            VkOem2 = 0xBF
        }

        public enum ScanCode : ushort
        {
            Control = 0x1D,
            Menu = 0x38,
            LShift = 0x2A,
            RShift = 0x36
        }

        public enum VirtualKeyCode : ushort
        {
            LButton = 0x01,
            MouseMin = LButton,
            Cancel = 0x03,
            XButton2 = 0x06,
            MouseMax = XButton2,
            Oem07 = 0x07,
            OemBackspace = 0x08,
            KeyTab = 0x09,
            Shift = 0x10,
            Oem0A = 0x0A,
            Oem0B = 0x0B,
            CR = 0x0D,
            Oem0E = 0x0E,
            Oem0F = 0x0F,
            Control = 0x11,
            Menu = 0x12,
            CapsLock = 0x14,
            VkEscape = 0x1B,
            Space = 0x20,
            PageUp = 0x21,
            PageDown = 0x22,
            KeyDelete = 0x2E,
            Key0 = 0x30,
            Key1 = 0x31,
            Key2 = 0x32,
            Key3 = 0x33,
            Key4 = 0x34,
            Key5 = 0x35,
            Key6 = 0x36,
            Key7 = 0x37,
            Key8 = 0x38,
            Key9 = 0x39,
            OemMinus = 0xBD,
            OemPlus = 0xBB,
            OemBackslash = 0xDC,
            KeyA = 0x41,
            KeyB = 0x42,
            KeyC = 0x43,
            KeyD = 0x44,
            KeyE = 0x45,
            KeyF = 0x46,
            KeyG = 0x47,
            KeyH = 0x48,
            KeyI = 0x49,
            KeyJ = 0x4A,
            KeyK = 0x4B,
            KeyL = 0x4C,
            KeyM = 0x4D,
            KeyN = 0x4E,
            KeyO = 0x4F,
            KeyP = 0x50,
            KeyQ = 0x51,
            KeyR = 0x52,
            KeyS = 0x53,
            KeyT = 0x54,
            KeyU = 0x55,
            KeyV = 0x56,
            KeyW = 0x57,
            KeyX = 0x58,
            KeyY = 0x59,
            KeyZ = 0x5A,
            Numpad0 = 0x60,
            Numpad1 = 0x61,
            Numpad2 = 0x62,
            Numpad3 = 0x63,
            Numpad4 = 0x64,
            Numpad5 = 0x65,
            Numpad6 = 0x66,
            Numpad7 = 0x67,
            Numpad8 = 0x68,
            Numpad9 = 0x69,
            Add = 0x6B,
            Subtract = 0x6D,
            Decimal = 0x6E,
            Insert = 0x2D,
            End = 0x23,
            Down = 0x28,
            Left = 0x25,
            Clear = 0x0C,
            Right = 0x27,
            Home = 0x24,
            Up = 0x26,
            LWin = 0x5B,
            RWin = 0x5C,
            Multiply = 0x6A,
            Divide = 0x6F,
            F1 = 0x70,
            F2= 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            F13 = 0x7C,
            F14 = 0x7D,
            F15 = 0x7E,
            F16 = 0x7F,
            F17 = 0x80,
            F18 = 0x81,
            F19 = 0x82,
            F20 = 0x83,
            F21 = 0x84,
            F22 = 0x85,
            F23 = 0x86,
            F24 = 0x87,
            NumLock = 0x90,
            ScrollLock = 0x91,
            LShift = 0xA0,
            RShift = 0xA1,
            LControl = 0xA2,
            RControl = 0xA3,
            LMenu = 0xA4,
            RMenu = 0xA5,
            OemComma = 0xBC,
            OemPeriod = 0xBE,
            OemQuestion = 0xBF,
            OemSemicolumn = 0xBA,
            Oem3 = 0xC0,
            OemQuotes = 0xDE,
            OemOpenBrackets = 0xDB,
            OemCloseBrackets = 0xDD,
            OemE3 = 0xE3,
            OemE4 = 0xE4,
            OemE6 = 0xE6,
            Packet = 0xE7,
            OemE9 = 0xE9,
            OemEA = 0xEA,
            OemEB = 0xEB,
            OemEC = 0xEC,
            OemED = 0xED,
            OemEE = 0xEE,
            OemEF = 0xEF,
            OemF0 = 0xF0,
            OemF1 = 0xF1,
            OemF2 = 0xF2,
            OemF3 = 0xF3,
            OemF4 = 0xF4,
            OemF5 = 0xF5,
            None = 0xFF
        }

        public enum LogonType
        {
            /// <summary>
            /// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
            /// by a terminal server, remote shell, or similar process.
            /// This logon type has the additional expense of caching logon information for disconnected operations;
            /// therefore, it is inappropriate for some client/server applications,
            /// such as a mail server.
            /// </summary>
            Logon32LogonInteractive = 2,
        }

        public enum LogonProvider
        {
            /// <summary>
            /// Use the standard logon provider for the system.
            /// The default security provider is negotiate, unless you pass NULL for the domain name and the user name
            /// is not in UPN format. In this case, the default provider is NTLM.
            /// NOTE: Windows 2000/NT:   The default security provider is NTLM.
            /// </summary>
            Logon32ProviderDefault = 0,
        }

        /// <summary>
        /// Top level collection Usage page for the raw input device.
        /// </summary>
        public enum HidUsagePage : ushort
        {
            GenericDesktopPage = 0x01,
        }

        /// <summary>
        /// Top level collection Usage for the raw input device.
        /// </summary>
        public enum HidUsage : ushort
        {
            Mouse = 0x02,
            Keyboard = 0x06
        }

        public enum SystemMetric
        {
            SmRemoteSession = 0x1000
        }

        #endregion

        #region Private Methods

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LocalFree(IntPtr handle);

        #endregion

        #region Methods

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string fileName,
            FileAccess desiredAccess,
            FileShare shareMode,
            IntPtr securityAttributes,
            FileMode creationDisposition,
            uint flags,
            IntPtr templateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(
            SafeFileHandle fileHandle,
            byte[] buffer,
            uint numberOfBytesToWrite,
            out uint numberOfBytesWritten,
            [In] ref NativeOverlapped lpOverlapped);

        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(SafeFileHandle fileHandle,
            [Out] byte[] buffer,
            uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead,
            [In] ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateEvent(IntPtr eventAttributes, bool manualReset, bool initialState, string name);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CancelIo(SafeFileHandle fileHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern UInt32 WaitForSingleObject(IntPtr eventHandle, UInt32 timeout);

        public static string GetLastErrorMessage()
        {
            return new Win32Exception(Marshal.GetLastWin32Error()).Message;
        }

        [DllImport("advapi32.dll", SetLastError = true, EntryPoint = "LogonUserW", CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(
            string userName,
            string domain,
            [MarshalAs(UnmanagedType.SysInt)] IntPtr password,
            LogonType logonType,
            LogonProvider logonProvider,
            out IntPtr token);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetDllDirectory(string pathName);

        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(byte[] state);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int vKey);

        [DllImport("user32.dll", EntryPoint = "SetKeyboardState")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetKeyboardState(byte[] state);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint MapVirtualKeyEx(uint uCode, MapType uMapType, IntPtr layoutId);

        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ToAscii(uint virtualKeyCodeKey, uint scanCode, byte[] keyboardState, [Out] StringBuilder buffer, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ToUnicode(uint virtualKeyCodeKey, uint scanCode, byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder buffer,
            int bufferSize, uint flags);

        [DllImport("user32.dll")]
        public static extern int ToUnicodeEx(uint virtualKey, uint scanCode, byte[] keyStates,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder buffer, int bufferSize,
            uint flags, IntPtr keyboardLayoutId);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool RegisterRawInputDevices(RawInputDevice[] rawInputDevices, uint numDevices, uint size);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern uint GetRawInputDeviceList([Out] RawInputDeviceList[] rawInputDeviceList,
                                                        ref uint numberDevices, uint size);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern uint GetRawInputDeviceInfo(IntPtr device, RawInputDeviceInfo command,
                                                        [In, Out] StringBuilder data, ref uint size);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int GetRawInputData(IntPtr rawInput, RawInputCommand command, [Out] out NativeRawInput buffer,
                                                 [In, Out] ref int size, int sizeHeader);

       
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint length, SendInputStruct[] input, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetTimer")]
        public static extern IntPtr SetTimer(IntPtr hwnd, IntPtr idEvent, UInt32 elapse, IntPtr proc);

        [DllImport("user32.dll", EntryPoint = "KillTimer")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool KillTimer(IntPtr hwnd, IntPtr idEvent);

        #endregion
    }
}