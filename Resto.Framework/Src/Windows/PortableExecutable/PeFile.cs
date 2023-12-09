using System;
using System.IO;
using System.Runtime.InteropServices;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// Вспомогательный класс для работы со структурой файлов формата Portable Executable (dll, exe, sys).
    /// </summary>
    /// <remarks>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms680547(v=vs.85).aspx
    /// </remarks>
    public sealed class PeFile
    {
        private readonly byte[] bytes;
        private readonly ImageDosHeader dosHeader;
        private readonly ImageFileHeader ntHeader;
        private readonly Either<ImageOptionalHeader32, ImageOptionalHeader64> optionalHeader;
        private readonly uint optionalHeaderOffset;

        /// <summary>
        /// Загрузить файл и разобрать его структуру.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArithmeticException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public PeFile([NotNull] string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            bytes = File.ReadAllBytes(filePath);

            dosHeader = ReadBlock<ImageDosHeader>(0);
            CheckDosHeader();

            var peSignature = ReadBlock<uint>(dosHeader.NewHeaderOffset);
            CheckPeSignature(peSignature);

            var ntHeaderOffset = dosHeader.NewHeaderOffset + (uint)Marshal.SizeOf(peSignature);
            ntHeader = ReadBlock<ImageFileHeader>(ntHeaderOffset);

            optionalHeaderOffset = ntHeaderOffset + (uint)Marshal.SizeOf(ntHeader);
            var optionalHeaderFormat = (MagicType)ReadBlock<ushort>(optionalHeaderOffset); // первые два байта (ushort) определяют выбор 32/64-битного заголовка
            optionalHeader = optionalHeaderFormat == MagicType.Nt32BitImage
                ? Either<ImageOptionalHeader32, ImageOptionalHeader64>.CreateLeft(ReadBlock<ImageOptionalHeader32>(optionalHeaderOffset))
                : Either<ImageOptionalHeader32, ImageOptionalHeader64>.CreateRight(ReadBlock<ImageOptionalHeader64>(optionalHeaderOffset));
        }

        public uint Size
        {
            get { return checked((uint)bytes.Length); }
        }

        [NotNull]
        public byte[] Bytes
        {
            get { return bytes; }
        }

        [PublicAPI]
        public ImageDosHeader DosHeader
        {
            get { return dosHeader; }
        }

        public Either<ImageOptionalHeader32, ImageOptionalHeader64> OptionalHeader
        {
            get { return optionalHeader; }
        }

        [PublicAPI]
        public ImageFileHeader NtHeader
        {
            get { return ntHeader; }
        }

        public uint OptionalHeaderOffset
        {
            get { return optionalHeaderOffset; }
        }

        public T ReadBlock<T>(uint offset)
            where T : struct
        {
            var blockSize = Marshal.SizeOf<T>();
            if (offset + blockSize > Size)
                throw new InvalidOperationException(string.Format("Cannot read {0} ({1} bytes) starting from {2} within file of length {3}.", typeof(T).Name, blockSize, offset, Size));

            var pinnedHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStructure<T>(Marshal.UnsafeAddrOfPinnedArrayElement(bytes, checked((int)offset)));
            }
            finally
            {
                pinnedHandle.Free();
            }
        }

        [AssertionMethod]
        private void CheckDosHeader()
        {
            if (dosHeader.Signature != ImageDosHeader.ExpectedSignature)
                throw new InvalidOperationException(string.Format("File doesn't have DOS's magic prefix, there is {0} instead", dosHeader.Signature));
            if (dosHeader.NewHeaderOffset > Size)
                throw new InvalidOperationException("New header offset exceeds the file's length");
        }

        [AssertionMethod]
        private static void CheckPeSignature(uint peSignature)
        {
            if (peSignature != ImageFileHeader.ExpectedSignature)
                throw new InvalidOperationException(string.Format("File doesn't have PE's magic prefix, there is {0} instead", peSignature));
        }
    }
}
