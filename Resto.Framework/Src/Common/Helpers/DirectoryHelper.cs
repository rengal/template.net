using System;
using System.IO;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Helpers
{
    public static class DirectoryHelper
    {
        [NotNull, Pure]
        public static DirectoryInfo[] GetDirectoriesOrEmpty([NotNull] this DirectoryInfo dir)
        {
            if (dir == null)
                throw new ArgumentNullException(nameof(dir));

            return dir.Exists ? dir.GetDirectories() : Array.Empty<DirectoryInfo>();
        }

        public static FileInfo[] GetFilesOrEmpty([NotNull] this DirectoryInfo dir, [NotNull] string searchPattern)
        {
            if (dir == null)
                throw new ArgumentNullException(nameof(dir));

            return dir.Exists ? dir.GetFiles(searchPattern) : Array.Empty<FileInfo>();
        }
    }
}
