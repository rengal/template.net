using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// The state of the image file.
    /// </summary>
    public enum MagicType : ushort
    {
        /// <summary>
        /// The file is an executable 32-bit image.
        /// </summary>
        [PublicAPI]
        Nt32BitImage = 0x10b,

        /// <summary>
        /// The file is an executable 64-bit image.
        /// </summary>
        [PublicAPI]
        Nt64BitImage = 0x20b
    }
}
