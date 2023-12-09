using System;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// The DLL characteristics of the image.
    /// </summary>
    [Flags]
    public enum DllCharacteristicsType : ushort
    {
        /// <summary>
        /// The DLL can be relocated at load time.
        /// </summary>
        DynamicBase = 0x0040,

        /// <summary>
        /// Code integrity checks are forced.
        /// If you set this flag and a section contains only uninitialized data, set the PointerToRawData member of IMAGE_SECTION_HEADER for that section to zero;
        /// otherwise, the image will fail to load because the digital signature cannot be verified.
        /// </summary>
        ForceIntegrity = 0x0080,

        /// <summary>
        /// The image is compatible with data execution prevention (DEP).
        /// </summary>
        NxCompat = 0x0100,

        /// <summary>
        /// The image is isolation aware, but should not be isolated.
        /// </summary>
        NoIsolation = 0x0200,

        /// <summary>
        /// The image does not use structured exception handling (SEH). No handlers can be called in this image.
        /// </summary>
        NoSeh = 0x0400,

        /// <summary>
        /// Do not bind the image.
        /// </summary>
        NoBind = 0x0800,

        /// <summary>
        /// A WDM driver.
        /// </summary>
        WdmDriver = 0x2000,

        /// <summary>
        /// The image is terminal server aware.
        /// </summary>
        TerminalServerAware = 0x8000
    }
}
