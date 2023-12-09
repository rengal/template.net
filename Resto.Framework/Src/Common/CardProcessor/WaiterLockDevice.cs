namespace Resto.Framework.Common.CardProcessor
{
    public sealed class WaiterLockDevice : ReaderDevice
    {
        public string WaiterLockPortName { get; set; }
        public string WaiterLockKeyInsertedPattern { get; set; }
        public string WaiterLockKeyRemovedPattern { get; set; }
    }
}