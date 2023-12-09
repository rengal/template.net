using System;
using System.Threading;

using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class ReaderWriterLockSlimExtensions
    {
        #region Inner Types
        public struct ReadLock : IDisposable
        {
            private readonly ReaderWriterLockSlim gate;

            public ReadLock([NotNull] ReaderWriterLockSlim gate)
            {
                if (gate == null)
                    throw new ArgumentNullException(nameof(gate));

                this.gate = gate;
                gate.EnterReadLock();
            }

            public void Dispose()
            {
                gate.ExitReadLock();
            }
        }

        public struct UpgradeableReadLock : IDisposable
        {
            private readonly ReaderWriterLockSlim gate;

            public UpgradeableReadLock([NotNull] ReaderWriterLockSlim gate)
            {
                if (gate == null)
                    throw new ArgumentNullException(nameof(gate));

                this.gate = gate;
                gate.EnterUpgradeableReadLock();
            }

            public void Dispose()
            {
                gate.ExitUpgradeableReadLock();
            }
        }

        public struct WriteLock : IDisposable
        {
            private readonly ReaderWriterLockSlim gate;

            public WriteLock([NotNull] ReaderWriterLockSlim gate)
            {
                if (gate == null)
                    throw new ArgumentNullException(nameof(gate));

                this.gate = gate;
                gate.EnterWriteLock();
            }

            public void Dispose()
            {
                gate.ExitWriteLock();
            }
        }
        #endregion

        public static ReadLock GetReadLock([NotNull] this ReaderWriterLockSlim lockSlim)
        {
            return new ReadLock(lockSlim);
        }

        public static UpgradeableReadLock GetUpgradeableReadLock([NotNull] this ReaderWriterLockSlim lockSlim)
        {
            return new UpgradeableReadLock(lockSlim);
        }

        public static WriteLock GetWriteLock([NotNull] this ReaderWriterLockSlim lockSlim)
        {
            return new WriteLock(lockSlim);
        }
    }
}