using System.Runtime.InteropServices;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Windows.PortableExecutable
{
    /// <summary>
    /// Represents the data directory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDataDirectory
    {
        /// <summary>
        /// The relative virtual address of the table.
        /// </summary>
        [PublicAPI]
        public readonly uint VirtualAddress;

        /// <summary>
        /// The size of the table, in bytes.
        /// </summary>
        [PublicAPI]
        public readonly uint Size;
    }
}
