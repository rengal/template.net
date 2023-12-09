using System;

namespace Resto.Framework.Common.CardProcessor
{
    public class ReaderDevice
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ReaderDeviceType Type { get; set; }
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public bool IsAutoSettings { get; set; }

        public static ReaderDevice CreateDevice(ReaderDeviceType type)
        {
            return new ReaderDevice
            {
                Id = Guid.NewGuid(),
                Type = type
            };
        }
    }
}