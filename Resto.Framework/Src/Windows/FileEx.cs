using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#if NET40
// Для компиляции под .Net 4.0 нужен пакет Microsoft.Bcl.Async
using Microsoft;
#else
using TaskEx = System.Threading.Tasks.Task;
#endif

namespace Resto.Framework.Windows
{
    public static class FileEx
    {
        public static void MoveWithProgress(string source, string destination, IProgress<double> progress, CancellationToken token)
        {

            NativeMethods.CopyProgressRoutine copyProgressHandler = null;
            if (progress != null)
            {
                copyProgressHandler = (total, transferred, streamSize, streamByteTrans, dwStreamNumber, reason, hSourceFile, hDestinationFile, lpData) =>
                {
                    progress.Report((double)transferred / total * 100);
                    return token.IsCancellationRequested ? NativeMethods.CopyProgressResult.PROGRESS_CANCEL : NativeMethods.CopyProgressResult.PROGRESS_CONTINUE;
                };
            }

            token.ThrowIfCancellationRequested();

            var success = NativeMethods.MoveFileWithProgress(source, destination, copyProgressHandler, IntPtr.Zero,
                NativeMethods.MoveFileFlags.MOVE_FILE_REPLACE_EXISTING |
                NativeMethods.MoveFileFlags.MOVE_FILE_COPY_ALLOWED |
                NativeMethods.MoveFileFlags.MOVE_FILE_WRITE_THROUGH);

            token.ThrowIfCancellationRequested();

            if (!success) throw new Win32Exception();
        }

        public static async Task MoveWithProgressAsync(string source, string destination,
            IProgress<double> progress, CancellationToken token)
        {
            await TaskEx.Run(() => MoveWithProgress(source, destination, progress, token), token)
                .ConfigureAwait(false);
        }


        public static void CopyWithProgress(string sourceFileName, string destFileName,
            IProgress<double> progress, CancellationToken token)
        {
            var pbCancel = 0;
            NativeMethods.CopyProgressRoutine copyProgressHandler = null;
            if (progress != null)
            {
                copyProgressHandler = (total, transferred, streamSize, streamByteTrans, dwStreamNumber, reason, hSourceFile, hDestinationFile, lpData) =>
                {
                    progress.Report((double)transferred / total * 100);
                    return NativeMethods.CopyProgressResult.PROGRESS_CONTINUE;
                };
            }

            token.ThrowIfCancellationRequested();

            using (token.Register(() => pbCancel = 1))
            {
                var success = NativeMethods.CopyFileEx(sourceFileName, destFileName, copyProgressHandler, IntPtr.Zero,
                    ref pbCancel, NativeMethods.CopyFileFlags.COPY_FILE_RESTARTABLE );
                token.ThrowIfCancellationRequested();

                if (!success) throw new Win32Exception();
            }

        }

        public static Task CopyWithProgressAsync(string sourceFileName, string destFileName, IProgress<double> progress)
            => CopyWithProgressAsync(sourceFileName, destFileName, progress, CancellationToken.None);

        public static async Task CopyWithProgressAsync(string sourceFileName, string destFileName, IProgress<double> progress, CancellationToken token)
        {
            var pbCancel = 0;
            NativeMethods.CopyProgressRoutine copyProgressHandler = null;
            if (progress != null)
            {
                copyProgressHandler = (total, transferred, streamSize, streamByteTrans, dwStreamNumber, reason, hSourceFile, hDestinationFile, lpData) =>
                {
                    progress.Report((double)transferred / total * 100);
                    return NativeMethods.CopyProgressResult.PROGRESS_CONTINUE;
                };
            }

            token.ThrowIfCancellationRequested();

            await TaskEx.Run(() =>
            {
                using (token.Register(() => pbCancel = 1))
                {
                    var success = NativeMethods.CopyFileEx(sourceFileName, destFileName, copyProgressHandler, IntPtr.Zero,
                        ref pbCancel, NativeMethods.CopyFileFlags.COPY_FILE_RESTARTABLE);
                    token.ThrowIfCancellationRequested();

                    if (!success) throw new Win32Exception();
                }
            }, token);

        }

        private static class NativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool MoveFileWithProgress(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, MoveFileFlags dwCopyFlags);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref int pbCancel, CopyFileFlags dwCopyFlags);

            internal delegate CopyProgressResult CopyProgressRoutine(long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

            internal enum CopyProgressResult : uint
            {
                PROGRESS_CONTINUE,
                PROGRESS_CANCEL,
                PROGRESS_STOP,
                PROGRESS_QUIET,
            }

            internal enum CopyProgressCallbackReason : uint
            {
                CALLBACK_CHUNK_FINISHED,
                CALLBACK_STREAM_SWITCH,
            }

            [Flags]
            internal enum MoveFileFlags : uint
            {
                MOVE_FILE_REPLACE_EXISTING = 1,
                MOVE_FILE_COPY_ALLOWED = 2,
                MOVE_FILE_DELAY_UNTIL_REBOOT = 4,
                MOVE_FILE_WRITE_THROUGH = 8,
                MOVE_FILE_CREATE_HARDLINK = 16, // 0x00000010
                MOVE_FILE_FAIL_IF_NOT_TRACKABLE = 32, // 0x00000020
            }

            [Flags]
            internal enum CopyFileFlags : uint
            {
                COPY_FILE_FAIL_IF_EXISTS = 1,
                COPY_FILE_RESTARTABLE = 2,
                COPY_FILE_OPEN_SOURCE_FOR_WRITE = 4,
                COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 8,
                COPY_FILE_COPY_SYMLINK = 2048, // 0x00000800
            }
        }
    }
}
