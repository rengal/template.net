using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    public static class PeUtils
    {
        /// <summary>
        /// Выравнивание записей <see cref="ImageCertificateEntryHeader"/> в таблице сертификатов.
        /// Если запись имеет длину, некратную восьми байтам, она дополняется нулями до ближайшего кратного размера.
        /// Это значит, что если, например, очередная запись имеет размер 795 байт, то за ней последуют 5 нулей для выравнивания адреса начала следующей записи.
        /// </summary>
        private const uint ImageCertificateEntryAlignment = 8;

        public static readonly HashAlgorithmName DefaultHashAlgorithmName = HashAlgorithmName.SHA256;
        public static readonly RSASignaturePadding DefaultRsaSignaturePadding = RSASignaturePadding.Pkcs1;

        [NotNull, Pure]
        public static IReadOnlyList<ImageCertificateEntry> ReadCertificates([NotNull] this PeFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var certificateTable = file.OptionalHeader.Case(x86 => x86.CertificateTable, x64 => x64.CertificateTable);
            var tableOffset = certificateTable.VirtualAddress;
            if (tableOffset == 0)
                return Array.Empty<ImageCertificateEntry>();

            var entryHeaderSize = Marshal.SizeOf<ImageCertificateEntryHeader>();
            var result = new List<ImageCertificateEntry>();
            var currentEntryOffset = tableOffset;
            while (currentEntryOffset < tableOffset + certificateTable.Size)
            {
                var header = file.ReadBlock<ImageCertificateEntryHeader>(currentEntryOffset);
                var certificate = new ArraySegment<byte>(file.Bytes, (int)currentEntryOffset + entryHeaderSize, (int)header.Length - entryHeaderSize);
                result.Add(new ImageCertificateEntry(header, certificate));

                var alignedEntryLength = header.Length % ImageCertificateEntryAlignment == 0
                    ? header.Length
                    : header.Length + (ImageCertificateEntryAlignment - (header.Length % ImageCertificateEntryAlignment));
                currentEntryOffset += alignedEntryLength;
            }

            return result;
        }

        /// <summary>
        /// Возвращает часть PE-файла, которая не изменится при добавлении/удалении цифровой подписи.
        /// </summary>
        [NotNull, Pure]
        public static byte[] GetHashableContent([NotNull] this PeFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var checksumOffset = file.OptionalHeader.Case(x86 => ImageOptionalHeader32.ChecksumOffset, x64 => ImageOptionalHeader64.ChecksumOffset);
            var certificateTableOffset = file.OptionalHeader.Case(x86 => ImageOptionalHeader32.CertificateTableOffset, x64 => ImageOptionalHeader64.CertificateTableOffset);
            var certificateTable = file.OptionalHeader.Case(x86 => x86.CertificateTable, x64 => x64.CertificateTable);

            // хеш для цифровой подписи считаем по всему файлу кроме следующих участков, которые могут меняться при добавлении сертификатов:
            var excludedSegments = new List<ArraySegment<byte>>
            {
                // контрольная сумма файла — она вычисляется по всему файлу, включая сертификаты
                new ArraySegment<byte>(file.Bytes, (int) (file.OptionalHeaderOffset + checksumOffset), sizeof (uint)),

                // заголовок таблицы сертификатов, где указаны смещение и размер этой таблицы (смещение меняется только при добавлении первого сертификата, размер всегда)
                new ArraySegment<byte>(file.Bytes, (int) (file.OptionalHeaderOffset + certificateTableOffset), Marshal.SizeOf<ImageDataDirectory>()), 
            };
            // сама таблица сертификатов, пропускаем её целиком, включая другие сертификаты, чтобы не полагаться на порядок подписывания
            if (certificateTable.Size != 0)
                excludedSegments.Add(new ArraySegment<byte>(file.Bytes, (int)certificateTable.VirtualAddress, (int)certificateTable.Size));

            // всё кроме перечисленных выше сегментов склеиваем и возвращаем
            var includedSegments = GetArraySegmentsExceptOf(file.Bytes, excludedSegments);
            return ConcatArraySegments(includedSegments.ToList());
        }

        /// <summary>
        /// «Вычесть» из массива указанные сегменты.
        /// </summary>
        /// <param name="source">Исходный массив.</param>
        /// <param name="excludedSegments">Непересекающиеся сегменты исходного массива, упорядоченные по смещению.</param>
        private static IEnumerable<ArraySegment<T>> GetArraySegmentsExceptOf<T>([NotNull] T[] source, [NotNull] IReadOnlyCollection<ArraySegment<T>> excludedSegments)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (excludedSegments == null)
                throw new ArgumentNullException(nameof(excludedSegments));

            Debug.Assert(excludedSegments.All(segment => segment.Array == source));
            Debug.Assert(excludedSegments.OrderBy(segment => segment.Offset).SequenceEqual(excludedSegments));
            Debug.Assert(excludedSegments.Zip(excludedSegments.Skip(1), (prev, next) => prev.Offset + prev.Count <= next.Offset).All(nonOverlapping => nonOverlapping));

            var offset = 0;
            foreach (var excludedSegment in excludedSegments)
            {
                var includedSegmentLength = excludedSegment.Offset - offset;
                if (includedSegmentLength > 0)
                    yield return new ArraySegment<T>(source, offset, includedSegmentLength);

                offset = excludedSegment.Offset + excludedSegment.Count;
            }

            if (offset < source.Length)
                yield return new ArraySegment<T>(source, offset, source.Length - offset);
        }

        [NotNull, Pure]
        private static T[] ConcatArraySegments<T>([NotNull] IReadOnlyCollection<ArraySegment<T>> segments)
        {
            if (segments == null)
                throw new ArgumentNullException(nameof(segments));

            var result = new T[segments.Sum(segment => segment.Count)];
            var destinationIndex = 0;
            foreach (var segment in segments)
            {
                Array.Copy(segment.Array, segment.Offset, result, destinationIndex, segment.Count);
                destinationIndex += segment.Count;
            }
            return result;
        }
    }
}
