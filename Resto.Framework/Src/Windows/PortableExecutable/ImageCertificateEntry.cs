using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// This structure encapsulates a signature used in verifying executable files.
    /// </summary>
    /// <seealso cref="ImageCertificateEntryHeader"/>
    public sealed class ImageCertificateEntry
    {
        private readonly ushort revision;
        private readonly CertificateType certificateType;
        private readonly ArraySegment<byte> certificate;

        public ImageCertificateEntry(ImageCertificateEntryHeader header, ArraySegment<byte> certificate)
        {
            revision = header.Revision;
            certificateType = header.CertificateType;
            this.certificate = certificate;
        }

        /// <summary>
        /// Specifies the certificate revision.
        /// </summary>
        [PublicAPI]
        public ushort Revision
        {
            get { return revision; }
        }

        /// <summary>
        /// Specifies the type of certificate.
        /// </summary>
        [PublicAPI]
        public CertificateType CertificateType
        {
            get { return certificateType; }
        }

        /// <summary>
        /// An array of certificates.
        /// The format of this member depends on the value of <see cref="CertificateType"/>.
        /// </summary>
        public ArraySegment<byte> Certificate
        {
            get { return certificate; }
        }
    }
}
