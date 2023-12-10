using System;

namespace Resto.Common
{
    /// <summary>
    /// Интерфейсы для выполнения операций в background потоках.
    /// </summary>
    public interface ISynchronizer
    {
        IEndSynchronizer BeginInvoke(Delegate handler, object arg);
        IEndSynchronizer BeginInvoke(Delegate handler, object arg, params object[] outArgs);
        bool InvokeRequired { get; }
    }

    public interface IEndSynchronizer
    {
        void WaitForEnd();
    }
}
