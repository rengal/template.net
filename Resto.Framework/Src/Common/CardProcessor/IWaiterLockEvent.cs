namespace Resto.Framework.Common.CardProcessor
{
    public interface IWaiterLockEvent
    {
        bool ErrorEvent { get; }
        bool KeyInserted { get; }
        string KeyNumber { get; }
    }
}