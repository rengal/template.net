using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// The subsystem required to run an image.
    /// </summary>
    public enum SubsystemType : ushort
    {
        /// <summary>
        /// Unknown subsystem.
        /// </summary>
        [PublicAPI]
        Unknown = 0,

        /// <summary>
        /// No subsystem required (device drivers and native system processes).
        /// </summary>
        [PublicAPI]
        Native = 1,

        /// <summary>
        /// Windows graphical user interface (GUI) subsystem.
        /// </summary>
        [PublicAPI]
        WindowsGui = 2,

        /// <summary>
        /// Windows character-mode user interface (CUI) subsystem.
        /// </summary>
        [PublicAPI]
        WindowsCui = 3,

        /// <summary>
        /// OS/2 CUI subsystem.
        /// </summary>
        [PublicAPI]
        Os2Cui = 5,

        /// <summary>
        /// POSIX CUI subsystem.
        /// </summary>
        [PublicAPI]
        PosixCui = 7,

        /// <summary>
        /// Windows CE system.
        /// </summary>
        [PublicAPI]
        WindowsCeGui = 9,

        /// <summary>
        /// Extensible Firmware Interface (EFI) application.
        /// </summary>
        [PublicAPI]
        EfiApplication = 10,

        /// <summary>
        /// EFI driver with boot services.
        /// </summary>
        [PublicAPI]
        EfiBootServiceDriver = 11,

        /// <summary>
        /// EFI driver with run-time services.
        /// </summary>
        [PublicAPI]
        EfiRuntimeDriver = 12,

        /// <summary>
        /// EFI ROM image.
        /// </summary>
        [PublicAPI]
        EfiRom = 13,

        /// <summary>
        /// Xbox system.
        /// </summary>
        [PublicAPI]
        Xbox = 14,

        /// <summary>
        /// Boot application.
        /// </summary>
        [PublicAPI]
        WindowsBootApplication = 16
    }
}
