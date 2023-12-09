using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// The architecture type of a computer.
    /// </summary>
    public enum ArchitectureType : ushort
    {
        /// <summary>
        /// i386.
        /// </summary>
        [PublicAPI]
        X86 = 0x014c,

        /// <summary>
        /// Intel Itanium.
        /// </summary>
        [PublicAPI]
        Ia64 = 0x0200,

        /// <summary>
        /// AMD64.
        /// </summary>
        [PublicAPI]
        X64 = 0x8664
    }
}
