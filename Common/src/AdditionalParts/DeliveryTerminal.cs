using System;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class DeliveryTerminal : IComparable
    {
        /// <summary>
        /// Поддерживает ли терминал общение по протоколу.
        /// </summary>
        public bool IsProtocolSupported(DeliveryPluginProtocol deliveryPluginProtocol)
        {
            if (ProtocolVersion == null)
            {
                return deliveryPluginProtocol.Equals(DeliveryPluginProtocol.ZERO_VERSION);
            }
            return deliveryPluginProtocol.Version <= ProtocolVersion;
        }

        public override string ToString()
        {
            return Name;
        }

        public string DisplayName
        {
            get
            {
                return string.IsNullOrWhiteSpace(CustomName)
                           ? Name
                           : string.Format(Resources.DeliveryTerminalDisplayNameFormat, CustomName, Name);
            }
        }

        /// <summary>
        /// Осуществляет сравнение объектов типа "Терминал доставки".
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            var deliveryTerminal = obj as DeliveryTerminal;
            if (deliveryTerminal != null)
                return name.CompareTo(deliveryTerminal.Name);
            throw new ArgumentException("Object is not a terminal");
        }
    }
}
