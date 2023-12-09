using System.Runtime.InteropServices;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// Represents the COFF header format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageFileHeader
    {
        /// <summary>
        /// “PE” in reversed byte order.
        /// </summary>
        public const uint ExpectedSignature = 0x4550;

        /// <summary>
        /// The architecture type of the computer.
        /// An image file can only be run on the specified computer or a system that emulates the specified computer.
        /// </summary>
        [PublicAPI]
        public readonly ArchitectureType Machine;

        /// <summary>
        /// The number of sections.
        /// This indicates the size of the section table, which immediately follows the headers.
        /// Note that the Windows loader limits the number of sections to 96.
        /// </summary>
        [PublicAPI]
        public readonly ushort NumberOfSections;

        /// <summary>
        /// The low 32 bits of the time stamp of the image.
        /// This represents the date and time the image was created by the linker.
        /// The value is represented in the number of seconds elapsed since midnight (00:00:00), January 1, 1970, Universal Coordinated Time, according to the system clock.
        /// </summary>
        [PublicAPI]
        public readonly uint TimeDateStamp;

        /// <summary>
        /// The offset of the symbol table, in bytes, or zero if no COFF symbol table exists.
        /// </summary>
        [PublicAPI]
        public readonly uint PointerToSymbolTable;

        /// <summary>
        /// The number of symbols in the symbol table.
        /// </summary>
        [PublicAPI]
        public readonly uint NumberOfSymbols;

        /// <summary>
        /// The size of the optional header, in bytes. This value should be 0 for object files.
        /// </summary>
        [PublicAPI]
        public readonly ushort SizeOfOptionalHeader;

        /// <summary>
        /// The characteristics of the image. This member can be one or more of the following values.
        /// </summary>
        [PublicAPI]
        public readonly PeCharacteristics Characteristics;
    }
}
