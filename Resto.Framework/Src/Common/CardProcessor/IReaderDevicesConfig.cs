using System.Collections.Generic;

namespace Resto.Framework.Common.CardProcessor
{
    public interface IReaderDevicesConfig
    {
        List<KeyboardReaderDevice> KeyboardDevices { get; set; }
        List<PosReaderDevice> PosDevices { get; set; }
        WaiterLockDevice WaiterLockDevice { get; set; }

        List<ReaderDevice> GetReaderDevices();
        void Save();
    }
}
