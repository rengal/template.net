using System;
using System.Runtime.InteropServices;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.Helpers;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// Represents the optional header format in a 64-bit application.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1), PublicAPI]
    public struct ImageOptionalHeader64
    {
        public static readonly uint ChecksumOffset = MarshallingHelper.OffsetOf<ImageOptionalHeader64>(x => x.Checksum);
        public static readonly uint CertificateTableOffset = MarshallingHelper.OffsetOf<ImageOptionalHeader64>(x => x.CertificateTable);

        /// <summary>
        /// The state of the image file.
        /// </summary>
        [PublicAPI]
        public readonly MagicType Magic;

        /// <summary>
        /// The major version number of the linker.
        /// </summary>
        [PublicAPI]
        public readonly byte MajorLinkerVersion;

        /// <summary>
        /// The minor version number of the linker.
        /// </summary>
        [PublicAPI]
        public readonly byte MinorLinkerVersion;

        /// <summary>
        /// The size of the code section, in bytes, or the sum of all such sections if there are multiple code sections.
        /// </summary>
        [PublicAPI]
        public readonly uint SizeOfCode;

        /// <summary>
        /// The size of the initialized data section, in bytes, or the sum of all such sections if there are multiple initialized data sections.
        /// </summary>
        [PublicAPI]
        public readonly uint SizeOfInitializedData;

        /// <summary>
        /// The size of the uninitialized data section, in bytes, or the sum of all such sections if there are multiple uninitialized data sections.
        /// </summary>
        [PublicAPI]
        public readonly uint SizeOfUninitializedData;

        /// <summary>
        /// A pointer to the entry point function, relative to the image base address. For executable files, this is the starting address.
        /// For device drivers, this is the address of the initialization function. The entry point function is optional for DLLs.
        /// When no entry point is present, this member is zero.
        /// </summary>
        [PublicAPI]
        public readonly uint AddressOfEntryPoint;

        /// <summary>
        /// A pointer to the beginning of the code section, relative to the image base.
        /// </summary>
        [PublicAPI]
        public readonly uint BaseOfCode;

        /// <summary>
        /// The preferred address of the first byte of the image when it is loaded in memory.
        /// This value is a multiple of 64K bytes. The default value for DLLs is 0x10000000.
        /// The default value for applications is 0x00400000, except on Windows CE where it is 0x00010000.
        /// </summary>
        [PublicAPI]
        public readonly ulong ImageBase;

        /// <summary>
        /// The alignment of sections loaded in memory, in bytes. This value must be greater than or equal to the FileAlignment member.
        /// The default value is the page size for the system.
        /// </summary>
        [PublicAPI]
        public readonly uint SectionAlignment;

        /// <summary>
        /// The alignment of the raw data of sections in the image file, in bytes.
        /// The value should be a power of 2 between 512 and 64K (inclusive). The default is 512.
        /// If the SectionAlignment member is less than the system page size, this member must be the same as SectionAlignment.
        /// </summary>
        [PublicAPI]
        public readonly uint FileAlignment;

        /// <summary>
        /// The major version number of the required operating system.
        /// </summary>
        [PublicAPI]
        public readonly ushort MajorOperatingSystemVersion;

        /// <summary>
        /// The minor version number of the required operating system.
        /// </summary>
        [PublicAPI]
        public readonly ushort MinorOperatingSystemVersion;

        /// <summary>
        /// The major version number of the image.
        /// </summary>
        [PublicAPI]
        public readonly ushort MajorImageVersion;

        /// <summary>
        /// The minor version number of the image.
        /// </summary>
        [PublicAPI]
        public readonly ushort MinorImageVersion;

        /// <summary>
        /// The major version number of the subsystem.
        /// </summary>
        [PublicAPI]
        public readonly ushort MajorSubsystemVersion;

        /// <summary>
        /// The minor version number of the subsystem.
        /// </summary>
        [PublicAPI]
        public readonly ushort MinorSubsystemVersion;

        /// <summary>
        /// This member is reserved and must be 0.
        /// </summary>
        [PublicAPI]
        public readonly uint Win32VersionValue;

        /// <summary>
        /// The size of the image, in bytes, including all headers. Must be a multiple of SectionAlignment.
        /// </summary>
        [PublicAPI]
        public readonly uint SizeOfImage;

        /// <summary>
        /// The combined size of the following items, rounded to a multiple of the value specified in the FileAlignment member.
        /// <list type="bullet">
        /// <item>e_lfanew member of IMAGE_DOS_HEADER</item>
        /// <item>4 byte signature</item>
        /// <item>size of IMAGE_FILE_HEADER</item>
        /// <item>size of optional header</item>
        /// <item>size of all section headers</item>
        /// </list>
        /// </summary>
        [PublicAPI]
        public readonly uint SizeOfHeaders;

        /// <summary>
        /// The image file checksum. The following files are validated at load time: all drivers, any DLL loaded at boot time, and any DLL loaded into a critical system process.
        /// </summary>
        [PublicAPI]
        public readonly uint Checksum;

        /// <summary>
        /// The subsystem required to run this image. The following values are defined.
        /// </summary>
        [PublicAPI]
        public readonly SubsystemType Subsystem;

        /// <summary>
        /// The DLL characteristics of the image.
        /// </summary>
        [PublicAPI]
        public readonly DllCharacteristicsType DllCharacteristics;

        /// <summary>
        /// The number of bytes to reserve for the stack.
        /// Only the memory specified by the <see cref="SizeOfStackCommit"/> member is committed at load time;
        /// the rest is made available one page at a time until this reserve size is reached.
        /// </summary>
        [PublicAPI]
        public readonly ulong SizeOfStackReserve;

        /// <summary>
        /// The number of bytes to commit for the stack.
        /// </summary>
        [PublicAPI]
        public readonly ulong SizeOfStackCommit;

        /// <summary>
        /// The number of bytes to reserve for the local heap.
        /// Only the memory specified by the <see cref="SizeOfHeapCommit"/> member is committed at load time;
        /// the rest is made available one page at a time until this reserve size is reached.
        /// </summary>
        [PublicAPI]
        public readonly ulong SizeOfHeapReserve;

        /// <summary>
        /// The number of bytes to commit for the local heap.
        /// </summary>
        [PublicAPI]
        public readonly ulong SizeOfHeapCommit;

        /// <summary>
        /// This member is obsolete.
        /// </summary>
        [PublicAPI, Obsolete]
        public readonly uint LoaderFlags;

        /// <summary>
        /// The number of directory entries in the remainder of the optional header.
        /// Each entry describes a location and size.
        /// </summary>
        [PublicAPI]
        public readonly uint NumberOfRvaAndSizes;

        [PublicAPI]
        public readonly ImageDataDirectory ExportTable;

        [PublicAPI]
        public readonly ImageDataDirectory ImportTable;

        [PublicAPI]
        public readonly ImageDataDirectory ResourceTable;

        [PublicAPI]
        public readonly ImageDataDirectory ExceptionTable;

        [PublicAPI]
        public readonly ImageDataDirectory CertificateTable;

        [PublicAPI]
        public readonly ImageDataDirectory BaseRelocationTable;

        [PublicAPI]
        public readonly ImageDataDirectory Debug;

        [PublicAPI]
        public readonly ImageDataDirectory Architecture;

        [PublicAPI]
        public readonly ImageDataDirectory GlobalPtr;

        [PublicAPI]
        public readonly ImageDataDirectory TLSTable;

        [PublicAPI]
        public readonly ImageDataDirectory LoadConfigTable;

        [PublicAPI]
        public readonly ImageDataDirectory BoundImport;

        [PublicAPI]
        public readonly ImageDataDirectory IAT;

        [PublicAPI]
        public readonly ImageDataDirectory DelayImportDescriptor;

        [PublicAPI]
        public readonly ImageDataDirectory CLRRuntimeHeader;

        [PublicAPI]
        public readonly ImageDataDirectory Reserved;
    }
}
