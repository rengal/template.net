using System;

namespace Resto.Common
{
    public class PreviousDeviceSettings
    {
        public Guid DeviceId { get; set; }
        public int CashNumber { get; set; }
        public int PortNumber { get; set; }
        public int BaudRate { get; set; }
        public string HostAddress { get; set; }
        public int HostPort { get; set; }
        public bool FullCheque { get; set; }

        public PreviousDeviceSettings(Guid deviceId)
        {
            DeviceId = deviceId;
            CashNumber = 1;
            PortNumber = 1;
            BaudRate = 115200;
        }
    }
}
