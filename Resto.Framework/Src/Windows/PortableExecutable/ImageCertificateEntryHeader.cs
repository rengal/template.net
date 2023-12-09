using System.Runtime.InteropServices;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// This structure encapsulates a signature used in verifying executable files.
    /// </summary>
    /// <remarks>
    /// Исходная структура <c>WIN_CERTIFICATE</c> имеет переменную длину за счёт расположенного в конце структуры массива байт,
    /// где содержатся один или несколько сертификатов указанного типа.
    /// В .Net не поддерживаются структуры переменной длины.
    /// Вместо самого массива в конце структуры можно поместить ссылку на массив, тогда структура будет иметь постоянный размер,
    /// но массив окажется где-то в другом месте памяти, и маршалить такую структуру не получится из-за того,
    /// что маршаллинг массива как заинлайненного (<see cref="UnmanagedType.ByValArray"/>) требует фиксированного размера (<see cref="MarshalAsAttribute.SizeConst"/>),
    /// нельзя объяснить маршаллеру, что размер массива определяется динамически как разница между <see cref="Length"/> и суммой размеров первых трёх полей.
    /// Так что отдельно маршалим первые три поля в <see cref="ImageCertificateEntryHeader"/>,
    /// затем, зная размер массива, отдельно маршалим массив, и потом объединяем их в <see cref="ImageCertificateEntry"/>.
    /// Альтернатива — ручной маршалинг, муторно.
    /// </remarks>
    /// <seealso cref="ImageCertificateEntry"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageCertificateEntryHeader
    {
        //public static uint Size 
        /// <summary>
        /// Specifies the length of the attribute certificate entry (including variable-length byte array, which is not presented here).
        /// </summary>
        [PublicAPI]
        public readonly uint Length;

        /// <summary>
        /// Specifies the certificate revision.
        /// </summary>
        [PublicAPI]
        public readonly ushort Revision;

        /// <summary>
        /// Specifies the type of certificate.
        /// </summary>
        [PublicAPI]
        public readonly CertificateType CertificateType;

        public ImageCertificateEntryHeader(uint length, ushort revision, CertificateType certificateType)
        {
            Length = length;
            Revision = revision;
            CertificateType = certificateType;
        }
    }
}
