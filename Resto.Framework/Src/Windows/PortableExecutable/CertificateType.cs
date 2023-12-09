using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    public enum CertificateType : ushort
    {
        /// <summary>
        /// X.509 Certificate. Not Supported.
        /// </summary>
        [PublicAPI]
        X509 = 0x0001,

        /// <summary>
        /// PKCS#7 SignedData structure.
        /// </summary>
        [PublicAPI]
        PkcsSignedData = 0x0002,

        /// <summary>
        /// Reserved.
        /// </summary>
        [PublicAPI]
        Reserved = 0x0003,

        /// <summary>
        /// Terminal Server Protocol Stack Certificate signing. Not Supported
        /// </summary>
        [PublicAPI]
        TsStackSigned = 0x0004,

        /// <summary>
        /// Тип сертификата, используемый для подписывания фронтовых плагинов.
        /// </summary>
        /// <remarks>
        /// 0xCAFE не имеет никакого специального смысла, здесь могло быть любое незанятое двухбайтовое значение.
        /// Просто определённый код, по которому мы узнаем свой сертификат среди других (другие будут <see cref="PkcsSignedData"/>).
        /// </remarks>
        [PublicAPI]
        Resto = 0xCAFE
    }
}
