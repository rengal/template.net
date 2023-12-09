using System.Runtime.InteropServices;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDosHeader
    {
        /// <summary>
        /// “MZ” in reversed byte order.
        /// </summary>
        public const ushort ExpectedSignature = 0x5A4D;

        /// <summary>
        /// Magic number.
        /// </summary>
        [PublicAPI]
        public readonly ushort Signature;

        /// <summary>
        /// Bytes on last page of file.
        /// </summary>
        [PublicAPI]
        public readonly ushort LastPageBytesCount;

        /// <summary>
        /// Pages in file.
        /// </summary>
        [PublicAPI]
        public readonly ushort PagesCount;

        /// <summary>
        /// Relocations.
        /// </summary>
        [PublicAPI]
        public readonly ushort RelocationsCount;

        /// <summary>
        /// Size of header in paragraphs.
        /// </summary>
        [PublicAPI]
        public readonly ushort HeaderParagraphsCount;

        /// <summary>
        /// Minimum extra paragraphs needed.
        /// </summary>
        [PublicAPI]
        public readonly ushort MinAlloc;

        /// <summary>
        /// Maximum extra paragraphs needed.
        /// </summary>
        [PublicAPI]
        public readonly ushort MaxAlloc;

        /// <summary>
        /// Initial (relative) SS value.
        /// </summary>
        [PublicAPI]
        public readonly ushort SS;

        /// <summary>
        /// Initial SP value.
        /// </summary>
        [PublicAPI]
        public readonly ushort SP;

        /// <summary>
        /// Checksum.
        /// </summary>
        [PublicAPI]
        public ushort Checksum;

        /// <summary>
        /// Initial IP value
        /// </summary>
        [PublicAPI]
        public readonly ushort IP;

        /// <summary>
        /// Initial (relative) CS value
        /// </summary>
        [PublicAPI]
        public readonly ushort CS;

        /// <summary>
        /// File address of relocation table
        /// </summary>
        [PublicAPI]
        public readonly ushort RelocationTableOffset;

        /// <summary>
        /// Overlay number.
        /// </summary>
        [PublicAPI]
        public ushort OverlayNumber;

        /// <summary>
        /// Reserved words.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private readonly ushort[] Reserved;

        /// <summary>
        /// OEM identifier.
        /// </summary>
        [PublicAPI]
        public readonly ushort OemId;

        /// <summary>
        /// OEM information.
        /// </summary>
        [PublicAPI]
        public readonly ushort OemInfo;

        /// <summary>
        /// Reserved words.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        private readonly ushort[] Reserved2;

        /// <summary>
        /// File address of new exe header.
        /// </summary>
        [PublicAPI]
        public readonly uint NewHeaderOffset;
    }
}
